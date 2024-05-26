using LittleLemon_API.Extensions;
using LittleLemon_API.Models;
using LittleLemon_API.Repository.MealRepository;

namespace LittleLemon_API.Services.MealService;

public class MealService : IMealService
{
    private readonly IMealRepository _mealRepository;

    public MealService(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<IEnumerable<Meal>> GetAllMealsAsync()
    {
        return await _mealRepository.GetAllMealsAsync();
    }

    public async Task<Meal> GetMealByIdAsync(int id)
    {
        return await _mealRepository.GetMealAsync(id);
    }

    public async Task CreateMealAsync(Meal meal)
    {
        await _mealRepository.AddMealAsync(meal);
    }

    public async Task UpdateMealAsync(Meal meal)
    {
        await _mealRepository.UpdateMealAsync(meal);
    }

    public async Task DeleteMealAsync(int id)
    {
        var mealExists = await _mealRepository.GetMealAsync(id);
        if (mealExists == null)
        {
            throw new KeyNotFoundException($"Meal with id {id} not found.");
        }

        await _mealRepository.DeleteMealAsync(id);
    }
    
    public async Task<(string MostPopularName, int Count)> GetMostPopularMealAsync()
    {
        var meals = await _mealRepository.GetAllMealsAsync();
        if (meals == null || !meals.Any())
        {
            throw new InvalidOperationException("No meals available.");
        }

        // Używamy metody rozszerzającej, aby znaleźć najpopularniejszy posiłek
        var mostPopular = meals.MostPopularMeal();
        
        return mostPopular;
    }
}