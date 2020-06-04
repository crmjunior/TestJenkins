using System;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Entities
{
    public class Entity : IEquatable<Entity>
    {
        public int Id { get; private set; }

        public DateTime DataCriacao { get; set; }

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }
    }
}