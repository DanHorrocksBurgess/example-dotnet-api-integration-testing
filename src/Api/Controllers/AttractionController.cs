using Api.Data;
using Api.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api;

[ApiController]
[Route("attraction")]
public class AttractionController(ApiContext apiContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Attraction>))]
    public async Task<IActionResult> ListAllAttractions()
    {
        var attractions = await apiContext.Attractions.ToListAsync();
        return Ok(attractions);
    }

    [HttpGet("{attractionId}", Name = "Attraction_GetById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Attraction))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttractionById([FromRoute] int attractionId)
    {
        var attraction = await apiContext.Attractions.SingleOrDefaultAsync(a => a.AttractionId == attractionId);

        if (attraction == null)
        {
            return NotFound();
        }

        return Ok(attraction);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Attraction))]
    public async Task<IActionResult> CreateAttraction([FromBody] Attraction attraction)
    {
        apiContext.Attractions.Add(attraction);
        await apiContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAttractionById), new { attractionId = attraction.AttractionId }, attraction);
    }
}