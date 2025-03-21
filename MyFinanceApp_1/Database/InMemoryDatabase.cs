using System.Collections.Generic;
using MyFinanceApp.Models;

namespace MyFinanceApp.Database
{
    public class InMemoryDatabase : IDatabase
    {
        private readonly Dictionary<int, BankAccount> _bankAccounts = new();
        private readonly Dictionary<int, Category> _categories = new();
        private readonly Dictionary<int, Operation> _operations = new();

        public void AddBankAccount(BankAccount account)
        {
            if (_bankAccounts.ContainsKey(account.Id))
                throw new System.Exception("Счёт с таким ID уже существует.");
            _bankAccounts[account.Id] = account;
        }

        public BankAccount GetBankAccount(int id)
        {
            _bankAccounts.TryGetValue(id, out var acc);
            return acc;
        }

        public void UpdateBankAccount(BankAccount account)
        {
            if (!_bankAccounts.ContainsKey(account.Id))
                throw new System.Exception("Счёт не найден для обновления.");
            _bankAccounts[account.Id] = account;
        }

        public void DeleteBankAccount(int id)
        {
            _bankAccounts.Remove(id);
        }

        public IEnumerable<BankAccount> GetAllBankAccounts()
        {
            return _bankAccounts.Values;
        }

        public void AddCategory(Category category)
        {
            if (_categories.ContainsKey(category.Id))
                throw new System.Exception("Категория с таким ID уже существует.");
            _categories[category.Id] = category;
        }

        public Category GetCategory(int id)
        {
            _categories.TryGetValue(id, out var cat);
            return cat;
        }

        public void UpdateCategory(Category category)
        {
            if (!_categories.ContainsKey(category.Id))
                throw new System.Exception("Категория не найдена для обновления.");
            _categories[category.Id] = category;
        }

        public void DeleteCategory(int id)
        {
            _categories.Remove(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categories.Values;
        }

        public void AddOperation(Operation operation)
        {
            if (_operations.ContainsKey(operation.Id))
                throw new System.Exception("Операция с таким ID уже существует.");
            _operations[operation.Id] = operation;
        }

        public Operation GetOperation(int id)
        {
            _operations.TryGetValue(id, out var op);
            return op;
        }

        public void UpdateOperation(Operation operation)
        {
            if (!_operations.ContainsKey(operation.Id))
                throw new System.Exception("Операция не найдена для обновления.");
            _operations[operation.Id] = operation;
        }

        public void DeleteOperation(int id)
        {
            _operations.Remove(id);
        }

        public IEnumerable<Operation> GetAllOperations()
        {
            return _operations.Values;
        }
    }
}
