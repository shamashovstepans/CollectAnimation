using System;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IView
    {
        Guid Id { get; }
        int EntityId { get; set; }
    }
}