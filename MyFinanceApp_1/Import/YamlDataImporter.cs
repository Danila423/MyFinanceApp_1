using System;
using System.Linq;
using MyFinanceApp.Factories;
using MyFinanceApp.Database;
using MyFinanceApp.Models;
using System.Collections.Generic;

namespace MyFinanceApp.Import
{
    public class YamlDataImporter : BaseDataImporter
    {
        public YamlDataImporter(IDatabase db, DomainFactory factory) : base(db, factory) { }

        protected override void ParseData(string raw)
        {
            var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            string currentSection = "";
            var currentObject = new Dictionary<string, string>();

            void FlushObject()
            {
                if (currentObject.Count > 0)
                {
                    ProcessYamlObject(currentSection, currentObject);
                    currentObject.Clear();
                }
            }

            foreach (var lineOrig in lines)
            {
                var line = lineOrig.TrimEnd();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (!line.StartsWith(" ") && line.EndsWith(":"))
                {
                    FlushObject();
                    currentSection = line.TrimEnd(':').Trim();
                    continue;
                }

                if (line.TrimStart().StartsWith("- "))
                {
                    FlushObject();
                    string trimmed = line.TrimStart().Substring(2).Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        var kvp = trimmed.Split(new[] { ':' }, 2);
                        if (kvp.Length == 2)
                            currentObject[kvp[0].Trim()] = kvp[1].Trim();
                    }
                }
                else
                {
                    string trimmed = line.Trim();
                    var kvp = trimmed.Split(new[] { ':' }, 2);
                    if (kvp.Length == 2)
                        currentObject[kvp[0].Trim()] = kvp[1].Trim();
                }
            }

            FlushObject();
            Console.WriteLine("Импорт YAML завершен.");
        }

        private void ProcessYamlObject(string section, Dictionary<string, string> objData)
        {
            if (section.Equals("BankAccounts", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    int id = int.Parse(objData["Id"]);
                    string name = objData["Name"];
                    decimal balance = decimal.Parse(objData["Balance"]);
                    _db.AddBankAccount(_factory.CreateBankAccount(id, name, balance));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось добавить BankAccount: {ex.Message}");
                }
            }
            else if (section.Equals("Categories", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    int id = int.Parse(objData["Id"]);
                    string typeStr = objData["Type"];
                    OperationType type = typeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                        ? OperationType.Income : OperationType.Expense;
                    string name = objData["Name"];
                    _db.AddCategory(_factory.CreateCategory(id, type, name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось добавить Category: {ex.Message}");
                }
            }
            else if (section.Equals("Operations", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    int id = int.Parse(objData["Id"]);
                    string typeStr = objData["Type"];
                    OperationType type = typeStr.Equals("Income", StringComparison.OrdinalIgnoreCase)
                        ? OperationType.Income : OperationType.Expense;
                    int bankAccountId = int.Parse(objData["BankAccountId"]);
                    decimal amount = decimal.Parse(objData["Amount"]);
                    DateTime date = DateTime.Parse(objData["Date"]);
                    string description = objData.ContainsKey("Description") ? objData["Description"] : "";
                    int categoryId = int.Parse(objData["CategoryId"]);
                    _db.AddOperation(_factory.CreateOperation(id, type, bankAccountId, amount, date, description, categoryId));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось добавить Operation: {ex.Message}");
                }
            }
        }
    }
}
