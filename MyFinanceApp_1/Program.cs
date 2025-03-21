using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyFinanceApp.Factories;
using MyFinanceApp.Database;
using MyFinanceApp.Facades;
using MyFinanceApp.Models;
using MyFinanceApp.Import;
using MyFinanceApp.Commands;

namespace MyFinanceApp
{
    internal class Program
    {
        private static BaseDataImporter GetImporterByExtension(string filePath, IDatabase db, DomainFactory factory)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".csv"  => new CsvDataImporter(db, factory),
                ".json" => new JsonDataImporter(db, factory),
                ".yaml" or ".yml" => new YamlDataImporter(db, factory),
                _ => throw new Exception("Неподдерживаемый формат файла")
            };
        }

        static void Main(string[] args)
        {
            // 1. Создаём HostBuilder, регистрируем все наши сервисы
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<DomainFactory>();
                    services.AddSingleton<IDatabase, InMemoryDatabase>();

                    // Регистрируем фасады
                    services.AddSingleton<BankAccountFacade>();
                    services.AddSingleton<CategoryFacade>();
                    services.AddSingleton<OperationFacade>();
                    services.AddSingleton<AnalyticsFacade>();
                    services.AddSingleton<BalanceRecalculationService>();

                    // Регистрируем Invoker (Команда + Декоратор)
                    services.AddSingleton<CommandInvoker>();

                })
                .Build();

            // 2. Получаем нужные сервисы из DI
            var factory  = host.Services.GetRequiredService<DomainFactory>();
            var db       = host.Services.GetRequiredService<IDatabase>();
            var accountF = host.Services.GetRequiredService<BankAccountFacade>();
            var categoryF= host.Services.GetRequiredService<CategoryFacade>();
            var operF    = host.Services.GetRequiredService<OperationFacade>();
            var analyticF= host.Services.GetRequiredService<AnalyticsFacade>();
            var balanceS = host.Services.GetRequiredService<BalanceRecalculationService>();

            // 3. Инициализация данных
            accountF.CreateBankAccount(1, "Основной счет", 1000m);
            categoryF.CreateCategory(1, OperationType.Income, "Зарплата");
            categoryF.CreateCategory(2, OperationType.Expense, "Кафе");
            categoryF.CreateCategory(3, OperationType.Expense, "Здоровье");
            
            var invoker = host.Services.GetRequiredService<CommandInvoker>();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MyFinanceApp Menu (with DI) ===");
                Console.WriteLine("1. Создать счёт (замер времени, Команда+Декоратор)");
                Console.WriteLine("2. Создать категорию");
                Console.WriteLine("3. Добавить операцию");
                Console.WriteLine("4. Показать все счета");
                Console.WriteLine("5. Показать все категории");
                Console.WriteLine("6. Показать все операции");
                Console.WriteLine("7. Подсчитать разницу доходов и расходов за период");
                Console.WriteLine("8. Группировать операции по категориям");
                Console.WriteLine("9. Средняя сумма операций по счетам");
                Console.WriteLine("10. Пересчитать баланс (Команда+декоратор)");
                Console.WriteLine("11. Экспорт данных");
                Console.WriteLine("12. Импорт данных");
                Console.WriteLine("13. Выход");
                Console.Write("Выберите опцию: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        try
                        {
                            Console.Write("Введите id счёта: ");
                            int accountId = int.Parse(Console.ReadLine());
                            Console.Write("Введите название счёта: ");
                            string accountName = Console.ReadLine();
                            Console.Write("Введите начальный баланс: ");
                            decimal initialBalance = decimal.Parse(Console.ReadLine());

                            var cmd = new CreateBankAccountCommand(accountF, accountId, accountName, initialBalance);
                            // measureTime = true => обернёт в TimeMeasureDecorator
                            invoker.AddCommand(cmd, measureTime: true);
                            invoker.ExecuteAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "2":
                        try
                        {
                            Console.Write("Введите id категории: ");
                            int categoryId = int.Parse(Console.ReadLine());
                            Console.Write("Введите название категории: ");
                            string categoryName = Console.ReadLine();
                            Console.Write("Введите тип категории (Income/Expense): ");
                            string typeStr = Console.ReadLine();
                            OperationType type = typeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                                ? OperationType.Income : OperationType.Expense;

                            var cmd = new CreateCategoryCommand(categoryF, categoryId, type, categoryName);
                            invoker.AddCommand(cmd, measureTime: true);
                            invoker.ExecuteAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "3":
                        try
                        {
                            Console.Write("Введите id операции: ");
                            int opId = int.Parse(Console.ReadLine());
                            Console.Write("Введите тип (Income/Expense): ");
                            string opTypeStr = Console.ReadLine();
                            OperationType opType = opTypeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                                ? OperationType.Income : OperationType.Expense;
                            Console.Write("Введите id счёта: ");
                            int accId = int.Parse(Console.ReadLine());
                            Console.Write("Введите сумму операции: ");
                            decimal amount = decimal.Parse(Console.ReadLine());
                            Console.Write("Введите дату (yyyy-mm-dd): ");
                            DateTime date = DateTime.Parse(Console.ReadLine());
                            Console.Write("Описание (необязательно): ");
                            string desc = Console.ReadLine();
                            Console.Write("Введите id категории: ");
                            int catId = int.Parse(Console.ReadLine());

                            var cmd = new CreateOperationCommand(operF, opId, opType, accId, amount, date, desc, catId);
                            invoker.AddCommand(cmd, measureTime: true);
                            invoker.ExecuteAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "4":
                        Console.WriteLine("Список счетов:");
                        foreach (var acc in db.GetAllBankAccounts())
                        {
                            Console.WriteLine($"Id: {acc.Id}, Название: {acc.Name}, Баланс: {acc.Balance}");
                        }
                        break;

                    case "5":
                        Console.WriteLine("Список категорий:");
                        foreach (var cat in db.GetAllCategories())
                        {
                            Console.WriteLine($"Id: {cat.Id}, Название: {cat.Name}, Тип: {cat.Type}");
                        }
                        break;

                    case "6":
                        Console.WriteLine("Список операций:");
                        foreach (var op in db.GetAllOperations())
                        {
                            Console.WriteLine($"Id: {op.Id}, Тип: {op.Type}, Счёт: {op.BankAccountId}, Сумма: {op.Amount}, Дата: {op.Date.ToShortDateString()}, Описание: {op.Description}, Категория: {op.CategoryId}");
                        }
                        break;

                    case "7":
                        try
                        {
                            Console.Write("Начальная дата (yyyy-mm-dd): ");
                            DateTime s = DateTime.Parse(Console.ReadLine());
                            Console.Write("Конечная дата (yyyy-mm-dd): ");
                            DateTime e = DateTime.Parse(Console.ReadLine());
                            decimal diff = analyticF.GetIncomeExpenseDifference(s, e);
                            Console.WriteLine($"Разница доходов и расходов: {diff}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "8":
                        try
                        {
                            Console.Write("Начальная дата (yyyy-mm-dd): ");
                            DateTime s = DateTime.Parse(Console.ReadLine());
                            Console.Write("Конечная дата (yyyy-mm-dd): ");
                            DateTime e = DateTime.Parse(Console.ReadLine());
                            var groups = analyticF.GroupOperationsByCategory(s, e);
                            Console.WriteLine("Группировка по категориям:");
                            foreach (var kvp in groups)
                            {
                                Console.WriteLine($"Категория {kvp.Key}, Сумма: {kvp.Value}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "9":
                        try
                        {
                            Console.Write("Начальная дата (yyyy-mm-dd): ");
                            DateTime s = DateTime.Parse(Console.ReadLine());
                            Console.Write("Конечная дата (yyyy-mm-dd): ");
                            DateTime e = DateTime.Parse(Console.ReadLine());
                            var averages = analyticF.GetAverageTransactionAmountByAccount(s, e);
                            Console.WriteLine("Средняя сумма по счетам:");
                            foreach (var kvp in averages)
                            {
                                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                        break;

                    case "10":
                        // Пересчитать баланс
                        {
                            var cmd = new RecalculateBalanceCommand(balanceS);
                            invoker.AddCommand(cmd, measureTime: true);
                            invoker.ExecuteAll();
                        }
                        break;

                    case "11": // Экспорт
                        try
                        {
                            Console.Write("Введите формат (1=CSV,2=JSON,3=YAML): ");
                            var exp = Console.ReadLine();

                            Console.Write("Путь к директории: ");
                            var dir = Console.ReadLine();
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                                Console.WriteLine("Директория создана.");
                            }

                            string filename = exp switch
                            {
                                "1" => "export.csv",
                                "2" => "export.json",
                                "3" => "export.yaml",
                                _   => "export.csv"
                            };
                            string path = Path.Combine(dir, filename);

                            var sb = new StringBuilder();
                            // Упрощённо — всё в одном формате
                            sb.AppendLine("=== Счета ===");
                            foreach (var a in db.GetAllBankAccounts())
                                sb.AppendLine($"BankAccount;{a.Id};{a.Name};{a.Balance}");

                            sb.AppendLine("=== Категории ===");
                            foreach (var c in db.GetAllCategories())
                                sb.AppendLine($"Category;{c.Id};{c.Type};{c.Name}");

                            sb.AppendLine("=== Операции ===");
                            foreach (var o in db.GetAllOperations())
                                sb.AppendLine($"Operation;{o.Id};{o.Type};{o.BankAccountId};{o.Amount};{o.Date};{o.Description};{o.CategoryId}");

                            File.WriteAllText(path, sb.ToString());
                            Console.WriteLine($"Экспортировано в {path}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка экспорта: " + ex.Message);
                        }
                        break;

                    case "12": // Импорт
                        try
                        {
                            Console.Write("Укажите путь к файлу/директории: ");
                            var importPath = Console.ReadLine();
                            if (File.Exists(importPath))
                            {
                                var importer = GetImporterByExtension(importPath, db, factory);
                                importer.ImportData(importPath);
                            }
                            else if (Directory.Exists(importPath))
                            {
                                var files = Directory.GetFiles(importPath);
                                if (files.Length == 0)
                                {
                                    Console.WriteLine("В директории нет файлов");
                                    break;
                                }
                                Console.WriteLine("Файлы:");
                                for (int i = 0; i < files.Length; i++)
                                    Console.WriteLine($"{i+1}. {files[i]}");

                                Console.Write("Выберите файл: ");
                                int idx = int.Parse(Console.ReadLine());
                                if (idx<1 || idx>files.Length) break;
                                var file = files[idx-1];

                                var importer = GetImporterByExtension(file, db, factory);
                                importer.ImportData(file);
                            }
                            else
                            {
                                Console.WriteLine("Указанный путь не существует.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка импорта: " + ex.Message);
                        }
                        break;

                    case "13":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверная опция");
                        break;
                }
            }

            Console.WriteLine("Приложение завершено.");
        }
    }
}
