using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL
{
    public class CategorySeed
    {
        public static async Task SeedAsync(CatalogDbContext context)
        {
            try
            {
                if(context.Categories != null && !context.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("../BLL/SeedData/Categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                    if(categories is not null)
                    {
                        foreach (var category in categories)
                            await context.Categories.AddAsync(category);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
