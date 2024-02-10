using System.Collections.Generic;
using _Project.Ecs.Scripts.Core.Common.View;
using _Project.Ecs.Scripts.Core.Systems.Core;
using _Project.Ecs.Scripts.Core.Systems.Physics;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Systems;
using Leopotam.EcsLite;
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
            Container.BindInterfacesTo<ViewFactory>().AsSingle();

            Container.BindInterfacesTo<EcsRunner>().AsSingle();

            foreach (var config in _configs)
            {
                Container.Bind(config.GetType()).FromInstance(config).AsSingle();
            }

            BindCoreSystems();
            BindPhysicsSystems();
        }

        private void BindCoreSystems()
        {
            Container.Bind<IEcsSystem>().To<PlayerInitializationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EnemySpawnerSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<PlayerInputSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerShootingSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<TargetValidatorSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<TargetSelectionSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EnemyTargetSelectionSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<ShootingSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MovingStateSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<ProjectileHitSystem>().AsSingle();

            Container.Bind<IEcsSystem>().To<CleanDetectionSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<ViewCleanerSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EntityCleanerSystem>().AsSingle();
        }

        private void BindPhysicsSystems()
        {
            Container.Bind<IEcsSystem>().To<LookAtTargetRotationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MoveToTargetSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MovableRotationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerMovementSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<CollisionProcessSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<LegacyProjectileMovementSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<BallisticProjectileMovementSystem>().AsSingle();
        }
    }
}