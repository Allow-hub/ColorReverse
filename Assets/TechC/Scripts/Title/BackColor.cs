using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class BackColor : MonoBehaviour
    {
        [SerializeField] private int colorColumn = 0;
        [SerializeField] private ColorPalette palette;
        [SerializeField] private int initRow;
        private int lastColorRow = 0;
        private Image image;

        private void OnValidate()
        {
            palette = FindAnyObjectByType<ColorPalette>();
            image = GetComponent<Image>();
            image.color = palette.colors[initRow].colors[colorColumn];

        }

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (GameManager.I.colorRow == lastColorRow) return;
            image.color = palette.colors[GameManager.I.colorRow].colors[colorColumn];
            lastColorRow = GameManager.I.colorRow;
        }
    }
}
