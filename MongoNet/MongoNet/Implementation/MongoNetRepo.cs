﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoNet.Interfaces;
using System.Linq.Expressions;

namespace MongoNet.Implementation
{
    public abstract class MongoNetRepo<TEntity> : IMongoNetRepo<TEntity> where TEntity : IDocument
    {
        protected IMongoCollection<TEntity>? DbCollection { get; set; }      
        protected IMongoDatabase? dataBase;
        /// <summary>
        /// 1. initialize with initial connection string and databasename
        /// </summary>
        /// <param name="databaseSettings"></param>
        public MongoNetRepo(string connectionString, string databaseName)
        {                        
            var mongoClient= new MongoClient(connectionString);
            dataBase = mongoClient.GetDatabase(databaseName);                                    
        }
        /// <summary>
        /// 2. set current collection name
        /// </summary>
        /// <param name="collectionName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetCollection(string collectionName)
        {
            if(dataBase == null)
            {
                throw new ArgumentNullException("Database is not initialized. Please initialize database first");
            }
            DbCollection = dataBase.GetCollection<TEntity>((collectionName));
        }
        /// <summary>
        /// insert record into the collection set in SetCollection()
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Create(TEntity document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + "object is null");
            }            
            await DbCollection.InsertOneAsync(document);
        }
        /// <summary>
        /// delete a record from collection passing id as string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(document => document.Id, objectId);
            var result = await DbCollection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }
        /// <summary>
        /// delete all records from collection
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAllDocuments()
        {
            var result = await DbCollection.DeleteManyAsync(Builders<TEntity>.Filter.Empty);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// get results by filtered expression
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await DbCollection.Find(filterExpression).SingleAsync();
        }

        /// <summary>
        /// get all records inside a collection
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Get()
        {
            var all = await DbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        //use get top x with limit
        public async Task<IEnumerable<TEntity>> GetTop(int limit)
        {
            FindOptions<TEntity> options = new FindOptions<TEntity> { Limit = limit };
            var top = await DbCollection.FindAsync(Builders<TEntity>.Filter.Empty, options);
            return await top.ToListAsync();
        }
        //use filterby with custom mongodb filterexpression
        public async Task<IEnumerable<TEntity>> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
        {
            var filteredList = await DbCollection.FindAsync(filterExpression);
            return await filteredList.ToListAsync();
        }

        //use filterby with custom mongodb filterexpression
        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return DbCollection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        //update entity
        public Task<TEntity> Replace(string id, TEntity newDocument)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            return DbCollection.FindOneAndReplaceAsync(filter, newDocument);
        }

        //get entity by id

        public async Task<TEntity> GetById(string id)
        {

            var objectId = ObjectId.Parse(id);
            var filter = Builders<TEntity>.Filter.Eq(document => document.Id, objectId);
            return await DbCollection.Find(filter).SingleAsync();
        }
        // get mongodb collection as querable
        public IQueryable<TEntity> GetAsQuerable()
        {
            return DbCollection.AsQueryable<TEntity>();
        }


        // get result after converting mongodb query into mongodb pipeline definition. useful for performing text search in mongodb
        public IEnumerable<TEntity> GetTextSearchResults(string index, string searchField, string searchText)
        {

            PipelineDefinition<TEntity, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$search",
                    new BsonDocument
                    {
                        { "index", index },
                        { "text",
                            new BsonDocument{
                                { "path", searchField },
                                { "query", searchText }
                            }
                        }
                    })
            };
            return GetResultsFromPipelineDefinition(pipeline);
        }

        // get result after regex search
        public IEnumerable<TEntity> GetRegexSearchResults(string searchField, string regex, string index)
        {
            PipelineDefinition<TEntity, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { searchField, new BsonDocument("$regex", regex) }
                })
            };
            return GetResultsFromPipelineDefinition(pipeline);
        }

        private List<TEntity> GetResultsFromPipelineDefinition(PipelineDefinition<TEntity, BsonDocument> pipelineDefinition)
        {
            var pipeLineSearchResults = DbCollection.Aggregate(pipelineDefinition).ToList();
            var mappedSearchResults = pipeLineSearchResults.Select(t => BsonSerializer.Deserialize<TEntity>(t)).ToList();
            return mappedSearchResults;
        }        

    }
}
