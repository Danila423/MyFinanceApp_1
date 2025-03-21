using System.Collections.Generic;
using MyFinanceApp.Models;

namespace MyFinanceApp.Database
{
    /// <summary>
    /// Пример паттерна "Прокси" для кэширования, логирования и т.д.
    /// </summary>
    public class DatabaseProxy : IDatabase
    {
        private readonly IDatabase _realDatabase;

        public DatabaseProxy(IDatabase realDatabase)
        {
            _realDatabase = realDatabase;
        }

        public void AddBankAccount(BankAccount account)
        {
            _realDatabase.AddBankAccount(account);
        }

        public BankAccount GetBankAccount(int id)
        {
            return _realDatabase.GetBankAccount(id);
        }

        public void UpdateBankAccount(BankAccount account)
        {
            _realDatabase.UpdateBankAccount(account);
        }

        public void DeleteBankAccount(int id)
        {
            _realDatabase.DeleteBankAccount(id);
        }

        public IEnumerable<BankAccount> GetAllBankAccounts()
        {
            return _realDatabase.GetAllBankAccounts();
        }

        public void AddCategory(Category category)
        {
            _realDatabase.AddCategory(category);
        }

        public Category GetCategory(int id)
        {
            return _realDatabase.GetCategory(id);
        }

        public void UpdateCategory(Category category)
        {
            _realDatabase.UpdateCategory(category);
        }

        public void DeleteCategory(int id)
        {
            _realDatabase.DeleteCategory(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _realDatabase.GetAllCategories();
        }

        public void AddOperation(Operation operation)
        {
            _realDatabase.AddOperation(operation);
        }

        public Operation GetOperation(int id)
        {
            return _realDatabase.GetOperation(id);
        }

        public void UpdateOperation(Operation operation)
        {
            _realDatabase.UpdateOperation(operation);
        }

        public void DeleteOperation(int id)
        {
            _realDatabase.DeleteOperation(id);
        }

        public IEnumerable<Operation> GetAllOperations()
        {
            return _realDatabase.GetAllOperations();
        }
    }
}
