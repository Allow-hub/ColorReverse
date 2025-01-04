using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class SetColor : MonoBehaviour
    {
        [SerializeField] private ColorPalette palette;
        [SerializeField] protected UIEffect uiEffect;

        [SerializeField] private int paletteRow = 0; 
        [SerializeField] private int paletteColumn = 0;
        [SerializeField] private bool canHit = false;
        [SerializeField] private bool isGimmick = false;
        private const int colZSize = 1;
        private RectTransform rect;
        private BoxCollider boxCollider;
        private Image image;
        private float duration = 1;        

        private void Awake ()
        {
            palette = FindAnyObjectByType<ColorPalette>();
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            if (uiEffect != null)
                uiEffect.transitionRate = 0;

        }
        private void Start()
        {

            if (isGimmick) return;
            paletteRow = GameManager.I.colorRow;
            image.color = palette.colors[paletteRow].colors[paletteColumn];
        }

        private void OnValidate()
        {
            palette = FindAnyObjectByType<ColorPalette>();
            if(palette ==null) return;  
            image = GetComponent<Image>();
            image.color = palette.colors[paletteRow].colors[paletteColumn];

            boxCollider = GetComponent<BoxCollider>();
            if(boxCollider == null) return;
            rect = GetComponent<RectTransform>();
            if (canHit)
                boxCollider.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, colZSize);
        }

        public void SetColorPalette(int row, int column)
        {
            if (row < 0 || row >= palette.colors.Count)
            {
                Debug.LogError($"Invalid row index: {row}. Available rows: 0 to {palette.colors.Count - 1}");
                return;
            }

            if (column < 0 || column >= palette.colors[row].colors.Count)
            {
                Debug.LogError($"Invalid column index: {column}. Available columns in row {row}: 0 to {palette.colors[row].colors.Count - 1}");
                return;
            }

            paletteRow = row;
            paletteColumn = column;
            image.color = palette.colors[paletteRow].colors[paletteColumn];
        }

        public void StartDisable() => StartCoroutine(DisableEffect());

        private IEnumerator DisableEffect()
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                uiEffect.transitionRate = Mathf.Lerp(0f, 1f, elapsed / duration); // 0から1へ補間
                elapsed += Time.deltaTime; // 経過時間を更新
                yield return null;         // 次のフレームまで待機
            }
            uiEffect.transitionRate = 1;
        }

        //public void SetColors(Color col)
        //{
        //    image.color = col;
        //}

        public int GetPaletteRow() => paletteRow;
        public int GetPaletteColumn() => paletteColumn;   
    }
}
