using Catalog.ApplicationCore.Interfaces;
using Catalog.ApplicationCore.Settings;
using Catalog.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Repositories;

public class ItemsRepository : IItemsRepository
{
    private readonly IMongoCollection<Item> _mongoDbCollection;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public ItemsRepository(IMongoDatabase database, IOptions<MongoDbCollectionSettings> mongoDbCollectionSettings)
    {
        _mongoDbCollection = database.GetCollection<Item>(mongoDbCollectionSettings.Value.Name);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await _mongoDbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return await _mongoDbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _mongoDbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<Item> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await _mongoDbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq(entity => entity.Id, id);
        await _mongoDbCollection.DeleteOneAsync(filter);
    }
}