
using UnityEngine;

namespace TechC
{

    [CreateAssetMenu(fileName = "LevelCollection", menuName = "Gimmick/LevelCollection", order = 2)]
    public class LevelCollection : ScriptableObject
    {
        public LevelData[] levels;
    }
}
