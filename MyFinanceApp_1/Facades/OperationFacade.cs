using System;
using MyFinanceApp.Database;
using MyFinanceApp.Factories;
using MyFinanceApp.Models;

namespace MyFinanceApp.Facades
{
    public class OperationFacade
    {
        private readonly DomainFactory _factory;
        private readonly IDatabase _db;

        public OperationFacade(DomainFactory factory, IDatabase db)
        {
            _factory = factory;
            _db = db;
        }

        public Operation CreateOperation(int id, OperationType type, int bankAccountId, 
            decimal amount, DateTime date, 
            string description, int categoryId)
        {
            // Создаём операцию
            var operation = _factory.CreateOperation(
                id, type, bankAccountId, amount, date, description, categoryId);

            // Сохраняем
            _db.AddOperation(operation);

            // Обновляем баланс
            var acc = _db.GetBankAccount(bankAccountId);
            if (acc == null)
                throw new Exception("Счёт не найден.");

            decimal sign = (type == OperationType.Income) ? +1 : -1;
            acc.UpdateBalance(sign * amount);
            _db.UpdateBankAccount(acc);

            return operation;
        }

        // ...
    }
}