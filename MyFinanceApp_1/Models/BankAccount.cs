namespace MyFinanceApp.Models
{
    public class BankAccount
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Balance { get; private set; }

        public BankAccount(int id, string name, decimal initialBalance)
        {
            Id = id;
            Name = name;
            Balance = initialBalance;
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
        }
    }
}