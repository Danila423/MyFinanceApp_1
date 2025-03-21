using MyFinanceApp.Models;

namespace MyFinanceApp.Export
{
    public interface IExportVisitor
    {
        void Visit(BankAccount account);
        void Visit(Category category);
        void Visit(Operation operation);
    }
}