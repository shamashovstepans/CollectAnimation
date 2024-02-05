using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    internal class ResourceCollectionSettings : ScriptableObject
    {
        public event Action Updated;

        [SerializeField] private float _flySpeed = 5f;

        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _spawnInterval;

        [SerializeField] private Resource _resourcePrefab;

        [SerializeField] private int _maxResources = 100;
        [SerializeField] private Ease _ease = Ease.InCubic;
        [SerializeField] private float _controlPointFollowSpeed;

        [OnValueChanged("OnValidate")]
        [SerializeField] private int _controlPointsCount = 3;
        [OnValueChanged("OnValidate")]
        [SerializeField] private float _maxAngle = 45f;
        [OnValueChanged("OnValidate")]
        [SerializeField] private float _radius = 10f;
        [OnValueChanged("OnValidate")]
        [SerializeField] private float _height = 1f;

        public float FlySpeed => _flySpeed;

        public float SpawnRadius => _spawnRadius;
        public float SpawnInterval => _spawnInterval;

        public Resource ResourcePrefab => _resourcePrefab;

        public int MaxResources => _maxResources;

        public Ease Ease => _ease;

        public int ControlPointsCount => _controlPointsCount;
        public float MaxAngle => _maxAngle;
        public float Radius => _radius;
        public float Height => _height;
        public float ControlPointFollowSpeed => _controlPointFollowSpeed;

        private void OnValidate()
        {
            Updated?.Invoke();
        }
    }
}