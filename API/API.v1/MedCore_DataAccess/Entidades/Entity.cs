using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Repository.MongoRepository;

namespace MedCore_DataAccess.Entidades
{
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements]
    public abstract class Entity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}