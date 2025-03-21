using MyFinanceApp.Database;
using MyFinanceApp.Factories;
using MyFinanceApp.Models;

namespace MyFinanceApp.Facades
{
    public class BankAccountFacade
    {
        private readonly DomainFactory _factory;
        private readonly IDatabase _db;

        public BankAccountFacade(DomainFactory factory, IDatabase db)
        {
            _factory = factory;
            _db = db;
        }

        public BankAccount CreateBankAccount(int id, string name, decimal initialBalance)
        {
            var acc = _factory.CreateBankAccount(id, name, initialBalance);
            _db.AddBankAccount(acc);
            return acc;
        }

        public BankAccount GetBankAccount(int id)
        {
            return _db.GetBankAccount(id);
        }

        public void DeleteBankAccount(int id)
        {
            _db.DeleteBankAccount(id);
        }

        // ... Любые другие операции (обновить, список всех и т.д.)
    }
}