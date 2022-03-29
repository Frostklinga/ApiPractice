using CRUD_Api.Interfaces;
using CRUD_Api.Model;
using CRUD_Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;

namespace DataStore.DataStore
{
    public class FileStoreSeparateFiles : IDatabase
    {
        private List<ToolModel> _purchaseInformation = new List<ToolModel>();
        string databasePath = @".\PersistentDataStore\";
        PopulateDataStoreWithBogusData _faker;
        public FileStoreSeparateFiles()
        {

            //PerformanceTest();
            Directory.CreateDirectory(databasePath);
            if (Directory.GetFiles(databasePath).Length == 0)
            {
                MockData(1000);
                WriteStoreToDisk();
            }
            else
                LoadData();
        }
        
        public void LoadData()
        {

            foreach(var f in Directory.GetFiles(databasePath))
            {
                var content = File.ReadAllText(f);
                var model = JsonSerializer.Deserialize<ToolModel>(content);
                Add(model);
            }
        }

        public void MockData(int entities)
        {
            _faker = new PopulateDataStoreWithBogusData(ref _purchaseInformation, entities);
        }
        public void WriteStoreToDisk()
        {
            var options = new JsonSerializerOptions { WriteIndented = false };
            foreach(ToolModel model in _purchaseInformation)
            {
                string serializedData = JsonSerializer.Serialize(model, options);
                File.WriteAllText(databasePath+model.Id.ToString()+".json",serializedData);
            }
        }
        public void Add(ToolModel purchaseInformation)
        {
            purchaseInformation.Id = _purchaseInformation.Last().Id + 1;
            _purchaseInformation.Add(purchaseInformation);
            StoreEntity(purchaseInformation);
        }
        public void StoreEntity(ToolModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = false };
            string serializedData = JsonSerializer.Serialize(model, options);
            File.WriteAllText(databasePath + model.Id.ToString() + ".json", serializedData);
        }
        

        public bool Delete(string tool)
        {
            var tools = _purchaseInformation.FindAll(x => x.Tool == tool);
            var succeeded = tools.Remove(tools[0]);
            return succeeded;
        }

        public bool Delete(int id)
        {
            var tools = _purchaseInformation.FindAll(x => x.Id == id);
            var succeeded = tools.Remove(tools[0]);
            return succeeded;
        }

        public List<ToolModel> Get()
        {
            return _purchaseInformation;
        }

        public ToolModel GetEntity(string tool)
        {
            var entity = _purchaseInformation.Find(x => x.Tool == tool);
            if(entity == null)
            {
                throw new ArgumentNullException("The tool " + tool + " was not found.");
            }
            return entity;
        }

        public ToolModel GetEntity(int id)
        {
            var entity = _purchaseInformation.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new ArgumentNullException("The tool with the requested id " + id.ToString() + " was not found.");
            }
            return entity;
        }

        public bool Update(string existingTool, string newTool)
        {
            var entity = _purchaseInformation.FindIndex(0, _purchaseInformation.Count, x => x.Tool == existingTool);
            if(entity == -1)
            {
                throw new NullReferenceException("The requested tool " + existingTool + " was not found.");
            }
            _purchaseInformation[entity].Tool = newTool;

            return true;
        }
    }
}
