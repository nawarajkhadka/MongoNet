using System.Linq.Expressions;


namespace MongoNet.Interfaces
{
    public interface IMongoNetRepo<TEntity>
    {
        //interfaces
        Task Create(TEntity document);
        Task<TEntity> Replace(string id, TEntity newDocument);
        Task<bool> Delete(string id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> filterExpression);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> GetTop(int limit);
        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression);
        Task<bool> DeleteAllDocuments();
        Task<IEnumerable<TEntity>> FilterBy(Expression<Func<TEntity, bool>> filterExpression);
        IQueryable<TEntity> GetAsQuerable();
        IEnumerable<TEntity> GetTextSearchResults(string index, string searchField, string searchText);
        IEnumerable<TEntity> GetRegexSearchResults(string searchField, string regex, string index);

    }
}

