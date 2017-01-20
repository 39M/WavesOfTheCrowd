using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Position", 21)]
    public class PositionTween : BaseTween
    {
        [Header("Local Position")]
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;

        public override void Play()
        {
            tweener = transform.DOLocalMove(to, _duration);
            base.Play();
        }

        public override void ResetTween()
        {
            base.ResetTween();
            transform.localPosition = from;
        }
    }
}