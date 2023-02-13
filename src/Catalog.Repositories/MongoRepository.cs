using Catalog.ApplicationCore.Interfaces;
using Catalog.ApplicationCore.Settings;
using Catalog.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> _mongoDbCollection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, IOptions<MongoDbCollectionSettings> mongoDbCollectionSettings)
    {
        _mongoDbCollection = database.GetCollection<T>(mongoDbCollectionSettings.Value.Name);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _mongoDbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<T> GetAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return await _mongoDbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _mongoDbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<T> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await _mongoDbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        await _mongoDbCollection.DeleteOneAsync(filter);
    }

}