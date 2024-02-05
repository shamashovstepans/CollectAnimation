using System;
using System.Collections.Generic;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Dependencies;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Ecs.Scripts.Core.Common.View
{
    internal class ViewFactory : IViewFactory, IInitializable
    {
        private readonly EcsWorld _world;
        private readonly ViewFactoryConfig _config;
        private readonly Dictionary<Guid, GameObject> _views = new();
        
        private EcsPool<EcsGameObject> _ecsGameObjectPool;

        public ViewFactory(EcsWorld world, ViewFactoryConfig config)
        {
            _world = world;
            _config = config;
        }

        public void Initialize()
        {
            _ecsGameObjectPool = _world.GetPool<EcsGameObject>();
        }

        public IView Create(int entity, string name,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            var prefab = _config.GetPrefab(name);
            var gameObject = Object.Instantiate(prefab, position, rotation, parent);
            var view = gameObject.GetComponent<IView>();
            _views.Add(view.Id, gameObject);

            ref var ecsGameObject = ref _ecsGameObjectPool.Add(entity);
            ecsGameObject.View = view;

            return view;
        }

        public void Destroy(IView view)
        {
            if (_views.TryGetValue(view.Id, out var gameObject))
            {
                Object.Destroy(gameObject);
                _views.Remove(view.Id);
            }
        }
    }
}