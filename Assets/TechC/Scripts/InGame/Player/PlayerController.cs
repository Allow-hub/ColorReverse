using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    /// <summary>
    /// プレイヤーの操作するオブジェクトの挙動
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Animator anim;
        [SerializeField] private ColorPalette palette;    
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private ObjectPool objectPool;

        [SerializeField] private Texture2D customCursor;  // カスタムカーソル
        [SerializeField] private GameObject mouseObj;     // マウスオブジェクト
        [SerializeField] private LayerMask raycastLayer;  // レイキャスト対象のレイヤー
        [SerializeField] private GameObject scoreTextPrefab;
        [SerializeField] private Vector2 xRange;
        [SerializeField] private Vector2 yRange;
        //private List<GameObject> scoreText=new List<GameObject>();
        //private const int scoreTextIndex = 0;
        private GameObject lastHitObject = null;
        private bool isScaling = false; // アニメーション中かどうかを判定するフラグ


        [Header("ColorChange")]
        [SerializeField] private float animDelay = 1;
        [SerializeField] private float fillDuration = 1;
        [SerializeField] private Image frontImage, backImage;
        [SerializeField] private GameObject frontImageObj;
        [SerializeField] private string colorImageTag;
        [SerializeField] private LayerMask colorChangeLayer;
        [SerializeField] private int addColorChangeScore;
        private int currentColorColumn; //現在乗っている色

        [Header("Gimmick")]
        private List<GameObject> hitObjects = new List<GameObject>();
        [SerializeField] private int addGimmickScore;

        public enum AnimationType
        {
            Normal,//y軸のみの回転
            Round//回る
        }

        [SerializeField] private float distance = 1000;


        private void Awake()
        {
            // カスタムカーソルを設定
            if (customCursor != null)
            {
                Cursor.SetCursor(customCursor, new Vector2(customCursor.width / 2, customCursor.height / 2), CursorMode.ForceSoftware);
            }
            else
            {
                Debug.LogWarning("カスタムカーソルが設定されていません。");
            }

            // カーソルを画面内に制限
            Cursor.lockState = CursorLockMode.Confined;

        }
    

        private void Update()
        {
            if (mouseObj != null)
            {
                // マウス位置からレイを飛ばす
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // デバッグ用にレイを描画
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

                // レイキャストがヒットした場合
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
                {
                    // ヒット位置にmouseObjを移動
                    mouseObj.transform.position = hit.point;


                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, colorChangeLayer))
                {
                    // もし、現在当たっているオブジェクトが前回と違う場合
                    if (hit.transform.gameObject != lastHitObject)
                    {
                        if (hit.collider.CompareTag(colorImageTag))
                        {
                            HitEvent(hit.transform.tag, hit.transform.gameObject);  // 新しいオブジェクトに対して処理を実行
                        }

                        lastHitObject = hit.transform.gameObject;  // 現在のオブジェクトを記録
                    }

                }
            }
            else
            {
                Debug.LogWarning("mouseObjが設定されていません。");
            }
        }

        private IEnumerator StartChange(int paletteIndex, int colorIndex, AnimationType animationType)
        {
            // アニメーションタイプに応じたトリガーを設定
            switch (animationType)
            {
                case AnimationType.Normal:
                    anim.ResetTrigger("IsChanging");
                    anim.ResetTrigger("IsChangingRoundRotation");
                    anim.SetTrigger("IsChanging");
                    break;
                case AnimationType.Round:
                    anim.ResetTrigger("IsChanging");
                    anim.ResetTrigger("IsChangingRoundRotation");
                    anim.SetTrigger("IsChangingRoundRotation");
                    break;
            }

            // フィルアニメーション開始
            if (frontImage != null)
            {
                float elapsedTime = 0f;
                frontImageObj.SetActive(true);
                frontImage.fillAmount = 0;
                ColorChange(frontImage, paletteIndex, colorIndex);
                // frontImageのfillAmountを0から1にアニメーション
                while (elapsedTime < fillDuration)
                {
                    elapsedTime += Time.deltaTime;
                    frontImage.fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
                    yield return null; // 次フレームまで待機
                }

                // フィルアニメーション完了後にbackImageの色を変更
                ColorChange(backImage,paletteIndex, colorIndex);
                frontImageObj.SetActive(false);

            }
            else
            {
                Debug.LogWarning("frontImageが設定されていません。");
            }
        }


        private void ColorChange(Image image,int paletteIndex, int colorIndex)
        {
            // パレットと色の範囲を確認
            if (palette == null)
            {
                Debug.LogWarning("カラーパレットが設定されていません。");
                return;
            }

            if (paletteIndex < 0 || paletteIndex >= palette.colors.Count)
            {
                Debug.LogWarning($"無効なパレットインデックス: {paletteIndex}");
                return;
            }

            if (colorIndex < 0 || colorIndex >= palette.colors[paletteIndex].colors.Count)
            {
                Debug.LogWarning($"無効なカラーパレットインデックス: {colorIndex}");
                return;
            }
            if (image != null)
            {
                image.color = palette.colors[paletteIndex].colors[colorIndex];
            }
            else
            {
                Debug.LogWarning("mouseObjにImageコンポーネントがありません。");
            }
        }

        private void HitEvent(string tag, GameObject hitObj)
        {
            GameManager.I.AddScore(addColorChangeScore);
            AppearScoreText(mouseObj.transform, addColorChangeScore);
            System.Random random = new System.Random();
            Array enumValues = Enum.GetValues(typeof(AnimationType));

            AnimationType type = (AnimationType)enumValues.GetValue(random.Next(enumValues.Length));
            // SetColorコンポーネントを取得
            SetColor setColor = hitObj.GetComponent<SetColor>();
            int currentColumn = setColor.GetPaletteColumn();
            currentColorColumn = currentColumn;
            // activeColorのリスト内のすべてのy値を取得
            List<float> yValues = new List<float>();
            foreach (var colorData in levelManager.activeColor)
            {
                yValues.Add(colorData.y);  // yの値をリストに追加
            }

            // 一致しないy値のみをフィルタリング
            List<float> nonMatchingYValues = yValues.FindAll(y => y != currentColumn);

            if (nonMatchingYValues.Count > 0)
            {
                // 一致しないy値の中からランダムに選択
                float randomY = nonMatchingYValues[random.Next(nonMatchingYValues.Count)];

                // 選択したy値に対応する処理を追加
                int paletteIndex = (int)levelManager.activeColor[0].x;
                StartCoroutine(StartChange(paletteIndex, (int)randomY, type));
            }
            else
            {
                Debug.LogWarning("一致しないy値がありません。");
            }
        }

        public void ReseyHitObj()=>hitObjects.Clear();


        private void OnTriggerEnter(Collider other)
        {
            // 衝突したオブジェクトが「Gimmick」タグを持つ場合
            if (other.gameObject.CompareTag("Gimmick"))
            {
                // 重複追加を防ぐ（既にリストに存在する場合は処理しない）
                if (hitObjects.Contains(other.gameObject)) return;
                SetColor setColor = other.GetComponent<SetColor>();
                int colorColumn = setColor.GetPaletteColumn();

                if(colorColumn !=currentColorColumn)return;
                hitObjects.Add(other.gameObject);
                StartCoroutine(ScaleMouseObj());

                GameManager.I.AddScore(addGimmickScore);
                AppearScoreText(mouseObj.transform,addGimmickScore);
            }
        }

        private void AppearScoreText(Transform pos, int score)
        {
            // プレイヤーの位置から半径3m以内のランダムなオフセットを生成
            //Vector3 randomOffset = new Vector3(
            //    UnityEngine.Random.Range(xRange.x, xRange.y),
                                  
            //    UnityEngine.Random.Range(yRange.x, yRange.y) ,
            //    pos.position.z
            //);

            // プレイヤーの位置にランダムなオフセットを加える
            //Vector3 randomPosition = pos.position + randomOffset;

            // オブジェクトプールからスコアテキストオブジェクトを取得
            GameObject scoreObj = objectPool.GetObject(scoreTextPrefab);
            scoreObj.transform.position = pos.position ;  // ランダムな位置に設定
            // スコアテキストを設定
            ScoreText scoreText = scoreObj.GetComponent<ScoreText>();
            scoreText.SetText(score.ToString());
        }

        private IEnumerator ScaleMouseObj()
        {
            isScaling = true; // アニメーションが進行中の間は新たに始めないようにする

            Vector3 originalScale = mouseObj.transform.localScale;  // 元のスケール
            Vector3 targetScale = originalScale * 1.2f;  // 目標のスケール（20%拡大）

            float elapsedTime = 0f;
            float scaleDuration = 0.5f;  // 変化時間（0.5秒）

            // スケールアップ処理
            while (elapsedTime < scaleDuration)
            {
                mouseObj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mouseObj.transform.localScale = targetScale; // 正確に目標スケールに到達させる

            elapsedTime = 0f;  // 再度計測をリセット

            // スケールダウン処理
            while (elapsedTime < scaleDuration)
            {
                mouseObj.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mouseObj.transform.localScale = originalScale; // 元のスケールに戻す

            isScaling = false; // アニメーション終了後、新たなアクションを可能にする
        }

    }
}
