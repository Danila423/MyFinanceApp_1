using System;
using MyFinanceApp.Models;

namespace MyFinanceApp.Export
{
    public class JsonExportVisitor : IExportVisitor
    {
        public void Visit(BankAccount account)
        {
            // Упрощённо формируем json
            Console.WriteLine($"{{\"Type\":\"BankAccount\",\"Id\":{account.Id},\"Name\":\"{account.Name}\",\"Balance\":{account.Balance}}}");
        }

        public void Visit(Category category)
        {
            Console.WriteLine($"{{\"Type\":\"Category\",\"Id\":{category.Id},\"CategoryType\":\"{category.Type}\",\"Name\":\"{category.Name}\"}}");
        }

        public void Visit(Operation operation)
        {
            Console.WriteLine($"{{\"Type\":\"Operation\",\"Id\":{operation.Id},\"OpType\":\"{operation.Type}\",\"Amount\":{operation.Amount},\"Date\":\"{operation.Date}\",\"Desc\":\"{operation.Description}\"}}");
        }
    }
}