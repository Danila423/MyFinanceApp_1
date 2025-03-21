using MyFinanceApp.Database;
using MyFinanceApp.Factories;
using MyFinanceApp.Facades;
using MyFinanceApp.Models;
using Xunit;

namespace MyFinanceApp.Tests.Facades
{
    public class BankAccountFacadeTests
    {
        private readonly BankAccountFacade _facade;
        private readonly IDatabase _db;

        public BankAccountFacadeTests()
        {
            var factory = new DomainFactory();
            _db = new InMemoryDatabase();
            _facade = new BankAccountFacade(factory, _db);
        }

        [Fact]
        public void CreateAccount_ValidData_AccountIsCreated()
        {
            // Act
            var acc = _facade.CreateBankAccount(1, "Test", 1000m);

            // Assert
            Assert.NotNull(acc);
            Assert.Equal(1, acc.Id);
            Assert.Equal("Test", acc.Name);
            Assert.Equal(1000m, acc.Balance);

            // Проверим, что в базе тоже есть
            var fromDb = _db.GetBankAccount(1);
            Assert.NotNull(fromDb);
        }

        [Fact]
        public void DeleteAccount_RemovesFromDatabase()
        {
            // Arrange
            _facade.CreateBankAccount(2, "ToDelete", 200m);

            // Act
            _facade.DeleteBankAccount(2);

            // Assert
            var acc = _db.GetBankAccount(2);
            Assert.Null(acc);
        }
    }
}