using UnityEngine;

namespace NoZ.Animations
{
    [CreateAssetMenu(menuName = "NoZ/Animation/Event")]
    public class AnimationEvent : ScriptableObject
    {
        private int _hash = 0;

        public void OnEnable()
        {
            _hash = Animator.StringToHash(name);
        }
    }
}
