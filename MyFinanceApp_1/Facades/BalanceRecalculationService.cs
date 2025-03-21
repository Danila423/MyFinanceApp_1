using MyFinanceApp.Database;
using System;

namespace MyFinanceApp.Facades
{
    /// <summary>
    /// Допустим, для пересчёта баланса при несоответствиях.
    /// </summary>
    public class BalanceRecalculationService
    {
        private readonly IDatabase _db;

        public BalanceRecalculationService(IDatabase db)
        {
            _db = db;
        }

        public void RecalculateAllAccounts()
        {
            // Примерный алгоритм:
            // 1) Обнулить баланс счёта
            // 2) Пробежать по всем операциям, которые относятся к этому счёту
            //    и заново пересчитать
            var allAccounts = _db.GetAllBankAccounts();
            foreach (var acc in allAccounts)
            {
                var balance = 0m;
                var operations = _db.GetAllOperations();
                foreach (var op in operations)
                {
                    if (op.BankAccountId == acc.Id)
                    {
                        decimal sign = (op.Type == Models.OperationType.Income) ? +1 : -1;
                        balance += sign * op.Amount;
                    }
                }

                acc.UpdateBalance(balance - acc.Balance); 
                _db.UpdateBankAccount(acc);
            }
            Console.WriteLine("Пересчёт балансов завершён.");
        }
    }
}