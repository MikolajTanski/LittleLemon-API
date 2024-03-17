using LittleLemon_API.Models;

namespace LittleLemon_API.Repository.MealRepository;

public interface IMealRepository
{
    Task<Meal> GetMealAsync(int id);
    Task<IEnumerable<Meal>> GetAllMealsAsync();
    Task<Meal> AddMealAsync(Meal meal);
    Task<Meal> UpdateMealAsync(Meal meal);
    Task DeleteMealAsync(int id);
}