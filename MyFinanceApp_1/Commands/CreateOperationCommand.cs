using MyFinanceApp.Facades;
using MyFinanceApp.Models;
using System;

namespace MyFinanceApp.Commands
{
    public class CreateOperationCommand : ICommand
    {
        private readonly OperationFacade _facade;
        private readonly int _id;
        private readonly OperationType _type;
        private readonly int _bankAccountId;
        private readonly decimal _amount;
        private readonly DateTime _date;
        private readonly string _description;
        private readonly int _categoryId;

        public CreateOperationCommand(
            OperationFacade facade,
            int id,
            OperationType type,
            int bankAccountId,
            decimal amount,
            DateTime date,
            string description,
            int categoryId)
        {
            _facade = facade;
            _id = id;
            _type = type;
            _bankAccountId = bankAccountId;
            _amount = amount;
            _date = date;
            _description = description;
            _categoryId = categoryId;
        }

        public void Execute()
        {
            _facade.CreateOperation(_id, _type, _bankAccountId, _amount, _date, _description, _categoryId);
        }
    }
}