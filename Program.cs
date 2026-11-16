using System;
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

            bool running = true;
            while (running)
            {
                Console.WriteLine("=== Поліклініка ===");
                Console.WriteLine("Користувач: реєстратор_1");
                Console.WriteLine("1 — показати всі талони (буде пізніше)");
                Console.WriteLine("2 — додати новий талон (буде пізніше)");
                Console.WriteLine("3 — довідка про програму");
                Console.WriteLine("4 — табулювання показників (таблиця)");
                Console.WriteLine("5 — зведена таблиця талонів за спеціальностями та днями");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                    case "2":
                        Console.WriteLine("Цей пункт з'явиться в наступних лабораторних роботах.");
                        Pause();
                        break;
                    case "3":
                        ShowAbout();
                        Pause();
                        break;
                    case "4":
                        ShowCostTable();
                        Pause();
                        break;
                    case "5":
                        ShowWorkloadTable();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення!");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        // Пауза перед очищенням екрана
        static void Pause()
        {
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
            double baseMinutes = 15.0;

            Console.WriteLine($"{"Пацієнтів у черзі",-20}{"Орієнтовний час (хв)",20}");
            Console.WriteLine(new string('-', 40));

            for (double count = 1.0; count <= 10.0 + 1e-9; count += 1.0)
            {
                double totalTime = count * baseMinutes * (1.0 + 0.05 * count);
                Console.WriteLine($"{count,-20:F0}{totalTime,20:F1}");
            }
        }

        // Пункт 5: Зведена таблиця на два входи з підсумками
        static void ShowWorkloadTable()
        {
            Console.WriteLine("=== Зведена таблиця: видані талони за спеціальностями та днями ===");

            int rows = ReadPositiveInt("Введіть кількість спеціальностей (рядків): ");
            if (rows <= 0) return;

            int cols = ReadPositiveInt("Введіть кількість днів (стовпців): ");
            if (cols <= 0) return;

            int[,] table = new int[rows, cols];
            Random rand = new Random();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    table[r, c] = rand.Next(2, 20);
                }
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

        static int ReadPositiveInt(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Помилка: введено нуль, від'ємне число або не число. Повернення в меню.");
            return -1;
        }
    }
}