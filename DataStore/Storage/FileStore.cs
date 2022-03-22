using CRUD_Api.Interfaces;
using CRUD_Api.Model;
using CRUD_Api.Helpers;

namespace CRUD_Api.DataStore
{
    public class FileStore : IDatabase
    {
        private List<ToolModel> _purchaseInformation = new List<ToolModel>();
        string databasePath = @".\database.txt";
        PopulateDataStoreWithBogusData faker;
        string [] initialData = { "Hammer", "Screwdriver", "Mallet", "Axe", "Saw", "Scissors", "Chisel", "Pliers", "Drill", "Tape measure" };

        public FileStore()
        {
            _purchaseInformation = new List<ToolModel>();
            faker = new PopulateDataStoreWithBogusData(ref _purchaseInformation, 5);

            if (!File.Exists(databasePath))
            {
                File.WriteAllLines(databasePath, initialData);
            }
            //_tools = File.ReadAllLines(databasePath).ToList<string>();
        }
        private void Save()
        {
            throw new NotImplementedException();
        }
        private void Load()
        {
            throw new NotImplementedException();
        }

        public List<ToolModel> Get()
        {
            return _purchaseInformation;
        }

        public bool Delete(string existingTool)
        {
            var item = _purchaseInformation.SingleOrDefault<ToolModel>(x => x.Tool == existingTool);
            if (item == null)
                return false;

            _purchaseInformation.Remove(item);
            Save();
            return true;
        }
        public bool Delete(int id)
        {
            var item = _purchaseInformation.Find(x => x.Id == id);
            if (item == null)
                return false;

            _purchaseInformation.Remove(item);
            Save();
            return true;
        }

        public bool Update(string existingTool, string newTool)
        {
            var storeIndex = _purchaseInformation.FindIndex(x => x.Tool == existingTool);
            if (storeIndex == -1)
                return false;

            _purchaseInformation[storeIndex].Tool = newTool;
            return true;
        }

        public ToolModel GetEntity(string tool)
        {
            var item = _purchaseInformation.Find(x => x.Tool == tool);
            if (item == null)
                throw new KeyNotFoundException("The requested item " + tool + " was not found");

            return item;
        }

        public ToolModel GetEntity(int id)
        {
            var item = _purchaseInformation.Find(x => x.Id == id);
            if (item == null)
                throw new KeyNotFoundException("The requested item of id " + id + " was not found");

            return item;
        }

        public void Add(ToolModel newPurchase)
        {
            _purchaseInformation.Add(newPurchase);
        }
    }
}
