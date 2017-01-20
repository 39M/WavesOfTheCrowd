using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Scale", 41)]
    public class ScaleTween : BaseTween
    {
        [Header("Scale")]
        public float from = 1;
        public float to = 1.05f;

        public override void Play()
        {
            tweener = transform.DOScale(to, _duration);
            base.Play();
        }

        public override void ResetTween()
        {
            base.ResetTween();
            transform.localScale = Vector3.one * from;
        }
    }
}