using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Color", 12)]
    public class ColorTween : BaseTween
    {
        [Header("Color")]
        public Color from = Color.white - Color.black;
        public Color to = Color.white;

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
            tweener = target.DOColor(to, _duration);
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
            target.color = from;
        }
    }
}