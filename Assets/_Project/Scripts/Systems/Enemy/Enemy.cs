using UnityEngine;

namespace Game.Systems.Enemy
{
    internal class Enemy : MonoBehaviour
    {
        private Transform _target;
        private EnemySpawnerSettings _settings;
        
        public void Init(Transform target, EnemySpawnerSettings settings)
        {
            _target = target;
            _settings = settings;
            UpdatePosition();
        }
        
        private void Update()
        {
            if (_target == null) return;

            UpdatePosition();
        }

        private void UpdatePosition()
        {

            transform.position = Vector3.MoveTowards(transform.position, _target.position, _settings.EnemySpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
        }
    }
}