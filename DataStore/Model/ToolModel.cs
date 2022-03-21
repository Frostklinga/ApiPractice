namespace CRUD_Api.Model
{
    public class ToolModel
    {
        public int Id { get; private set; }
        public string Tool { get; private set; }
        public DateTime TimeOfPurchase { get; private set; }
        public string Owner { get; private set; }
    }
}
