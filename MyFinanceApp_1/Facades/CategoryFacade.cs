using MyFinanceApp.Database;
using MyFinanceApp.Factories;
using MyFinanceApp.Models;
using System.Collections.Generic;

namespace MyFinanceApp.Facades
{
    public class CategoryFacade
    {
        private readonly DomainFactory _factory;
        private readonly IDatabase _db;

        public CategoryFacade(DomainFactory factory, IDatabase db)
        {
            _factory = factory;
            _db = db;
        }

        public Category CreateCategory(int id, OperationType type, string name)
        {
            var cat = _factory.CreateCategory(id, type, name);
            _db.AddCategory(cat);
            return cat;
        }

        public Category GetCategory(int id)
        {
            return _db.GetCategory(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _db.GetAllCategories();
        }

        // ...
    }
}