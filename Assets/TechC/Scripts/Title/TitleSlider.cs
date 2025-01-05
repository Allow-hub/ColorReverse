using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class TitleSlider : MonoBehaviour
    {
        public void SetBGMVolume(float volume)
        {
            BgmManager.I.SetBGMVolume(volume);
        }
        public void SetSEVolume(float volume)
        {
            SeManager.I.SetSEVolume(volume);
        }
    }
}
