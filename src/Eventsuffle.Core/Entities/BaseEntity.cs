using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Core.Entities
{
    /// <summary>
    /// Common base entity type for all entities.
    /// </summary>
    /// <typeparam name="T">Type of the entity's primary key.</typeparam>
    public abstract class BaseEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
