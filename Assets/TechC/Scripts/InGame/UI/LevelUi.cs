using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class LevelUi : MonoBehaviour
    {
        private TextMeshProUGUI tex;
        private void Awake()
        {
            tex = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            tex.text ="Level " + GameManager.I.GetCurrentLevel().ToString();
        }
    }
}
