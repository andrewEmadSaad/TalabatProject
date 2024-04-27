using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    //D:\Assignments\Projects\TalabatProject\Talabat.Repository\Data\DataSeed\brands.json
                    var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                    foreach (var brand in Brands)
                        context.Set<ProductBrand>().Add(brand);
                }
                if (!context.ProductTypes.Any())
                {
                    var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    foreach (var Type in Types)
                        context.Set<ProductType>().Add(Type);
                }
                if (!context.Products.Any())
                {
                    var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                    foreach (var Product in Products)
                        context.Set<Product>().Add(Product);
                }

                if (!context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                    foreach (var DeliveryMethod in DeliveryMethods)
                        context.Set<DeliveryMethod>().Add(DeliveryMethod);
                }
                //delivery.json
                await context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, ex.Message);
            }
            //D:\Assignments\Projects\TalabatProject\Talabat.Repository\Data\DataSeed\brands.json
            

        }
    }
}
