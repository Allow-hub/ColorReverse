using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TechC.LevelManager;

namespace TechC
{
    /// <summary>
    /// レベル変更時の画面のアニメーション
    /// </summary>
    public class ChangeLevelAnimation : MonoBehaviour
    {
        [SerializeField] private float[] animDuration; // 各レベルのアニメーション時間
        [SerializeField] private AnimationCurve[] animCurves; // アニメーション曲線（インスペクターで設定）

        [SerializeField] private RectTransform minWidth, minHeight;
        [SerializeField] private RectTransform maxWidth, maxHeight;
        [Header("AnimationObj")]
        [SerializeField] private RectTransform stage_1Rect;
        [SerializeField] private RectTransform stage_2Rect;
        [SerializeField] private RectTransform stage_3Rect;
        [SerializeField] private RectTransform stage_4Rect;
        [SerializeField] private RectTransform stage_5Rect;
        [SerializeField] private GameObject levelText;

        public void StartChangeLevelAnim(Level level) => StartCoroutine(ChangeLevelAnim(level));

        private IEnumerator ChangeLevelAnim(Level level)
        {

            switch (level)
            {
                case Level.Level_1:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;

                case Level.Level_2:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);

                    break;

                case Level.Level_3:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);

                    break;

                case Level.Level_4:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;

                case Level.Level_5:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;
            }

            GameManager.I.ChangePlayModeState();
        }

        /// <summary>
        /// アニメーションを行う汎用メソッド
        /// </summary>
        private IEnumerator Animate(RectTransform target, Vector2 startPosition, Vector2 endPosition, float duration, AnimationCurve curve)
        {
            levelText.SetActive(true);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration; // 進行割合（0～1）
                float curveValue = curve.Evaluate(t); // AnimationCurve に基づいた割合

                // Lerp で位置を補間
                target.anchoredPosition = Vector2.Lerp(startPosition, endPosition, curveValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終位置を明示的に設定
            target.anchoredPosition = endPosition;
            levelText.SetActive(false);

        }

        public float GetAnimationDuration(int level)
        {
            if (level >= 0 && level < animDuration.Length)
            {
                return animDuration[level];
            }
            else
            {
                return 4f;  // デフォルトのアニメーション時間
            }
        }
    }
}
