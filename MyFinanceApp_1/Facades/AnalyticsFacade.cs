using System;
using System.Collections.Generic;
using System.Linq;
using MyFinanceApp.Database;
using MyFinanceApp.Models;

namespace MyFinanceApp.Facades
{
    public class AnalyticsFacade
    {
        private readonly IDatabase _db;

        public AnalyticsFacade(IDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// a. Подсчет разницы доходов и расходов за выбранный период.
        /// </summary>
        public decimal GetIncomeExpenseDifference(DateTime start, DateTime end)
        {
            var operations = _db.GetAllOperations()
                .Where(o => o.Date >= start && o.Date <= end);

            decimal totalIncome = operations
                .Where(o => o.Type == OperationType.Income)
                .Sum(o => o.Amount);

            decimal totalExpense = operations
                .Where(o => o.Type == OperationType.Expense)
                .Sum(o => o.Amount);

            return totalIncome - totalExpense;
        }

        /// <summary>
        /// b. Группировка доходов и расходов по категориям за выбранный период.
        /// </summary>
        public Dictionary<string, decimal> GroupOperationsByCategory(DateTime start, DateTime end, OperationType? opType = null)
        {
            var operations = _db.GetAllOperations()
                .Where(o => o.Date >= start && o.Date <= end);

            if (opType.HasValue)
                operations = operations.Where(o => o.Type == opType.Value);

            var result = new Dictionary<string, decimal>();

            foreach (var op in operations)
            {
                var category = _db.GetCategory(op.CategoryId);
                var categoryName = category != null ? category.Name : "Неизвестная категория";

                if (result.ContainsKey(categoryName))
                    result[categoryName] += op.Amount;
                else
                    result[categoryName] = op.Amount;
            }
            return result;
        }

        /// <summary>
        /// c. Дополнительная аналитика:
        /// </summary>
        public Dictionary<string, decimal> GetAverageTransactionAmountByAccount(DateTime start, DateTime end)
        {
            var operations = _db.GetAllOperations()
                .Where(o => o.Date >= start && o.Date <= end)
                .GroupBy(o => o.BankAccountId);

            var result = new Dictionary<string, decimal>();

            foreach (var group in operations)
            {
                var account = _db.GetBankAccount(group.Key);
                var accountName = account != null ? account.Name : $"Счет {group.Key}";
                decimal avgAmount = group.Average(o => o.Amount);
                result[accountName] = avgAmount;
            }
            return result;
        }
    }
}
