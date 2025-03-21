using System;
using MyFinanceApp.Models;

namespace MyFinanceApp.Export
{
    public class YamlExportVisitor : IExportVisitor
    {
        public void Visit(BankAccount account)
        {
            // Упрощённо
            Console.WriteLine($"- type: BankAccount\n  id: {account.Id}\n  name: {account.Name}\n  balance: {account.Balance}");
        }

        public void Visit(Category category)
        {
            Console.WriteLine($"- type: Category\n  id: {category.Id}\n  categoryType: {category.Type}\n  name: {category.Name}");
        }

        public void Visit(Operation operation)
        {
            Console.WriteLine($"- type: Operation\n  id: {operation.Id}\n  opType: {operation.Type}\n  amount: {operation.Amount}\n  date: {operation.Date}\n  description: {operation.Description}");
        }
    }
}