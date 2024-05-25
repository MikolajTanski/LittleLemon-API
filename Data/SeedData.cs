using LittleLemon_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LittleLemon_API.Data;

public static class SeedData
{
  public static void Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
      {
          using (var context = new ApplicationDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>(), configuration))
          {
              context.Database.EnsureCreated();
  
              if (!context.Meals.Any() && !context.Orders.Any())
              {
                  var order1 = new Order { Total = 0 };
                  var order2 = new Order { Total = 0 };
  
                  var meals = new List<Meal>
                  {
                      new Meal { Name = "Margherita Pizza", Type = "Pizza", Price = 5.99m },
                      new Meal { Name = "Pepperoni Pizza", Type = "Pizza", Price = 6.99m },
                      new Meal { Name = "Caesar Salad", Type = "Salad", Price = 4.99m },
                      new Meal { Name = "Greek Salad", Type = "Salad", Price = 4.99m },
                      new Meal { Name = "Spaghetti Carbonara", Type = "Pasta", Price = 7.99m },
                      new Meal { Name = "Lasagna", Type = "Pasta", Price = 8.99m },
                  };


                order1.Meals = new List<Meal> { meals[0], meals[1], meals[5] };
                order2.Meals = new List<Meal> { meals[2], meals[3], meals[4] };

                order1.Total = order1.Meals.Sum(m => m.Price);
                order2.Total = order2.Meals.Sum(m => m.Price);

                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Meals.AddRange(meals);
                context.SaveChanges();
            }
        }
    }
}
