using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// 枠の点滅アニメーション
    /// </summary>
    public class FrameAnimation : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private string animTriggerName = "IsChanging";
        [SerializeField] private float interval = 0.7f;

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            anim.SetTrigger(animTriggerName);

        }





        public void StartFrameAnim()
        {
            anim.SetTrigger(animTriggerName);
        }
    }

}
