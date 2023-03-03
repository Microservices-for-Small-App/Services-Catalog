using Catalog.Contracts;
using Catalog.Data.Dtos;
using Catalog.Data.Entities;
using Catalog.Data.Extensions;
using CommonLibrary.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/items")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IRepository<CatalogItem> _catalogitemsRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public ItemsController(IRepository<CatalogItem> catalogitemsRepository, IPublishEndpoint publishEndpoint)
    {
        _catalogitemsRepository = catalogitemsRepository ?? throw new ArgumentNullException(nameof(catalogitemsRepository));

        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CatalogItemDto>>> GetAsync()
    {
        var items = (await _catalogitemsRepository.GetAllAsync()).Select(item => item.AsDto());

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CatalogItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _catalogitemsRepository.GetAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item.AsDto());
    }

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
        await _catalogitemsRepository.CreateAsync(item);

        await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateCatalogItemDto updateItemDto)
    {
        var existingItem = await _catalogitemsRepository.GetAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await _catalogitemsRepository.UpdateAsync(existingItem);

        await _publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await _catalogitemsRepository.GetAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        await _catalogitemsRepository.RemoveAsync(item.Id);

        await _publishEndpoint.Publish(new CatalogItemDeleted(id));

        return NoContent();
    }

}

// TODO: Remove this code. This is used only to simulate issue
// private static int requestCounter = 0;

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

// Console.WriteLine($"Request {requestCounter}: 200 (OK).");
// TODO: Remove this code. This is used only to simulate issue

