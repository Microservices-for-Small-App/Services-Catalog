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

    public ItemsController(IRepository<CatalogItem> itemsRepository)
    {
        _itemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
    }

    [HttpGet]
    public async Task<IEnumerable<CatalogItemDto>> Get()
    {
        var items = (await _itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
        return items;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CatalogItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item.AsDto();
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

        if (existingItem == null)
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

        if (item == null)
        {
            return NotFound();
        }

        await _itemsRepository.RemoveAsync(item.Id);

        return NoContent();
    }

}