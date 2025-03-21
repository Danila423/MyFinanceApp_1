using MyFinanceApp.Facades;

namespace MyFinanceApp.Commands
{
    /// <summary>
    /// Команда для создания счёта.
    /// </summary>
    public class CreateBankAccountCommand : ICommand
    {
        private readonly BankAccountFacade _facade;
        private readonly int _id;
        private readonly string _name;
        private readonly decimal _initialBalance;

        public CreateBankAccountCommand(
            BankAccountFacade facade, 
            int id, 
            string name, 
            decimal initialBalance)
        {
            _facade = facade;
            _id = id;
            _name = name;
            _initialBalance = initialBalance;
        }

        public void Execute()
        {
            _facade.CreateBankAccount(_id, _name, _initialBalance);
        }
    }
}