using CRUD_Api.Model;
using Bogus;
namespace CRUD_Api.Helpers
{
    public class PopulateDataStoreWithBogusData
    {
        public PopulateDataStoreWithBogusData(ref List<ToolModel> listToPopulate, int numberOfEntities)
        {
            var BogusGenerator = new Faker<ToolModel>()
                .RuleFor(x => x.Id, z => Faker.GlobalUniqueIndex)
                .RuleFor(x => x.Owner, z => z.Name.FirstName())
                .RuleFor(x => x.TimeOfPurchase, z => z.Date.Recent(30))
                .RuleFor(x => x.Tool, z => z.Commerce.Product());
            listToPopulate.AddRange(BogusGenerator.GenerateLazy(numberOfEntities));
        }
    }
}
