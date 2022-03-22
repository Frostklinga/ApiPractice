using CRUD_Api.Model;
using Bogus;
using System.Diagnostics;
namespace CRUD_Api.Helpers
{
    public class PopulateDataStoreWithBogusData
    {
        public PopulateDataStoreWithBogusData(ref List<ToolModel> listToPopulate, int numberOfEntities)
        {
            var startTime = DateTime.Now;
            Debug.WriteLine("Began creating objects at: " + DateTime.Now.ToString());
            var BogusGenerator = new Faker<ToolModel>()
                .RuleFor(x => x.Id, z => Faker.GlobalUniqueIndex)
                .RuleFor(x => x.Owner, z => z.Name.FirstName())
                .RuleFor(x => x.TimeOfPurchase, z => z.Date.Recent(30))
                .RuleFor(x => x.Tool, z => z.Commerce.Product());
            var differance = DateTime.Now - startTime;
            Debug.WriteLine("Generation took: " + differance.ToString());
            listToPopulate.AddRange(BogusGenerator.GenerateLazy(numberOfEntities));
        }
    }
}
