using System.Collections.Generic;
using _Project.Ecs.Scripts.Core.Common.View;
using _Project.Ecs.Scripts.Core.Systems.Core;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Ecs.Core.Installer
{
    [CreateAssetMenu]
    internal class EcsCoreInstaller : ScriptableObjectInstaller<EcsCoreInstaller>
    {
        [SerializeField]
        private List<EcsConfig> _configs = default;

        public override void InstallBindings()
        {
            Container.BindInstance(new EcsWorld()).AsSingle();
            Container.BindInterfacesTo<EcsRunner>().AsSingle();

            foreach (var config in _configs)
            {
                Container.Bind(config.GetType()).FromInstance(config).AsSingle();
            }

            Container.BindInterfacesTo<ViewFactory>().AsSingle();

            BindCoreSystems();
            BindPhysicsSystems();
        }

        private void BindCoreSystems()
        {
            Container.Bind<IEcsSystem>().To<PlayerInitializationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EnemySpawnerSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<PlayerInputSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerMovementSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerTargetRemovalSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerTargetSelectionSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<EnemyTargetSelectionSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<SyncTransformsSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MovableRotationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<LookAtTargetRotationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MovingStateSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<DeathSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<ViewCleanerSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EntityCleanerSystem>().AsSingle();
        }

        private void BindPhysicsSystems()
        {
            Container.Bind<IEcsSystem>().To<MoveToTargetSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<UpdateRigidbodySystem>().AsSingle();
        }
    }
}