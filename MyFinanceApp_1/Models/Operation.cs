using System;

namespace MyFinanceApp.Models
{
    public class Operation
    {
        public int Id { get; }
        public OperationType Type { get; }
        public int BankAccountId { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Description { get; }
        public int CategoryId { get; }

        public Operation(int id, OperationType type, int bankAccountId, 
            decimal amount, DateTime date, 
            string description, int categoryId)
        {
            if (amount < 0)
                throw new ArgumentException("Сумма операции не может быть отрицательной.");

            Id = id;
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }
    }
}