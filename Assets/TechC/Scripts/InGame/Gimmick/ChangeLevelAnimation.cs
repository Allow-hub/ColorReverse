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

        [SerializeField] private GameObject stageParent;

        [SerializeField] private GameObject[] stage;
        [Header("AnimationObj")]
        [SerializeField] private RectTransform stage_1Rect;
        [SerializeField] private RectTransform stage_2Rect, stage_2Rect_2;
        [SerializeField] private RectTransform stage_3Rect;
        [SerializeField] private RectTransform stage_4Rect;
        [SerializeField] private RectTransform stage_5Rect;
        [SerializeField] private GameObject levelText;


        private void OnValidate()
        {
            int childCount = stageParent.transform.childCount;
            stage =new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                stage[i] = stageParent.transform.GetChild(i).gameObject;    
            }
        }

        public void StartChangeLevelAnim(Level level) => StartCoroutine(ChangeLevelAnim(level));

        private IEnumerator ChangeLevelAnim(Level level)
        {

            switch (level)
            {
                case Level.Level_1:
                    ActiveStage(GameManager.I.GetCurrentLevel() - 1);
                    yield return Animate(stage_1Rect, Vector2.zero, minWidth.anchoredPosition, animDuration[0], animCurves[0]);
                    break;

                case Level.Level_2:
                    ActiveStage(GameManager.I.GetCurrentLevel() - 1);
                    StartCoroutine(Animate(stage_2Rect, stage_2Rect.anchoredPosition, new Vector2(stage_2Rect.anchoredPosition.x, minHeight.anchoredPosition.y), animDuration[0], animCurves[0]));
                    StartCoroutine(Animate(stage_2Rect_2, stage_2Rect_2.anchoredPosition, new Vector2(stage_2Rect_2.anchoredPosition.x, minHeight.anchoredPosition.y), animDuration[0], animCurves[0]));
                    break;

                case Level.Level_3:
                    ActiveStage(GameManager.I.GetCurrentLevel() - 1);
                    yield return AnimateScale(stage_3Rect, stage_3Rect.localScale, new Vector2(0, 1), animDuration[0], animCurves[0]);

                    break;

                case Level.Level_4:
                    ActiveStage(GameManager.I.GetCurrentLevel() - 1);
                    yield return AnimateScale(stage_4Rect, stage_4Rect.localScale, new Vector2(0, 1), animDuration[0], animCurves[0]);
                    break;

                case Level.Level_5:
                    ActiveStage(GameManager.I.GetCurrentLevel() - 1);
                    yield return AnimateScale(stage_5Rect, stage_5Rect.localScale, new Vector2(0, 0), animDuration[0], animCurves[0]);
                    break;
                case Level.AfterLevel_5:
                    StartCoroutine(ActiveLevelText());  
                    break;
            }

            GameManager.I.ChangePlayModeState();
        }

        private IEnumerator ActiveLevelText()
        {
            levelText.SetActive(true);
            yield return new WaitForSeconds(animDuration[0]);
            levelText.SetActive(false);
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
            yield return new WaitForSeconds(1f);

            levelText.SetActive(false);

        }

        /// <summary>
        /// スケールアニメーションを行う汎用メソッド
        /// </summary>
        private IEnumerator AnimateScale(Transform target, Vector3 startScale, Vector3 endScale, float duration, AnimationCurve curve)
        {
            float elapsedTime = 0f;
            levelText.SetActive(true);

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration; // 進行割合（0～1）
                float curveValue = curve.Evaluate(t); // AnimationCurve に基づいた割合

                // Lerp でスケールを補間
                target.localScale = Vector3.Lerp(startScale, endScale, curveValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終スケールを明示的に設定
            target.localScale = endScale;
            yield return new WaitForSeconds(1f);
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
                return animDuration[0];  // デフォルトのアニメーション時間
            }
        }
    }
}
