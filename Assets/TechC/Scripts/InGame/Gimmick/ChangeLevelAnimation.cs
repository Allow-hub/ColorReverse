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

        public void StartChangeLevelAnim(Level level) => StartCoroutine(ChangeLevelAnim(level));

        private IEnumerator ChangeLevelAnim(Level level)
        {

            switch (level)
            {
                case Level.Level_1:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;

                case Level.Level_2:
                    Debug.Log("Playing Level 2 animation.");
                    // 他のアニメーション処理を追加可能
                    break;

                case Level.Level_3:
                    Debug.Log("Playing Level 3 animation.");
                    break;

                case Level.Level_4:
                    Debug.Log("Playing Level 4 animation.");
                    break;

                case Level.Level_5:
                    Debug.Log("Playing Level 5 animation.");
                    break;
            }

            GameManager.I.ChangePlayModeState();
        }

        /// <summary>
        /// アニメーションを行う汎用メソッド
        /// </summary>
        private IEnumerator Animate(RectTransform target, Vector2 startPosition, Vector2 endPosition, float duration, AnimationCurve curve)
        {
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
        }
    }
}
