using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class LineGimmick : MonoBehaviour
    {
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private LevelCollection levelCollection;

        [SerializeField] private ColorPalette colorPalette;
        [SerializeField] private GameObject pointParent;
        [SerializeField] private GameObject lineParent;
        [SerializeField] private GameObject circleParent;

        [SerializeField] private GameObject lineObj;
        [SerializeField] private GameObject circleObj;

        [SerializeField] private RectTransform[] points;

        [SerializeField] private Vector2 circleInitSize;

        private List<GameObject> activeObj = new List<GameObject>();
        private GameObject lastObj;
        public enum ObjType
        {
            Line,
            Circle
        }

        private void OnValidate()
        {
            points = GetAllRectTransforms(pointParent);
        }



        private RectTransform[] GetAllRectTransforms(GameObject parent)
        {
            if (parent == null) return new RectTransform[0];
            List<RectTransform> rectTransforms = new List<RectTransform>();
            int childCount = parent.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (child is RectTransform rectTransform)
                {
                    rectTransforms.Add(rectTransform);
                }
            }

            return rectTransforms.ToArray();
        }

        private GameObject[] GetGameObjects(GameObject parent, bool includeInactive)
        {
            if (parent == null) return new GameObject[0];
            List<GameObject> objects = new List<GameObject>();
            int childCount = parent.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = parent.transform.GetChild(i).gameObject;
                if (includeInactive || child.activeSelf)
                {
                    objects.Add(child);
                }
            }

            return objects.ToArray();
        }

        public void ShotGimmick(ObjType type, float speed, int initPointNum, Vector2 direction, int colorColumn, bool isRandomColor)
        {
          

            // 通常の処理
            switch (type)
            {
                case ObjType.Line:
                    LineMove(speed, initPointNum, direction);
                    break;
                case ObjType.Circle:
                    CircleMove(speed, initPointNum);
                    break;
            }

            // カラー設定の処理（既存コード）
            SetColor setColor = lastObj?.GetComponent<SetColor>();
            if (setColor == null || colorPalette == null || colorPalette.colors.Count == 0) return;

            if (!isRandomColor)
            {
                if (IsValidColorIndex(GameManager.I.colorRow, colorColumn))
                {
                    setColor.SetColorPalette(GameManager.I.colorRow, colorColumn);
                }
                else
                {
                    Debug.LogError("Invalid color index specified.");
                }
            }
            else
            {
                int maxColorIndex = 0;
                // 現在のレベルが5以上かチェック
                if (GameManager.I.GetCurrentLevel() > 5)
                {
                    maxColorIndex = Mathf.Min(
                levelCollection.levels[4].activeColor,
                colorPalette.colors[GameManager.I.colorRow].colors.Count - 1);
                }
                else
                {
                    maxColorIndex = Mathf.Min(
                levelCollection.levels[GameManager.I.GetCurrentLevel() - 1].activeColor,
                colorPalette.colors[GameManager.I.colorRow].colors.Count - 1);
                }


                if (maxColorIndex < 0)
                {
                    Debug.LogError("Invalid maximum color index. Ensure activeColor and palette row are correctly configured.");
                    return;
                }

                int randomIndex = Random.Range(0, maxColorIndex + 1);
                setColor.SetColorPalette(GameManager.I.colorRow, randomIndex);
            }
        }

 


        private bool IsValidColorIndex(int row, int column)
        {
            return row >= 0 && row < colorPalette.colors.Count &&
                   column >= 0 && column < colorPalette.colors[row].colors.Count;
        }

        public void InitGimmick()
        {
            //lineIndex = 0;
            //circleIndex = 0;    
            foreach (var obj in activeObj)
            {
                if (obj != null)
                {
                    //objectPool.ReturnObject(obj);
                    obj.SetActive(false);
                }
            }
            activeObj.Clear();
        }

        /// <summary>
        /// initPointからdirectionの方向に一定速度で動かす
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="initPointNum"></param>
        /// <param name="direction"></param>
        private void LineMove(float speed, int initPointNum, Vector2 direction)
        {
            if (points == null || initPointNum < 0 || initPointNum >= points.Length)
            {
                Debug.LogWarning($"Invalid initPointNum ({initPointNum}). points is null or out of range.");
                return;
            }

            GameObject line = objectPool.GetObject(lineObj);
            if (line == null)
            {
                Debug.LogError("Failed to get line object from pool.");
                return;
            }

            if (points[initPointNum] == null)
            {
                Debug.LogError($"points[{initPointNum}] is null.");
                return;
            }

            line.transform.position = points[initPointNum].position;
            SetRotation(line, direction);

            activeObj.Add(line);
            lastObj = line;

            RectTransform rectTransform = line.GetComponent<RectTransform>();
            StartCoroutine(MoveRectTransform(rectTransform, direction, speed));
        }


        private IEnumerator MoveRectTransform(RectTransform rectTransform, Vector2 direction, float speed)
        {
            while (rectTransform.gameObject.activeSelf)
            {
                // 現在の位置に速度と方向を加算
                rectTransform.anchoredPosition += direction * speed * Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// initPointから徐々にサイズを大きくしていく
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="initPointNum"></param>
        private void CircleMove(float speed, int initPointNum)
        {
            GameObject circle = objectPool.GetObject(circleObj);
            if (circle == null) return;

            circle.transform.position = points[initPointNum].position;

            activeObj.Add(circle);
            lastObj = circle;

            RectTransform rect = circle.GetComponent<RectTransform>();
            CapsuleCollider capsuleCollider = circle.GetComponent<CapsuleCollider>();  // カプセルコライダーの取得

            if (rect != null)
            {
                rect.sizeDelta = circleInitSize;

                if (capsuleCollider != null)
                {
                    capsuleCollider.radius = circleInitSize.x / 2;  // 初期サイズに合わせて設定
                }

                StartCoroutine(ExpandCircle(rect, capsuleCollider, speed));
            }
        }

        private IEnumerator ExpandCircle(RectTransform rect, CapsuleCollider capsuleCollider, float speed)
        {
            Vector2 currentSize = circleInitSize;
            while (rect.gameObject.activeSelf)
            {
                currentSize += Vector2.one * speed * Time.deltaTime;
                rect.sizeDelta = currentSize;

                if (capsuleCollider != null)
                {
                    capsuleCollider.radius = currentSize.x / 2;  // 半径を幅の半分に更新
                }

                yield return null;
            }
        }


        private void SetRotation(GameObject obj,Vector2 direction)
        {
            switch (direction)
            {
                case Vector2 up when direction == Vector2.up:
                    // 上方向の場合
                    obj.transform.rotation = Quaternion.identity;
                    break;
                case Vector2 down when direction == Vector2.down:
                    // 下方向の場合
                    obj.transform.rotation = Quaternion.identity;
                    break;
                case Vector2 right when direction == Vector2.right:
                    // 右方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case Vector2 left when direction == Vector2.left:
                    // 左方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case Vector2 rightDown when direction == new Vector2(1, -1):
                    // 右下方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, 45);  // -45度回転
                    break;
                case Vector2 rightUp when direction == new Vector2(1, 1):
                    // 右上方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, -45);  // 45度回転
                    break;
                case Vector2 leftDown when direction == new Vector2(-1, -1):
                    // 左下方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, 135);  // 135度回転
                    break;
                case Vector2 leftUp when direction == new Vector2(-1, 1):
                    // 左上方向の場合
                    obj.transform.rotation = Quaternion.Euler(0, 0, -135);  // -135度回転
                    break;
                default:
                    Debug.Log("Other direction.");
                    break;
            }

        }
    }
}
