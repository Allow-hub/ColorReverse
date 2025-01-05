using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    [System.Serializable]
    public class GimmickData
    {
        public LineGimmick.ObjType type;
        public float speed;
        public int initPointNum;
        public Vector2 direction;
        public bool isRandomColor;
        public float delay;
        public int colorColumn;
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "Gimmick/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public int activeColor;
        public float lastDuration;
        public GimmickData[] gimmicks;
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelData levelData = (LevelData)target;

            int maxColors = levelData.activeColor + 1;
            string[] options = new string[maxColors];
            for (int i = 0; i < maxColors; i++)
            {
                options[i] = i.ToString();
            }

            levelData.activeColor = EditorGUILayout.IntPopup("Active Color", levelData.activeColor, options, GetIntArray(maxColors));

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
#endif
}
