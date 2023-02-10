using Catalog.API.Extensions;
using Catalog.Data.Dtos;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly ItemsRepository itemsRepository = new();

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> Get()
    {
        var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
        return items;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        return items.Where(item => item.Id == id).SingleOrDefault() is ItemDto item ? Ok(item) : NotFound();
    }

    // POST /items
    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
        var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
        items.Add(item);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

        if (existingItem is null)
        {
            return NotFound();
        }

        var updatedItem = existingItem with
        {
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price
        };

        var index = items.FindIndex(existingItem => existingItem.Id == id);
        items[index] = updatedItem;

        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = items.FindIndex(existingItem => existingItem.Id == id);

        if (index < 0)
        {
            return NotFound();
        }

        items.RemoveAt(index);

        return NoContent();
    }
}