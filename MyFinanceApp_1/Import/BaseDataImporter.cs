using System.IO;
using MyFinanceApp.Factories;
using MyFinanceApp.Database;

namespace MyFinanceApp.Import
{
    public abstract class BaseDataImporter
    {
        protected IDatabase _db;
        protected DomainFactory _factory;

        public BaseDataImporter(IDatabase db, DomainFactory factory)
        {
            _db = db;
            _factory = factory;
        }

        public void ImportData(string filePath)
        {
            string raw = File.ReadAllText(filePath);
            ParseData(raw);
        }

        protected abstract void ParseData(string raw);
    }
}