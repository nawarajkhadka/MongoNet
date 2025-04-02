# Introduction 
TODO: MongoNet is a library project to facilitate normal crud operation with c# and mongodb

# Features
1. Normal CRUD Operation
2. Atlas search Operation


# Usage

Install package.
`Install-Package MongoSharp -version [version number]`

The usage is pretty simple, Just create an object of mongonetrepo and get going with all supported methods.
1.Create a model inheriting the DocumentBase Class
`public class MyClass:DocumentBase `
`{`
`public string Myproperty1 {get;set;}`
`}`

2.Replace TEntity with your model

3.Create a object of mongonetrepo
`var mongonetRepo= new MongoNetRepo(myconnectionstring, mydatabasename)`

4.Call the methods as required passing your model
`mongonetRepo.Method(parameter)`

Supported Methods:

1. **Create(TEntity document)**  
   Creates a new document in the database.

2. **Task<TEntity> Replace(string id, TEntity newDocument)**  
   Replaces an existing document with a new document.

3. **Task<bool> Delete(string id)**  
   Deletes a document from the database.

4. **Task<TEntity> Get(Expression<Func<TEntity, bool>> filterExpression)**  
   Retrieves a document that matches the specified filter.

5. **Task<TEntity> GetById(string id)**  
   Retrieves a document by its identifier.

6. **Task<IEnumerable<TEntity>> Get()**  
   Retrieves all documents from the database.

7. **Task<IEnumerable<TEntity>> GetTop(int limit)**  
   Retrieves the top 'limit' documents from the database.

8. **IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)**  
   Filters documents based on the specified criteria and projects them into a different type.

9. **Task<bool> DeleteAllDocuments()**  
   Deletes all documents from the database.

10. **Task<IEnumerable<TEntity>> FilterBy(Expression<Func<TEntity, bool>> filterExpression)**  
    Filters documents based on the specified criteria.

11. **IQueryable<TEntity> GetAsQuerable()**  
    Retrieves documents as a queryable collection.

12. **IEnumerable<TEntity> GetTextSearchResults(string index, string searchField, string searchText)**  
    Performs a text search on the specified field.

13. **IEnumerable<TEntity> GetRegexSearchResults(string searchField, string regex, string index)**  
    Performs a regex search on the specified field.

14. **void CreateIndex(TEntity entity, string fieldName, string indexName, bool unique = false)**  
    Creates an index on a specified field of the entity.

# Enjoy
