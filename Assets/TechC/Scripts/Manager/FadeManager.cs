using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class FadeManager : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;

        private void Start()
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        public void StartFade(float duration)
        {
            StartCoroutine(FadeIn(duration / 2));
        }
        public IEnumerator FadeIn(float duration)
        {
            Color canvasColor = fadeImage.color;

            float startAlpha = 0f;
            float endAlpha = 1f; // アルファ値は0.0f〜1.0fの範囲
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                canvasColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                fadeImage.color = canvasColor; // 色を更新
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // 最終的なアルファ値を設定
            canvasColor.a = endAlpha;
            fadeImage.color = canvasColor;

            yield return new WaitForSeconds(duration);
            StartCoroutine(FadeOut(duration));
        }
        private IEnumerator FadeOut(float duration)
        {
            Color canvasColor = fadeImage.color;

            float startAlpha = 1f;
            float endAlpha = 0f; // アルファ値は0.0f〜1.0fの範囲
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                canvasColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                fadeImage.color = canvasColor; // 色を更新
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終的なアルファ値を設定
            canvasColor.a = endAlpha;
            fadeImage.color = canvasColor;

        }
    }

}