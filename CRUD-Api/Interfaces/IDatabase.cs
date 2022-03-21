using CRUD_Api.Model;
using System.Collections.Generic;

namespace CRUD_Api.Interfaces
{
    public interface IDatabase
    {
        List<ToolModel> Get();
        bool Remove();
        bool Replace(string existingTool, string newTool);
        bool Delete(string existingTool);
        bool Update(string existingTool, string newTool);
        ToolModel GetEntity(string tool);
        ToolModel GetEntity(int id)
    }
}
