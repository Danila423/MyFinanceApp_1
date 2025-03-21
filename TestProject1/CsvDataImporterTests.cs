using System.IO;
using MyFinanceApp.Factories;
using MyFinanceApp.Database;
using MyFinanceApp.Import;
using Xunit;

namespace MyFinanceApp.Tests.Import
{
    public class CsvDataImporterTests
    {
        [Fact]
        public void ImportData_ValidCsv_ImportsSuccessfully()
        {
            // Arrange
            string csvContent = @"
=== Счета ===
BankAccount;1;TestAccount;1000
=== Категории ===
Category;10;Income;Salary
=== Операции ===
Operation;100;Income;1;500;2023-01-01;Test Operation;10
";
            var factory = new DomainFactory();
            var db = new InMemoryDatabase();
            var importer = new CsvDataImporter(db, factory);

            // Создадим временный файл
            string filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, csvContent);

            // Act
            importer.ImportData(filePath);

            // Assert
            var acc = db.GetBankAccount(1);
            Assert.NotNull(acc);
            Assert.Equal("TestAccount", acc.Name);
            Assert.Equal(1000m, acc.Balance);

            var cat = db.GetCategory(10);
            Assert.NotNull(cat);
            Assert.Equal("Salary", cat.Name);

            var op = db.GetOperation(100);
            Assert.NotNull(op);
            Assert.Equal(500m, op.Amount);

            File.Delete(filePath);
        }
    }
}