namespace CRUD_Api
{
    public class FileStore
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
    }
}
