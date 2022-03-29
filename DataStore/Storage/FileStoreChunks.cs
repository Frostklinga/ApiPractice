using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CRUD_Api.Helpers;
using CRUD_Api.Interfaces;
using CRUD_Api.Model;

namespace DataStore.DataStore
{
    class FileStoreChunks : IDatabase
    {
        private List<ToolModel> _purchaseInformation = new List<ToolModel>();
        string databasePath = @".\PersistentDataStore\";
        PopulateDataStoreWithBogusData _faker;
        int chunkSize = 0;
        int entities;
        public FileStoreChunks(int pChunkSize,int pEnteties)
        {
            chunkSize = pChunkSize;
            entities = pEnteties;
            PerformanceTest();
        }
        public void PerformanceTest()
        {
            int performanceTestIterations = 100;
            int entries = entities;
            for (int i = 0; i < performanceTestIterations; i++)
            {
                Directory.Delete(databasePath, true);
                Directory.CreateDirectory(databasePath);
                entries = entries + chunkSize;
                _purchaseInformation.Clear();
                var beganMock = DateTime.Now;
                MockData(entries);
                var mockDuration = DateTime.Now - beganMock;

                var beganStoreChunk = DateTime.Now;
                StoreChunk(_purchaseInformation[0]);
                var chunkStoreDuration = DateTime.Now - beganStoreChunk;

                var beganWriteToDisk = DateTime.Now;
                WriteStoreToDisk();
                var writeDuration = DateTime.Now - beganWriteToDisk;
                Debug.WriteLine("Mocked " + entries.ToString() + " in " + mockDuration.ToString());
                Debug.WriteLine("Wrote a chunk of chunksize " + chunkSize + " to disk in " + chunkStoreDuration.ToString());
                Debug.WriteLine("Wrote " + entries.ToString() + " to disk in " + writeDuration.ToString());
                Debug.WriteLine("Total number of files: " + _purchaseInformation.Count / chunkSize);
            }
        }
        public void MockData(int entities)
        {
            _faker = new PopulateDataStoreWithBogusData(ref _purchaseInformation, entities);
        }
        public void WriteStoreToDisk()
        {
            var options = new JsonSerializerOptions { WriteIndented = false };
            for(int i=0;i<_purchaseInformation.Count;i=i+chunkSize)
            {
                var data = _purchaseInformation.GetRange(i,chunkSize);
                string serializedData = JsonSerializer.Serialize(data, options);
                File.WriteAllText(GetChunkFileName(data[0]), serializedData);
            }
        }
        public void ClearStorage()
        {
            Directory.Delete(databasePath, true);
        }
        public void Add(ToolModel purchaseInformation)
        {
            purchaseInformation.Id = _purchaseInformation.Last().Id + 1;
            _purchaseInformation.Add(purchaseInformation);
        }
        public string GetChunkFileName(ToolModel model)
        {
            int chunkStart = CalculateChunkStart(model);
            int chunkEnd = chunkStart + chunkSize;
            return databasePath + chunkStart + "-" + chunkEnd + ".json";
        }
        public void StoreChunk(ToolModel model)
        {
            int chunkStart = CalculateChunkStart(model);
            var options = new JsonSerializerOptions { WriteIndented = false };
            int chunkToBeWritten = chunkSize;
            if (chunkStart >= _purchaseInformation.Count 
                || 
                (chunkStart + chunkSize) >= _purchaseInformation.Count)
                    chunkToBeWritten = (_purchaseInformation.Count % chunkSize);
            if (chunkToBeWritten == 0)
                return;
            var data = _purchaseInformation.GetRange(chunkStart, chunkToBeWritten);
            string serializedData = JsonSerializer.Serialize(data, options);
            File.WriteAllText(GetChunkFileName(model), serializedData);
        }
        public int CalculateChunkStart(ToolModel model)
        {
            if (model.Id < chunkSize)
                return 0;
            int chunkStart = model.Id - (model.Id % chunkSize);
            if (chunkStart > _purchaseInformation.Count)
                chunkStart -= chunkSize;
            return chunkStart;
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
            if (entity == null)
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
            if (entity == -1)
            {
                throw new NullReferenceException("The requested tool " + existingTool + " was not found.");
            }
            _purchaseInformation[entity].Tool = newTool;

            return true;
        }
    }
}
