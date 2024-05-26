using LittleLemon_API.Models;
using LittleLemon_API.Services.MealService;
using Microsoft.AspNetCore.Mvc;

namespace LittleLemon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;

    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meal>>> GetAllMeals()
    {
        var meals = await _mealService.GetAllMealsAsync();
        return Ok(meals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meal>> GetMeal(int id)
    {
        var meal = await _mealService.GetMealByIdAsync(id);
        if (meal == null)
        {
            return NotFound();
        }
        return meal;
    }

    [HttpPost]
    public async Task<ActionResult<Meal>> CreateMeal(Meal meal)
    {
        await _mealService.CreateMealAsync(meal);
        return CreatedAtAction(nameof(GetMeal), new { id = meal.Id }, meal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeal(int id, Meal meal)
    {
        if (id != meal.Id)
        {
            return BadRequest();
        }

        try
        {
            await _mealService.UpdateMealAsync(meal);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeal(int id)
    {
        try
        {
            await _mealService.DeleteMealAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpGet("most-popular")]
    public async Task<IActionResult> GetAllMealsWithMostPopular()
    {
        try
        {
            var (meals, mostPopular) = await _mealService.GetMostPopularMealAsync();
            return Ok(new { Meals = meals, Count = mostPopular });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}