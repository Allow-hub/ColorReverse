using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TechC.LevelManager;
using static UnityEngine.Rendering.DebugUI;

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

        [SerializeField] private GameObject[] stage;
        [Header("AnimationObj")]
        [SerializeField] private RectTransform stage_1Rect;
        [SerializeField] private RectTransform stage_2Rect, stage_2Rect_2;
        [SerializeField] private RectTransform stage_3Rect;
        [SerializeField] private RectTransform stage_4Rect;
        [SerializeField] private RectTransform stage_5Rect;
        [SerializeField] private GameObject levelText;

        public void StartChangeLevelAnim(Level level) => StartCoroutine(ChangeLevelAnim(level));

        private IEnumerator ChangeLevelAnim(Level level)
        {

            ActiveStage(GameManager.I.GetCurrentLevel() - 1);
            switch (level)
            {
                case Level.Level_1:
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;

                case Level.Level_2:
                    StartCoroutine(Animate(stage_2Rect, stage_2Rect.anchoredPosition, new Vector2(stage_2Rect.anchoredPosition.x, minHeight.anchoredPosition.y), animDuration[0], animCurves[0]));
                    StartCoroutine(Animate(stage_2Rect_2, stage_2Rect_2.anchoredPosition, new Vector2(stage_2Rect_2.anchoredPosition.x, minHeight.anchoredPosition.y), animDuration[0], animCurves[0]));
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

        private void ActiveStage(int value)
        {
            // 全てのステージをループで処理
            for (int i = 0; i < stage.Length; i++)
            {
                if (i == value)
                {
                    // 指定されたステージをアクティブ化
                    stage[i].SetActive(true);
                }
                else
                {
                    // それ以外のステージを非アクティブ化
                    stage[i].SetActive(false);
                }
            }
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
