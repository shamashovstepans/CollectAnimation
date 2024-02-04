using System.Collections.Generic;
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
        private List<EcsConfig> _configs;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EcsRunner>().AsSingle();

            foreach (var config in _configs)
            {
                Container.Bind(config.GetType()).FromInstance(config).AsSingle();
            }

            Container.Bind<IEcsSystem>().To<PlayerInitializationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerInputSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<SyncTransformSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<UpdateRigidbodySystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<PlayerMovementSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EnemySpawnerSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<EnemyFollowTargetSelectionSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<FollowTargetSystem>().AsSingle();
        }
    }
}