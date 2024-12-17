using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private ChangeLevelAnimation changeLevelAnimation;
        [SerializeField] private LevelCollection levelCollection;
        [SerializeField] private LineGimmick lineGimmick;
        [SerializeField] private ColorPalette colorPalette;
        [SerializeField] private PlayerController playerController;

        public List<Vector2> activeColor = new List<Vector2>();

        private int paletteRow = 0;

        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI levelText;

        public enum Level
        {
            Level_1,
            Level_2,
            Level_3,
            Level_4,
            Level_5
        }

        public Level currentLevel = Level.Level_1;
        private int lastLevel;

        private void Awake()
        {
            lastLevel = 0;
        }
        private void Start()
        {
            if (GameManager.I == null) return;
            paletteRow =GameManager.I.colorRow;
        }
        private void Update()
        {
            StateHandler();

            if (lastLevel == GameManager.I.GetCurrentLevel()) return;
            switch (GameManager.I.GetCurrentLevel())
            {
                case 1:
                    ChangeLevel_1State();
                    break;
                case 2:
                    ChangeLevel_2State();
                    break;
                case 3:
                    ChangeLevel_3State();
                    break;
                case 4:
                    ChangeLevel_4State();
                    break;
                case 5:
                    ChangeLevel_5State();
                    break;
            }
            lastLevel = GameManager.I.GetCurrentLevel();
            levelText.text = currentLevel.ToString();
            changeLevelAnimation.StartChangeLevelAnim(currentLevel);
        }

        private void SetState(Level level)
        {
            currentLevel = level;
            switch (currentLevel)
            {
                case Level.Level_1:
                    Level_1Init();
                    break;
                case Level.Level_2:
                    Level_2Init();
                    break;
                case Level.Level_3:
                    Level_3Init();
                    break;
                case Level.Level_4:
                    Level_4Init();
                    break;
                case Level.Level_5:
                    Level_5Init();
                    break;
            }
        }

        private void StateHandler()
        {
            switch (currentLevel)
            {
                case Level.Level_1:
                    Level_1();
                    break;
                case Level.Level_2:
                    Level_2();
                    break;
                case Level.Level_3:
                    Level_3();
                    break;
                case Level.Level_4:
                    Level_4();
                    break;
                case Level.Level_5:
                    Level_5();
                    break;
            }
        }
        public void StartLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levelCollection.levels.Length) return;

            LevelData levelData = levelCollection.levels[GameManager.I.GetCurrentLevel() - 1];
            playerController.ReseyHitObj();
            // ギミックの処理を開始
            StartCoroutine(TriggerGimmicksWithDelay(levelData.gimmicks));
        }

        private IEnumerator TriggerGimmicksWithDelay(GimmickData[] gimmicks)
        {
            foreach (var gimmick in gimmicks)
            {
                // ギミックの遅延を処理
                yield return new WaitForSeconds(gimmick.delay);

                // ギミックを発火
                lineGimmick.ShotGimmick(
                    gimmick.type,
                    gimmick.speed,
                    gimmick.initPointNum,
                    gimmick.direction,
                    gimmick.colorColumn,
                    gimmick.isRandomColor
                );
            }
        }



        private void Level_1Init()
        {
            activeColor.Clear();
            activeColor.Add(new Vector2(paletteRow, 0));
            activeColor.Add(new Vector2(paletteRow, 1));
            StartLevel(GameManager.I.GetCurrentLevel());
        }

        private void Level_2Init()
        {
            Debug.Log("Initializing Level 2...");
            // レベル2の初期化処理
        }

        private void Level_3Init()
        {
            Debug.Log("Initializing Level 3...");
            // レベル3の初期化処理
        }

        private void Level_4Init()
        {
            Debug.Log("Initializing Level 4...");
            // レベル4の初期化処理
        }

        private void Level_5Init()
        {
            Debug.Log("Initializing Level 5...");
            // レベル5の初期化処理
        }

        private void Level_1()
        {
            Debug.Log("Executing Level 1 logic...");
            // レベル1のステート処理
        }

        private void Level_2()
        {
            Debug.Log("Executing Level 2 logic...");
            // レベル2のステート処理
        }

        private void Level_3()
        {
            Debug.Log("Executing Level 3 logic...");
            // レベル3のステート処理
        }

        private void Level_4()
        {
            Debug.Log("Executing Level 4 logic...");
            // レベル4のステート処理
        }

        private void Level_5()
        {
            Debug.Log("Executing Level 5 logic...");
            // レベル5のステート処理
        }

        private void ChangeLevel_1State() => SetState(Level.Level_1);
        private void ChangeLevel_2State() => SetState(Level.Level_2);
        private void ChangeLevel_3State() => SetState(Level.Level_3);
        private void ChangeLevel_4State() => SetState(Level.Level_4);
        private void ChangeLevel_5State() => SetState(Level.Level_5);
    }
}
