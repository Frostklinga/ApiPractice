using CRUD_Api.Model;
using System.Collections.Generic;

namespace CRUD_Api.Interfaces
{
    public interface IDatabase
    {
        public List<ToolModel> Get();
        public bool Delete(string tool);
        public bool Delete(int id);
        public bool Update(string existingTool, string newTool);
        public void Add(ToolModel purchaseInformation);
        public ToolModel GetEntity(string tool);
        public ToolModel GetEntity(int id);
    }
}
