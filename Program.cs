using System.Globalization;
using System.Text;

namespace Polyclinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Налаштування для української мови в консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            const int Capacity = 100;
            double[] values = new double[Capacity];
            int count = 0;

            bool running = true;
            while (running)
            {
                ShowMenu();
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": PrintValues(values, count); Pause(); break;
                    case "2":
                        double newValue = ReadDouble("Введіть тривалість прийому (хв, 1..240): ", 1, 240);
                        AddValue(values, ref count, newValue, Capacity);
                        Pause();
                        break;
                    case "3": ShowAbout(); Pause(); break;
                    case "4": ShowCostTable(); Pause(); break;
                    case "5": ShowWorkloadTable(); Pause(); break;
                    case "6": ShowStatistics(values, count); Pause(); break;
                    case "0": running = false; ShowGoodbye(); break;
                    default: ShowInvalidChoice(); Pause(); break;
                }
            }
        }

        // Головне меню застосунку
        static void ShowMenu()
        {
            Console.WriteLine("=== Поліклініка ===");
            Console.WriteLine("Користувач: реєстратор_1");
            Console.WriteLine("1 — показати всі значення (час прийому, хв)");
            Console.WriteLine("2 — додати значення");
            Console.WriteLine("3 — довідка про програму");
            Console.WriteLine("4 — табулювання показників (таблиця)");
            Console.WriteLine("5 — зведена таблиця талонів за спеціальностями та днями");
            Console.WriteLine("6 — показати статистику");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        // Додавання значення до масиву зі зміною лічильника через ref
        static void AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count >= capacity)
            {
                Console.WriteLine("Масив заповнений, місця більше немає.");
                return;
            }

            values[count] = newValue;
            count++;
            Console.WriteLine($"Значення {newValue:F1} додано. Усього записів: {count}.");
        }

        // Виведення вмісту масиву до count таблицею з номерами
        static void PrintValues(double[] values, int count)
        {
            if (count <= 0)
            {
                Console.WriteLine("Масив порожній.");
                return;
            }

            Console.WriteLine("=== Збережені значення (час прийому, хв) ===");
            Console.WriteLine($"{"№",-6}{"Значення",12}");
            Console.WriteLine(new string('-', 18));

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1,-6}{values[i],12:F1}");
            }
        }

        // Пошук найбільшого значення та його індексу через out-параметри
        static bool TryFindMax(double[] values, int count, out double maxValue, out int maxIndex)
        {
            if (count <= 0)
            {
                maxValue = 0;
                maxIndex = -1;
                return false;
            }

            maxValue = values[0];
            maxIndex = 0;

            for (int i = 1; i < count; i++)
            {
                if (values[i] > maxValue)
                {
                    maxValue = values[i];
                    maxIndex = i;
                }
            }

            return true;
        }

        // Обчислення суми та середнього значення через out-параметри
        static void CalculateStatistics(double[] values, int count, out double sum, out double average)
        {
            sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }

            average = count > 0 ? sum / count : 0;
        }

        // Виведення статистики за масивом значень
        static void ShowStatistics(double[] values, int count)
        {
            if (!TryFindMax(values, count, out double maxValue, out int maxIndex))
            {
                Console.WriteLine("Статистика недоступна: масив порожній.");
                return;
            }

            CalculateStatistics(values, count, out double sum, out double average);

            Console.WriteLine("=== Статистика тривалості прийому ===");
            Console.WriteLine($"Кількість записів: {count}");
            Console.WriteLine($"Сума:              {sum:F1} хв");
            Console.WriteLine($"Середнє значення:  {average:F1} хв");
            Console.WriteLine($"Найбільше:         {maxValue:F1} хв (номер у списку: {maxIndex + 1})");
        }

        // Повідомлення про невірний вибір
        static void ShowInvalidChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
        }

        // Прощання при виході
        static void ShowGoodbye()
        {
            Console.WriteLine("До побачення!");
        }

        // Пауза перед очищенням екрана
        static void Pause()
        {
            if (Console.IsInputRedirected) return;
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Пункт 3: Довідка про програму (Варіант 18)
        static void ShowAbout()
        {
            Console.WriteLine("=== Довідка про застосунок ===");
            Console.WriteLine("Тема проєкту: Поліклініка (Варіант 18)");
            Console.WriteLine("Головна сутність: Талон до лікаря.");
            Console.WriteLine("Сутності: Користувач, Талон до лікаря, Спеціальність лікаря (довідник).");
            Console.WriteLine("Перелік полів та їх типи:");
            Console.WriteLine("  1. Номер талона — рядок (string)");
            Console.WriteLine("  2. Прізвище пацієнта — рядок (string)");
            Console.WriteLine("  3. Код спеціальності лікаря — ціле число (int)");
            Console.WriteLine("  4. Кабінет — рядок (string)");
            Console.WriteLine("  5. Дата — дата (DateTime)");
            Console.WriteLine("  6. Час — рядок (string)");
            Console.WriteLine("  7. Чи прийшов пацієнт — логічне (bool)");
            Console.WriteLine("Підсумкова величина: скільки талонів пропущено по кожному лікарю.");
        }

        // Пункт 4: Табулювання функції
        static void ShowCostTable()
        {
            Console.WriteLine("=== Табулювання показників (залежність часу від кількості пацієнтів у черзі) ===");
            double start = ReadDouble("Введіть початкову кількість пацієнтів (1..50): ", 1, 50);
            double end = ReadDouble($"Введіть кінцеву кількість пацієнтів ({start}..100): ", start, 100);
            double step = ReadDouble("Введіть крок табулювання (0.5..20): ", 0.5, 20);
            Console.WriteLine();

            PrintQueueTimeTable(start, end, step);
        }

        // Обчислення орієнтовного часу обслуговування черги (хв)
        static double CalculateQueueTime(double patientCount, double baseMinutes)
        {
            return patientCount * baseMinutes * (1.0 + 0.05 * patientCount);
        }

        // Виведення таблиці табулювання часу черги за межами та кроком
        static void PrintQueueTimeTable(double start, double end, double step)
        {
            if (step <= 0 || start > end)
            {
                Console.WriteLine("Некоректні межі або крок табулювання.");
                return;
            }

            double baseMinutes = 15.0;

            Console.WriteLine($"{"Пацієнтів у черзі",-20}{"Орієнтовний час (хв)",20}");
            Console.WriteLine(new string('-', 40));

            for (double count = start; count <= end + 1e-9; count += step)
            {
                double totalTime = CalculateQueueTime(count, baseMinutes);
                Console.WriteLine($"{count,-20:F1}{totalTime,20:F1}");
            }
        }

        // Пункт 5: Зведена таблиця на два входи з підсумками
        static void ShowWorkloadTable()
        {
            Console.WriteLine("=== Зведена таблиця: видані талони за спеціальностями та днями ===");

            int rows = ReadInt("Введіть кількість спеціальностей (рядків, 1..20): ", 1, 20);
            int cols = ReadInt("Введіть кількість днів (стовпців, 1..14): ", 1, 14);
            Console.WriteLine();

            int[,] table = GenerateWorkloadTable(rows, cols);
            PrintWorkloadTable(table);
        }

        // Генерація таблиці завантаження спеціальностей випадковими значеннями
        static int[,] GenerateWorkloadTable(int rows, int cols)
        {
            int[,] table = new int[rows, cols];
            Random rand = new Random();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    table[r, c] = rand.Next(2, 20);
                }
            }

            return table;
        }

        // Виведення зведеної таблиці та підсумків за рядками і стовпцями
        static void PrintWorkloadTable(int[,] table)
        {
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            if (rows <= 0 || cols <= 0)
            {
                Console.WriteLine("Нема чого виводити.");
                return;
            }

            // Шапка таблиці
            Console.Write($"{"Спеціальн.",-12}");
            for (int c = 1; c <= cols; c++)
            {
                Console.Write($"{("День " + c),8}");
            }
            Console.WriteLine($"{"Підсумок",10}");
            Console.WriteLine(new string('-', 12 + cols * 8 + 10));

            int grandTotal = 0;
            int[] colSums = new int[cols];

            for (int r = 0; r < rows; r++)
            {
                Console.Write($"{("Лікар " + (r + 1)),-12}");
                int rowSum = 0;

                for (int c = 0; c < cols; c++)
                {
                    Console.Write($"{table[r, c],8}");
                    rowSum += table[r, c];
                    colSums[c] += table[r, c];
                }

                Console.WriteLine($"{rowSum,10}");
                grandTotal += rowSum;
            }

            Console.WriteLine(new string('-', 12 + cols * 8 + 10));

            // Рядок підсумків
            Console.Write($"{"Підсумок",-12}");
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{colSums[c],8}");
            }
            Console.WriteLine($"{grandTotal,10}");
        }

        // Безпечне введення дійсного числа в заданих межах
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                string normalized = (input ?? "").Trim().Replace(',', '.');

                if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Помилка: введено не число. Спробуйте ще раз.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути в межах від {min} до {max}. Спробуйте ще раз.");
                    continue;
                }

                return value;
            }
        }

        // Безпечне введення цілого числа в заданих межах
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!int.TryParse((input ?? "").Trim(), out int value))
                {
                    Console.WriteLine("Помилка: введено не ціле число. Спробуйте ще раз.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути в межах від {min} до {max}. Спробуйте ще раз.");
                    continue;
                }

                return value;
            }
        }
    }
}