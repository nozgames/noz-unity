using System;
using UnityEngine;

namespace NoZ.Animations
{
    [CreateAssetMenu(menuName = "NoZ/Animation/Shader")]
    public class AnimationShader : ScriptableObject
    {
        [Serializable]
        internal struct EventFrame
        {
            [Tooltip("Normalized time to raise the event [0-1]")]
            public float time;

            [Tooltip("Event to raise")]
            public AnimationEvent raise;
        }

        [SerializeField] private AnimationClip _clip = null;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _blendTime = 0.1f;
        [SerializeField] private EventFrame[] _events = null;

        public AnimationClip clip => _clip;
        public float speed => _speed;
        public float blendTime => _blendTime;
        public bool isLooping => clip.isLooping;
        public float length => clip.length;

        internal EventFrame[] events => _events;
    }
}
