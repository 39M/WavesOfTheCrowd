using DG.Tweening;

namespace UnityEngine.UI
{
    [AddComponentMenu("NewUI/Tween/Rotation", 31)]
    public class RotationTween : BaseTween
    {
        [Header("Local Rotation")]
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;
        public RotateMode rotateMode = RotateMode.Fast;

        public override void Play()
        {
            tweener = transform.DOLocalRotate(to, _duration, rotateMode);
            base.Play();
        }

        public override void ResetTween()
        {
            base.ResetTween();
            transform.localRotation = Quaternion.Euler(from);
        }
    }
}