using DG.Tweening;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class BounceSettings : ScriptableObject
    {
        [SerializeField] private float _timeToBounce = default;
        [SerializeField] private float _magnitude = default;
        [SerializeField] private Ease _bounceEase;

        public float TimeToBounce => _timeToBounce;
        public float Magnitude => _magnitude;
        public Ease BounceEase => _bounceEase;
    }
}