

using System;
using System.Collections.Generic;

namespace MyCrud.Domain.Entities
{
    public abstract class BaseDomainEvent
    {
        public Guid Id { get; }
    }

    public abstract class BaseEntity<Key>
    {
        public Key Id { get; }
        private List<BaseDomainEvent> Events { get; set; }

        public void AddEvent(BaseDomainEvent @event)
        {
            this.Events.Add(@event);
        }
    }

    public class CreatedProductEvent : BaseDomainEvent { }

    public class Product : BaseEntity<Guid>
    {
        public string Name { get; }
        public Money Price { get; }
        public ProductCategory Category { get; }
        public DateTime ExpireDate { get; }
        public DateTime RegisterDate { get; }

        public void Create(CreateProductCommand command)
        {
            this.Events.Add(new CreatedProductEvent());
        }
    }

    public interface IDatetimeProvider
    {
    }
}
