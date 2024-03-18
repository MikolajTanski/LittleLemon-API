using LittleLemon_API.Models;

namespace LittleLemon_API.Services.MealService;

public interface IMealService
{
    Task<IEnumerable<Meal>> GetAllMealsAsync();
    Task<Meal> GetMealByIdAsync(int id);
    Task CreateMealAsync(Meal meal);
    Task UpdateMealAsync(Meal meal);
    Task DeleteMealAsync(int id);
}