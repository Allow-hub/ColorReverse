using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [CreateAssetMenu(fileName = "NewAudioClipSet", menuName = "TechC/AudioClipSet")]
    public class AudioClipSet : ScriptableObject
    {
        public AudioClip[] audioClips;
    }
}
