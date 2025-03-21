using MyFinanceApp.Models;
using System.Collections.Generic;

namespace MyFinanceApp.Database
{
    public interface IDatabase
    {
        // Счета
        void AddBankAccount(BankAccount account);
        BankAccount GetBankAccount(int id);
        void UpdateBankAccount(BankAccount account);
        void DeleteBankAccount(int id);
        IEnumerable<BankAccount> GetAllBankAccounts();

        // Категории
        void AddCategory(Category category);
        Category GetCategory(int id);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        IEnumerable<Category> GetAllCategories();

        // Операции
        void AddOperation(Operation operation);
        Operation GetOperation(int id);
        void UpdateOperation(Operation operation);
        void DeleteOperation(int id);
        IEnumerable<Operation> GetAllOperations();
    }
}