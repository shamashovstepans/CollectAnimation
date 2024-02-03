using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    internal class Bouncer : MonoBehaviour
    {
        [SerializeField] private BounceSettings _bounceSettings = default;

        private Tween _punchTween;
        
        [Button()]
        public void Bounce()
        {
            _punchTween?.Kill();
            transform.localScale = Vector3.one;
            _punchTween = transform.DOPunchScale(Vector3.one * _bounceSettings.Magnitude, _bounceSettings.TimeToBounce,
                0, 0).SetEase(_bounceSettings.BounceEase);
        }
    }
}