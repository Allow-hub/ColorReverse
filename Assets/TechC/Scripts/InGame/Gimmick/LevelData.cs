using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        public int colorColumn;         // ギミックの色
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "Gimmick/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public string levelName;                // レベル名
        public int activeColor;
        public GimmickData[] gimmicks;          // ギミックの配列
    }

    [CreateAssetMenu(fileName = "LevelCollection", menuName = "Gimmick/LevelCollection", order = 2)]
    public class LevelCollection : ScriptableObject
    {
        public LevelData[] levels; // レベルデータの配列
    }

    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // ベースクラスのインスペクターGUIを表示
            base.OnInspectorGUI();

            // LevelDataターゲット取得
            LevelData levelData = (LevelData)target;

            // 選択肢を用意 (activeColor の最大値まで)
            int maxColors = levelData.activeColor + 1;
            string[] options = new string[maxColors];
            for (int i = 0; i < maxColors; i++)
            {
                options[i] = i.ToString();
            }

            // activeColorの選択 (プルダウン)
            levelData.activeColor = EditorGUILayout.IntPopup("Active Color", levelData.activeColor, options, GetIntArray(maxColors));

            // gimmicks 配列内の colorColumn のプルダウン表示
            if (levelData.gimmicks != null && levelData.gimmicks.Length > 0)
            {
                EditorGUILayout.LabelField("Gimmicks", EditorStyles.boldLabel);
                for (int i = 0; i < levelData.gimmicks.Length; i++)
                {
                    EditorGUILayout.LabelField($"Gimmick {i + 1}");
                    levelData.gimmicks[i].colorColumn = EditorGUILayout.IntPopup(
                        "Color Column",
                        levelData.gimmicks[i].colorColumn,
                        options,
                        GetIntArray(maxColors)
                    );
                }
            }

            // データが変更されたことをUnityに通知
            if (GUI.changed)
            {
                EditorUtility.SetDirty(levelData);
            }
        }

        private int[] GetIntArray(int length)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = i;
            }
            return array;
        }
    }
}
