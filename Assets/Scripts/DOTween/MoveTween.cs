using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Move", 22)]
    public class MoveTween : BaseTween
    {
        [Header("Move Addition")]
        public Vector3 _add = Vector3.zero;

        Vector3 origin;
        bool moved = false;

        public override void Play()
        {
            if (!moved)
            {
                origin = transform.localPosition;
                moved = true;
            }
            tweener = transform.DOLocalMove(origin + _add, _duration);

            base.Play();
        }

        public override void ResetTween()
        {
            base.ResetTween();

            if (!moved)
            {
                origin = transform.localPosition;
                moved = true;
            }
            transform.localPosition = origin;
        }
    }
}