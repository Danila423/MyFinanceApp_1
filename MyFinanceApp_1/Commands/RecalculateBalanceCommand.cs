using MyFinanceApp.Facades;

namespace MyFinanceApp.Commands
{
    public class RecalculateBalanceCommand : ICommand
    {
        private readonly BalanceRecalculationService _service;

        public RecalculateBalanceCommand(BalanceRecalculationService service)
        {
            _service = service;
        }

        public void Execute()
        {
            _service.RecalculateAllAccounts();
        }
    }
}