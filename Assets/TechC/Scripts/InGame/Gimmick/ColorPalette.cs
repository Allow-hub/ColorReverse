using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// ゲーム内で使う色のリスト
    /// </summary>
    [System.Serializable]
    public class Colors
    {
        public List<Color> colors = new List<Color>();
    }

    public class ColorPalette : MonoBehaviour
    {
        // インスペクターで編集可能なリストのリスト
        public List<Colors> colors = new List<Colors>();
    }
}
