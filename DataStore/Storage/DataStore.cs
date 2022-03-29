using CRUD_Api.Interfaces;

namespace CRUD_Api.Storage
{
    public class DataStore
    {
        public DataStore(IDatabase datastoreImplementation)
        {
            DataBase = datastoreImplementation;
        }
        public IDatabase DataBase { get; private set; }
    }
}

