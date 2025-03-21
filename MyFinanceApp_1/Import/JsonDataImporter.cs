using System;
using System.Text.Json;
using MyFinanceApp.Factories;
using MyFinanceApp.Database;
using MyFinanceApp.Models;

namespace MyFinanceApp.Import
{
    public class JsonDataImporter : BaseDataImporter
    {
        public JsonDataImporter(IDatabase db, DomainFactory factory) : base(db, factory) { }

        protected override void ParseData(string raw)
        {
            try
            {
                var exportData = JsonSerializer.Deserialize<ExportData>(raw);
                if (exportData != null)
                {
                    if (exportData.BankAccounts != null)
                    {
                        foreach (var acc in exportData.BankAccounts)
                        {
                            try
                            {
                                _db.AddBankAccount(_factory.CreateBankAccount(acc.Id, acc.Name, acc.Balance));
                            }
                            catch { }
                        }
                    }
                    if (exportData.Categories != null)
                    {
                        foreach (var cat in exportData.Categories)
                        {
                            OperationType type = cat.Type.Equals("Income", StringComparison.OrdinalIgnoreCase)
                                ? OperationType.Income : OperationType.Expense;
                            try
                            {
                                _db.AddCategory(_factory.CreateCategory(cat.Id, type, cat.Name));
                            }
                            catch { }
                        }
                    }
                    if (exportData.Operations != null)
                    {
                        foreach (var op in exportData.Operations)
                        {
                            OperationType type = op.Type.Equals("Income", StringComparison.OrdinalIgnoreCase)
                                ? OperationType.Income : OperationType.Expense;
                            try
                            {
                                _db.AddOperation(_factory.CreateOperation(
                                    op.Id, type, op.BankAccountId, op.Amount, op.Date, op.Description, op.CategoryId
                                ));
                            }
                            catch { }
                        }
                    }
                }
                Console.WriteLine("Импорт JSON завершен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка импорта JSON: " + ex.Message);
            }
        }

        // DTO-классы
        public class ExportData
        {
            public BankAccountDto[] BankAccounts { get; set; }
            public CategoryDto[] Categories { get; set; }
            public OperationDto[] Operations { get; set; }
        }
        public class BankAccountDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Balance { get; set; }
        }
        public class CategoryDto
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
        }
        public class OperationDto
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int BankAccountId { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public int CategoryId { get; set; }
        }
    }
}
