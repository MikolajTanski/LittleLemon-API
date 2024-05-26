using LittleLemon_API.Models;

namespace LittleLemon_API.Extensions;

public static class EnumerableExtension
{

        // Metoda rozszerzająca MostPopularMeal
        public static (string Name, int Count) MostPopularMeal(this IEnumerable<Meal> meals)
        {
            if (meals == null || !meals.Any())
            {
                throw new ArgumentException("Collection of meals is empty or null");
            }

            // Grupowanie posiłków po nazwie i zliczanie wystąpień każdej nazwy
            var mostPopular = meals
                .GroupBy(meal => meal.Name)
                .Select(group => new { Name = group.Key, Count = group.Count() })
                .OrderByDescending(result => result.Count)
                .First();
            
            return (mostPopular.Name, mostPopular.Count);
        }
        
    }
