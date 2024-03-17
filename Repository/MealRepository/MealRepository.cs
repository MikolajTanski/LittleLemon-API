using LittleLemon_API.Data;
using LittleLemon_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LittleLemon_API.Repository.MealRepository;

public class MealRepository : IMealRepository
{
    private readonly ApplicationDbContext _context;

    public MealRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Meal> GetMealAsync(int id)
    {
        return await _context.Meals.FindAsync(id);
    }

    public async Task<IEnumerable<Meal>> GetAllMealsAsync()
    {
        return await _context.Meals.ToListAsync();
    }

    public async Task<Meal> AddMealAsync(Meal meal)
    {
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();
        return meal;
    }

    public async Task<Meal> UpdateMealAsync(Meal meal)
    {
        _context.Entry(meal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return meal;
    }

    public async Task DeleteMealAsync(int id)
    {
        var meal = await _context.Meals.FindAsync(id);
        if (meal != null)
        {
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
        }
    }
}