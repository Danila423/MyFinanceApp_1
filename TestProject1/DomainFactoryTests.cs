using System;
using MyFinanceApp.Factories;
using MyFinanceApp.Models;
using Xunit;

namespace MyFinanceApp.Tests.Factories
{
    public class DomainFactoryTests
    {
        private readonly DomainFactory _factory;

        public DomainFactoryTests()
        {
            _factory = new DomainFactory();
        }

        [Fact]
        public void CreateBankAccount_PositiveInitialBalance_Success()
        {
            // Act
            var acc = _factory.CreateBankAccount(100, "Test Account", 500m);

            // Assert
            Assert.NotNull(acc);
            Assert.Equal(100, acc.Id);
            Assert.Equal("Test Account", acc.Name);
            Assert.Equal(500m, acc.Balance);
        }

        [Fact]
        public void CreateBankAccount_NegativeBalance_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateBankAccount(101, "Neg Account", -100m);
            });
        }

        [Fact]
        public void CreateOperation_NegativeAmount_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateOperation(1, OperationType.Expense, 10, -50m, DateTime.Now, "", 5);
            });
        }

        [Fact]
        public void CreateCategory_EmptyName_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateCategory(1, OperationType.Income, "");
            });
        }
    }
}