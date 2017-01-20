
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace UnityEngine.UI
{
    public class BaseTween : MonoBehaviour
    {
        [Header("Tweener")]
        public Ease _curve = Ease.Unset;
        public bool _loop = false;
        public LoopType _style = LoopType.Restart;
        public float _duration = 0.02f;
        public float _delay = 0f;
        public bool _ignoreTimeScale;
        public UpdateType _updateType = UpdateType.Normal;

        protected Tween tweener;

        public TweenCallback onComplete;

        public virtual void ResetTween()
        {
        }

        public void PauseTween()
        {
            if (onComplete == null)
            {
                tweener.Pause();
            }
            if (needReset())
            {
                ResetTween();
            }
        }

        public virtual void Play()
        {
            PauseTween();

            tweener.OnComplete<Tween>(onComplete);

            tweener.SetUpdate(_updateType);
            tweener.SetEase(_curve);
            tweener.SetDelay(_delay);
            tweener.timeScale = _ignoreTimeScale ? 1 : Time.timeScale;
            if (_loop)
            {
                tweener.SetLoops(-1, _style);
            }
            else
            {
                if(_style == LoopType.Yoyo)
                {
                    tweener.SetLoops(2, _style);
                }
                else if(_style == LoopType.Restart)
                {
                    if (onComplete == null)
                    {
                        tweener.OnComplete(delegate { PauseTween(); });
                    }
                    tweener.SetLoops(1, _style);
                }
                else
                {
                    tweener.SetLoops(1, _style);
                }
            }
            tweener.Play();
        }

        protected virtual bool needReset()
        {
            return _style != LoopType.Incremental;
        }
    }
}
