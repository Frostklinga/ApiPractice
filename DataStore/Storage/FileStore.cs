using CRUD_Api.Interfaces;
using CRUD_Api.Model;
using CRUD_Api.Helpers;
using System.Text.Json;
using System.Diagnostics;
using DataStore.DataStore;


namespace CRUD_Api.DataStore
{
    public class FileStore : IDatabase
    {
        private List<ToolModel> _purchaseInformation = new List<ToolModel>();
        string databasePath = @".\database.json";
        PopulateDataStoreWithBogusData faker;
        
        public FileStore()
        {

            //Save();
            //var startTime = DateTime.Now;
            //Debug.WriteLine("Began loading at: " + startTime.ToString());

            //Load();
            //var differance = DateTime.Now - startTime;
            //Debug.WriteLine("Loading took: " + differance.ToString());
            //FileStoreSeparateFiles storage = new FileStoreSeparateFiles();
            FileStoreChunks chunks;
            int chunkSize = 100000;
            int enteties = 10000000;
            chunks = new FileStoreChunks(chunkSize, enteties);
            
        }
        private void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = false };
            string serializedData = JsonSerializer.Serialize(_purchaseInformation, options);
            File.WriteAllText(databasePath, serializedData);
        }
        private void Load()
        {
            var jsonData = File.ReadAllText(databasePath);
            _purchaseInformation = JsonSerializer.Deserialize<List<ToolModel>>(jsonData)!;
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
