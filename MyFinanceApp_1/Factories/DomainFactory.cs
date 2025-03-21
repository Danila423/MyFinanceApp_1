using System;
using MyFinanceApp.Models;

namespace MyFinanceApp.Factories
{
    public class DomainFactory
    {
        public BankAccount CreateBankAccount(int id, string name, decimal initialBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Начальный баланс не может быть отрицательным.");

            return new BankAccount(id, name, initialBalance);
        }

        public Category CreateCategory(int id, OperationType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название категории не может быть пустым.");

            return new Category(id, type, name);
        }

        public Operation CreateOperation(int id, OperationType type, int bankAccountId,
            decimal amount, DateTime date,
            string description, int categoryId)
        {
            // Дополнительная валидация, если нужно
            if (amount < 0)
                throw new ArgumentException("Сумма операции не может быть отрицательной.");

            return new Operation(id, type, bankAccountId, amount, date, description, categoryId);
        }
    }
}