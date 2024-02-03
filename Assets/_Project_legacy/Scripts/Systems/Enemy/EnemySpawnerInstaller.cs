using UnityEngine;
using Zenject;

namespace Game.Systems.Enemy
{
    [CreateAssetMenu]
    internal class EnemySpawnerInstaller : ScriptableObjectInstaller<EnemySpawnerInstaller>
    {
        [SerializeField] private EnemySpawnerSettings _settings = default;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EnemySpawner>().AsSingle().WithArguments(_settings);
        }
    }
}