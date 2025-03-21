using System;
using MyFinanceApp.Database;
using MyFinanceApp.Factories;
using MyFinanceApp.Facades;
using Xunit;
using MyFinanceApp.Models;

namespace MyFinanceApp.Tests.Facades
{
    public class OperationFacadeTests
    {
        private readonly OperationFacade _operationFacade;
        private readonly BankAccountFacade _accountFacade;
        private readonly IDatabase _db;

        public OperationFacadeTests()
        {
            var factory = new DomainFactory();
            _db = new InMemoryDatabase();

            _accountFacade = new BankAccountFacade(factory, _db);
            _operationFacade = new OperationFacade(factory, _db);

            // Добавим счёт для теста
            _accountFacade.CreateBankAccount(10, "Test Account", 1000m);
        }

        [Fact]
        public void CreateOperation_Income_IncreasesBalance()
        {
            // Act
            var op = _operationFacade.CreateOperation(1, OperationType.Income, 10, 200m, DateTime.Today, "Salary", 0);

            // Assert
            Assert.NotNull(op);
            Assert.Equal(200m, op.Amount);
            var acc = _db.GetBankAccount(10);
            Assert.Equal(1200m, acc.Balance);
        }

        [Fact]
        public void CreateOperation_Expense_DecreasesBalance()
        {
            var op = _operationFacade.CreateOperation(2, OperationType.Expense, 10, 300m, DateTime.Today, "Shopping", 0);

            var acc = _db.GetBankAccount(10);
            Assert.Equal(700m, acc.Balance);
        }

        [Fact]
        public void CreateOperation_NoSuchAccount_Throws()
        {
            Assert.Throws<Exception>(() =>
            {
                _operationFacade.CreateOperation(1, OperationType.Expense, 999, 50m, DateTime.Now, "", 5);
            });

        }
    }
}