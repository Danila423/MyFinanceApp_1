using MyFinanceApp.Facades;
using MyFinanceApp.Models;

namespace MyFinanceApp.Commands
{
    public class CreateCategoryCommand : ICommand
    {
        private readonly CategoryFacade _facade;
        private readonly int _id;
        private readonly OperationType _type;
        private readonly string _name;

        public CreateCategoryCommand(
            CategoryFacade facade,
            int id,
            OperationType type,
            string name)
        {
            _facade = facade;
            _id = id;
            _type = type;
            _name = name;
        }

        public void Execute()
        {
            _facade.CreateCategory(_id, _type, _name);
        }
    }
}