using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class SetColor : MonoBehaviour
    {
        [SerializeField] private ColorPalette palette;

        [SerializeField] private int paletteRow = 0; 
        [SerializeField] private int paletteColumn = 0;
        [SerializeField] private bool canHit = false;
        [SerializeField] private bool isGimmick = false;
        private const int colZSize = 1;
        private RectTransform rect;
        private BoxCollider boxCollider;
        private Image image;

        private void Awake ()
        {
            palette = FindAnyObjectByType<ColorPalette>();
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
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
            rect = GetComponent<RectTransform>();
            if (canHit)
                boxCollider.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, colZSize);
        }

        public void SetColorPalette(int row ,int column)
        {
            paletteRow = row;
            paletteColumn = column;
            image.color = palette.colors[paletteRow].colors[paletteColumn];
        }

        //public void SetColors(Color col)
        //{
        //    image.color = col;
        //}

        public int GetPaletteRow() => paletteRow;
        public int GetPaletteColumn() => paletteColumn;   
    }
}
