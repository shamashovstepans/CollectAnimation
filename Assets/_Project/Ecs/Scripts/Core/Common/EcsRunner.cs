using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using Zenject;

namespace _Project.Scripts.Ecs.Core.Common
{
    internal class EcsRunner : IInitializable, ITickable, IDisposable, IFixedTickable
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _systems;
        private readonly List<IEcsSystem> _systemsToRegister;

        private readonly List<IEcsInitSystem> _initSystems = new();
        private readonly List<IEcsCoreRunSystem> _coreRunSystems = new();
        private readonly List<IEcsPhysicsRunSystem> _physicsRunSystems = new();
        private readonly List<IEcsDestroySystem> _destroySystems = new();

        public EcsRunner(EcsWorld world, List<IEcsSystem> systemsToRegister)
        {
            _world = world;
            _systems = new EcsSystems(_world);
            _systemsToRegister = systemsToRegister;
        }

        public void Initialize()
        {
            foreach (var ecsSystem in _systemsToRegister)
            {
                if (ecsSystem is IEcsInitSystem initSystem)
                {
                    _initSystems.Add(initSystem);
                }
                if (ecsSystem is IEcsCoreRunSystem coreRunSystem)
                {
                    _coreRunSystems.Add(coreRunSystem);
                }
                if (ecsSystem is IEcsPhysicsRunSystem physicsRunSystem)
                {
                    _physicsRunSystems.Add(physicsRunSystem);
                }
                if (ecsSystem is IEcsDestroySystem destroySystem)
                {
                    _destroySystems.Add(destroySystem);
                }

                _systems.Add(ecsSystem);
            }

            _systemsToRegister.Clear();

            foreach (var system in _initSystems)
            {
                system.Init(_systems);
            }
        }

        public void Tick()
        {
            foreach (var system in _coreRunSystems)
            {
                system.Run(_systems);
            }
        }

        public void FixedTick()
        {
            foreach (var system in _physicsRunSystems)
            {
                system.Run(_systems);
            }
        }

        public void Dispose()
        {
            foreach (var system in _destroySystems)
            {
                system.Destroy(_systems);
            }

            _world.Destroy();
        }
    }
}