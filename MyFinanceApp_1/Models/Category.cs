namespace MyFinanceApp.Models
{
    public enum OperationType
    {
        Income,
        Expense
    }

    public class Category
    {
        public int Id { get; }
        public OperationType Type { get; }
        public string Name { get; }

        public Category(int id, OperationType type, string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}