using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [System.Serializable]
    public class GimmickData
    {
        public LineGimmick.ObjType type; // ギミックの種類 (Line or Circle)
        public float speed;             // ギミックの速度
        public int initPointNum;        // 初期ポイント番号
        public Vector2 direction;       // 移動方向
        public bool isRandomColor;      // ランダム色にするか
        public float delay;             // ギミック開始までの遅延時間
        public int colorColumn;         //ギミックの色
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "Gimmick/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public string levelName;                // レベル名
        public GimmickData[] gimmicks;          // ギミックの配列
    }

    [CreateAssetMenu(fileName = "LevelCollection", menuName = "Gimmick/LevelCollection", order = 2)]
    public class LevelCollection : ScriptableObject
    {
        public LevelData[] levels; // レベルデータの配列
    }
}
