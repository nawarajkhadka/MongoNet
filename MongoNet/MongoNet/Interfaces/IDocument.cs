using MongoDB.Bson;

namespace MongoNet.Interfaces
{
    public interface IDocument
    {
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }
}

﻿
