using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Alpha", 11)]
    public class AlphaTween : BaseTween
    {
        [Header("Alpha")]
        public float from = 0;
        public float to = 1;

        Graphic target;

        public override void Play()
        {
            if (target == null)
            {
                target = GetComponent<Graphic>();
                if (target == null)
                {
                    return;
                }
            }
            tweener = target.DOFade(to, _duration);
            base.Play();
        }

        public override void ResetTween()
        {
            base.ResetTween();
            if (target == null)
            {
                target = GetComponent<Graphic>();
                if (target == null)
                {
                    return;
                }
            }
            target.color += new Color(0, 0, 0, 1) * (from - target.color.a);
        }
    }
}