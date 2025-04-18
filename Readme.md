# MongoSharper

**MongoSharper** is a lightweight C# library designed to simplify standard CRUD operations and integrate seamlessly with MongoDB, including support for advanced features like Atlas Search.

---

## 🚀 Features

- Easy-to-use, strongly typed repository pattern  
- Standard CRUD operations  
- Support for MongoDB Atlas Search  
- Flexible filtering and projection  
- Index creation utility  

---

## 📦 Installation

You can install the MongoSharper package via NuGet:

```bash
Install-Package MongoSharper
```

---

## 🧑‍💻 Usage

### 1. Define Your Model

Your model should inherit from `DocumentBase`:

```csharp
public class MyClass : DocumentBase
{
    public string MyProperty1 { get; set; }
}
```

### 2. Instantiate the Repository

Replace `TEntity` with your model class when using the generic repository:

```csharp
var mongoRepo = new MongoNetRepo<MyClass>("your_connection_string", "your_database_name");
```

### 3. Use the Available Methods

You can now call any of the supported methods on the `mongoRepo` instance.

---

## 🛠️ Supported Methods

### CRUD Operations

- `void Create(TEntity document)`  
  Inserts a new document.

- `Task<TEntity> Replace(string id, TEntity newDocument)`  
  Replaces an existing document.

- `Task<bool> Delete(string id)`  
  Deletes a document by ID.

- `Task<TEntity> Get(Expression<Func<TEntity, bool>> filterExpression)`  
  Retrieves a document by filter.

- `Task<TEntity> GetById(string id)`  
  Retrieves a document by ID.

- `Task<IEnumerable<TEntity>> Get()`  
  Retrieves all documents.

- `Task<IEnumerable<TEntity>> GetTop(int limit)`  
  Retrieves the top N documents.

### Filtering & Projection

- `IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)`  
  Filters and projects data.

- `Task<IEnumerable<TEntity>> FilterBy(Expression<Func<TEntity, bool>> filterExpression)`  
  Filters data by criteria.

- `IQueryable<TEntity> GetAsQueryable()`  
  Gets a queryable collection.

### Search Operations

- `IEnumerable<TEntity> GetTextSearchResults(string index, string searchField, string searchText)`  
  Performs full-text search.

- `IEnumerable<TEntity> GetRegexSearchResults(string searchField, string regex, string index)`  
  Performs regex-based search.

### Utility

- `void CreateIndex(TEntity entity, string fieldName, string indexName, bool unique = false)`  
  Creates an index on a field.

- `Task<bool> DeleteAllDocuments()`  
  Deletes all documents from the collection.

---
