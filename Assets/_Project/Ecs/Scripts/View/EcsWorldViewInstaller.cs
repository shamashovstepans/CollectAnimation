using UnityEngine;
using Zenject;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsWorldViewInstaller : MonoInstaller<EcsWorldViewInstaller>
    {
        [SerializeField] private EcsWorldView _worldView = default;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EcsWorldView>().FromInstance(_worldView).AsSingle();
        }
    }
}