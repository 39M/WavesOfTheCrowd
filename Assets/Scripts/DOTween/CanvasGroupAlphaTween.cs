using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/CanvasGroupAlpha", 51)]
    public class CanvasGroupAlphaTween : BaseTween
    {
        [Header("Alpha")]
        public float from = 0;
        public float to = 1;

        CanvasGroup target;

        public override void Play()
        {
            if (target == null)
            {
                target = GetComponent<CanvasGroup>();
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
                target = GetComponent<CanvasGroup>();
                if (target == null)
                {
                    return;
                }
            }
            target.alpha = from;
        }
    }
}
