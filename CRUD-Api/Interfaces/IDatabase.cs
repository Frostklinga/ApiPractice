using System.Collections.Generic;

namespace CRUD_Api.Interfaces
{
    public interface IDatabase
    {
        List<string> Get();
        bool Remove();
        bool Replace(string existingTool, string newTool);
        bool Delete(string existingTool);
        bool Update(string existingTool, string newTool);
        string GetTool(string tool);
    }
}
