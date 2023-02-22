using Catalog.Data.Dtos;
using Catalog.Data.Entities;
using Catalog.Data.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<CatalogItem> _itemsRepository;
    private static int requestCounter = 0;

    public ItemsController(IRepository<CatalogItem> itemsRepository)
    {
        _itemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CatalogItemDto>>> GetAsync()
    {
        var items = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDto());

        return Ok(items);
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CatalogItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item.AsDto());
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<CatalogItemDto>> PostAsync(CreateCatalogItemDto createItemDto)
    {
        var item = new CatalogItem
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _itemsRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateCatalogItemDto updateItemDto)
    {
        var existingItem = await _itemsRepository.GetAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await _itemsRepository.UpdateAsync(existingItem);

        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        await _itemsRepository.RemoveAsync(item.Id);

        return NoContent();
    }

}

// TODO: Remove this code. This is used only to simulate issue
//requestCounter++;
//        Console.WriteLine($"Request {requestCounter}: Starting...");

//        if (requestCounter <= 2)
//        {
//            Console.WriteLine($"Request {requestCounter}: Delaying...");
//            await Task.Delay(TimeSpan.FromSeconds(10));
//        }

//        if (requestCounter <= 4)
//{
//    Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error).");
//    return StatusCode(500);
//}
// TODO: Remove this code. This is used only to simulate issue

// Console.WriteLine($"Request {requestCounter}: 200 (OK).");