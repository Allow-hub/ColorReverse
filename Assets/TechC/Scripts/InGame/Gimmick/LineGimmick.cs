using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class LineGimmick : MonoBehaviour
    {
        [SerializeField] private ObjectPool objectPool;

        [SerializeField] private ColorPalette colorPalette;
        [SerializeField] private GameObject pointParent;
        [SerializeField] private GameObject lineParent;
        [SerializeField] private GameObject circleParent;

        [SerializeField] private RectTransform[] linesRect;
        [SerializeField] private RectTransform[] circlesRect;
        [SerializeField] private GameObject[] linesObj;
        [SerializeField] private GameObject[] circlesObj;

        [SerializeField] private RectTransform[] points;

        [SerializeField] private Vector2 circleInitSize;

        private List<GameObject> activeObj = new List<GameObject>();
        private GameObject lastObj;
        private int lineIndex = 0;
        private int circleIndex = 0;
        public enum ObjType
        {
            Line,
            Circle
        }

        private void OnValidate()
        {
            points = GetAllRectTransforms(pointParent);
            linesRect = GetAllRectTransforms(lineParent);
            linesObj = GetGameObjects(lineParent, true);
            circlesRect = GetAllRectTransforms(circleParent);
            circlesObj = GetGameObjects(circleParent,true);
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

        public void ShotGimmick(ObjType type, float speed, int initPointNum, Vector2 direction,int colorColumn,bool isRandomColor)
        {
            switch (type)
            {
                case ObjType.Line:
                    LineMove(speed, initPointNum, direction);
                    break;
                case ObjType.Circle:
                    CircleMove(speed, initPointNum);
                    break;
            }
            SetColor setColor = lastObj.gameObject.GetComponent<SetColor>();

            if (!isRandomColor && colorPalette != null && colorPalette.colors.Count > 0)
            {
                setColor.SetColorPalette(GameManager.I.colorRow,colorColumn);
            }
            if (isRandomColor && colorPalette != null && colorPalette.colors.Count > 0)
            {
                // 最後の Colors オブジェクトを取得
                Colors lastColors = colorPalette.colors[colorPalette.colors.Count - 1];
                
                int rand = Random.Range(0, colorPalette.colors.Count);
                setColor.SetColorPalette(GameManager.I.colorRow, rand);
            }
        }

        public void InitGimmick()
        {
            lineIndex = 0;
            circleIndex = 0;    
            foreach (var obj in activeObj)
            {
                if (obj != null)
                {
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
            if (initPointNum < 0 || initPointNum >= points.Length || initPointNum >= linesObj.Length) return;

            GameObject line = linesObj[lineIndex];
            if (line == null) return;

            // オブジェクトを再利用
            line.SetActive(true);
            line.transform.position = points[initPointNum].position;
            line.transform.rotation = Quaternion.identity;

            activeObj.Add(line);
            lastObj = line;

            StartCoroutine(MoveRectTransform(linesRect[lineIndex], direction, speed));
            lineIndex++;

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
            if (initPointNum < 0 || initPointNum >= points.Length || initPointNum >= circlesObj.Length) return;

            GameObject circle = circlesObj[circleIndex];
            circleIndex++;  
            if (circle == null) return;

            // オブジェクトを再利用
            circle.SetActive(true);
            circle.transform.position = points[initPointNum].position;

            activeObj.Add(circle);
            lastObj = circle;

            RectTransform rect = circle.GetComponent<RectTransform>();
            if (rect != null)
            {
                // サイズを初期化
                rect.sizeDelta = circleInitSize;
                StartCoroutine(ExpandCircle(rect, speed));
            }
        }


        private IEnumerator ExpandCircle(RectTransform rect, float speed)
        {
            Vector2 currentSize = circleInitSize;
            while (rect.gameObject.activeSelf)
            {
                currentSize += Vector2.one * speed * Time.deltaTime;
                rect.sizeDelta = currentSize;
                yield return null;
            }
        }
    }
}
