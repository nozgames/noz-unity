using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.InputSystem;

using NoZ;

class AnimationFrameEvents : MonoBehaviour
{
    public void PlayAudioClip (AudioClip clip)
    {
        clip.Play();
    }
}
