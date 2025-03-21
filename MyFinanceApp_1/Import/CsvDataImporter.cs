using System;
using System.Linq;
using MyFinanceApp.Factories;
using MyFinanceApp.Database;
using MyFinanceApp.Models;

namespace MyFinanceApp.Import
{
    public class CsvDataImporter : BaseDataImporter
    {
        public CsvDataImporter(IDatabase db, DomainFactory factory) : base(db, factory) { }

        protected override void ParseData(string raw)
        {
            string[] lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            string currentSection = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("==="))
                {
                    if (line.Contains("Счета"))
                        currentSection = "BankAccounts";
                    else if (line.Contains("Категории"))
                        currentSection = "Categories";
                    else if (line.Contains("Операции"))
                        currentSection = "Operations";
                    continue;
                }
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(';');
                if (currentSection == "BankAccounts" && parts[0] == "BankAccount")
                {
                    // BankAccount;Id;Name;Balance
                    int id = int.Parse(parts[1]);
                    string name = parts[2];
                    decimal balance = decimal.Parse(parts[3]);
                    try
                    {
                        _db.AddBankAccount(_factory.CreateBankAccount(id, name, balance));
                    }
                    catch { }
                }
                else if (currentSection == "Categories" && parts[0] == "Category")
                {
                    // Category;Id;Type;Name
                    int id = int.Parse(parts[1]);
                    string typeStr = parts[2];
                    OperationType type = typeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                        ? OperationType.Income : OperationType.Expense;
                    string name = parts[3];
                    try
                    {
                        _db.AddCategory(_factory.CreateCategory(id, type, name));
                    }
                    catch { }
                }
                else if (currentSection == "Operations" && parts[0] == "Operation")
                {
                    // Operation;Id;Type;BankAccountId;Amount;Date;Description;CategoryId
                    if (parts.Length >= 8)
                    {
                        int id = int.Parse(parts[1]);
                        string typeStr = parts[2];
                        OperationType type = typeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                            ? OperationType.Income : OperationType.Expense;
                        int bankAccountId = int.Parse(parts[3]);
                        decimal amount = decimal.Parse(parts[4]);
                        DateTime date = DateTime.Parse(parts[5]);
                        string description = parts[6];
                        int categoryId = int.Parse(parts[7]);
                        try
                        {
                            _db.AddOperation(_factory.CreateOperation(id, type, bankAccountId, amount, date, description, categoryId));
                        }
                        catch { }
                    }
                }
            }
            Console.WriteLine("Импорт CSV завершен.");
        }
    }
}
