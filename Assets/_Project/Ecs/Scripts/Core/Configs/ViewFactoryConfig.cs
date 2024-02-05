using _Project.Scripts.Ecs.Core.Common;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    public class ViewFactoryConfig : EcsConfig
    {
        [SerializedDictionary("ID", "Person")]
        [SerializeField]
        private SerializedDictionary<string, GameObject> _prefabs = default;
        
        public GameObject GetPrefab(string prefabName)
        {
            return _prefabs[prefabName];
        }
    }
}