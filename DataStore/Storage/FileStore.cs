using CRUD_Api.Interfaces;
using CRUD_Api.Model;

namespace CRUD_Api.DataStore
{
    public class FileStore : IDatabase
    {
        private List<string> _tools;
        string databasePath = @".\database.txt";

        string [] initialData = { "Hammer", "Screwdriver", "Mallet", "Axe", "Saw", "Scissors", "Chisel", "Pliers", "Drill", "Tape measure" };
        public FileStore()
        {
            _tools = new List<string>();

            if(!File.Exists(databasePath))
            {
                File.WriteAllLines(databasePath, initialData);
            }
            _tools = File.ReadAllLines(databasePath).ToList<string>();
        }
        public List<string> Tools()
        {
            return _tools;
        }
        public bool AddTool(string tool)
        {
            _tools.Add(tool);
            Save();
            return true;
        }
        public bool RemoveTool(string tool)
        {
            if (!_tools.Contains(tool))
                return false;

            _tools.Remove(tool);
            Save();
            return true;
        }
        public bool ReplaceTool(string existingTool, string newTool)
        {
            if (!_tools.Contains(existingTool))
                return false;
            _tools.Remove(existingTool);
            _tools.Add(newTool);
            
            Save();
            return true;
        }
        private void Save()
        {
            File.WriteAllLines(databasePath, _tools);
        }

        List<ToolModel> IDatabase.Get()
        {
            throw new NotImplementedException();
        }

        bool IDatabase.Remove()
        {
            throw new NotImplementedException();
        }

        bool IDatabase.Replace(string existingTool, string newTool)
        {
            throw new NotImplementedException();
        }

        bool IDatabase.Delete(string existingTool)
        {
            throw new NotImplementedException();
        }

        bool IDatabase.Update(string existingTool, string newTool)
        {
            throw new NotImplementedException();
        }

        ToolModel IDatabase.GetEntity(string tool)
        {
            throw new NotImplementedException();
        }

        ToolModel IDatabase.GetEntity(int id)
        {
            throw new NotImplementedException();
        }
    }
}
