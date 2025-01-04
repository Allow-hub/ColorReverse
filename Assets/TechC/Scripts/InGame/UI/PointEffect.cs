using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class PointEffect : MonoBehaviour
    {
        [SerializeField] private UIEffect uiEffect;
        [SerializeField] private float duration = 1f;
        [SerializeField] private Vector2 intervalRange = new Vector2(1, 3);

        private float currentInterval = 0;
        private float elapsedTime =0f;
        private void OnValidate()
        {
            uiEffect = GetComponent<UIEffect>();
        }

        private void Awake()
        {
            currentInterval = Random.Range(intervalRange.x, intervalRange.y);
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= currentInterval) 
            {
                StopAllCoroutines();  
                StartCoroutine(Shiny());
                currentInterval = Random.Range(intervalRange.x, intervalRange.y);
                elapsedTime = 0f;
            }
        }


        private IEnumerator Shiny()
        {
            uiEffect.transitionRate = 0;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                uiEffect.transitionRate = Mathf.Lerp(0f, 1f, elapsed / duration); // 0から1へ補間
                elapsed += Time.deltaTime; // 経過時間を更新
                yield return null;         // 次のフレームまで待機
            }
            uiEffect.transitionRate = 1;
        }
    }
}
