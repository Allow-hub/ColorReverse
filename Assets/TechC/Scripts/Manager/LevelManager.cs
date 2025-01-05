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
        private Coroutine currentGimmickCoroutine;
        private int gimmickCounter = 0;

        private int paletteRow = 0;

        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI levelText;

        public enum Level
        {
            Level_1,
            Level_2,
            Level_3,
            Level_4,
            Level_5,
            AfterLevel_5
        }

        public Level currentLevel = Level.Level_1;
        private int lastLevel;

        private void Awake()
        {
            lastLevel = 0;    
            // ResourcesフォルダからScriptableObjectをロード
            levelCollection = Resources.Load<LevelCollection>("LevelCollection");

            if (levelCollection == null)
            {
                Debug.LogError("LevelCollection could not be loaded from Resources.");
            }
        }
        private void Start()
        {
            if (GameManager.I == null) return;
            paletteRow = GameManager.I.colorRow;
        }
        private void Update()
        {
            if (GameManager.I.currentState == GameManager.GameState.GameOver)
            {
                StopAllCoroutines();
                return;
            }

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
            changeLevelAnimation.StartChangeLevelAnim(currentLevel);
            lastLevel = GameManager.I.GetCurrentLevel();

        }

        private void SetState(Level level)
        {
            currentLevel = level;
            levelText.text = currentLevel.ToString();

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
                case Level.AfterLevel_5:
                    AfterLevel_5Init();
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
                case Level.AfterLevel_5:
                    AfterLevel_5();
                    break;
            }
        }


        private IEnumerator TriggerGimmicksWithDelay(GimmickData[] gimmicks)
        {
            yield return new WaitForSeconds(changeLevelAnimation.GetAnimationDuration(GameManager.I.GetCurrentLevel() - 1));
            yield return new WaitForSeconds(1f);

            gimmickCounter = 0;

            lineGimmick.InitGimmick();
            foreach (var gimmick in gimmicks)
            {
                yield return new WaitForSeconds(gimmick.delay);
                lineGimmick.ShotGimmick(
                    gimmick.type,
                    gimmick.speed,
                    gimmick.initPointNum,
                    gimmick.direction,
                    gimmick.colorColumn,
                    gimmick.isRandomColor
                );

                gimmickCounter++;
                //Debug.Log($"Gimmick triggered. Count: {gimmickCounter}");
            }

            if (currentLevel == Level.AfterLevel_5)
                yield return new WaitForSeconds(levelCollection.levels[4].lastDuration);
            else
                yield return new WaitForSeconds(levelCollection.levels[GameManager.I.GetCurrentLevel() - 1].lastDuration);
            //Debug.Log($"Total gimmicks triggered: {gimmickCounter}");
            lineGimmick.InitGimmick();
            MoveToNextState();
        }


        public void StartLevel(int levelIndex)
        {

            if (levelIndex < 0 || levelIndex >= levelCollection.levels.Length) return;
            // 既存のコルーチンを停止
            if (currentGimmickCoroutine != null)
            {
                StopCoroutine(currentGimmickCoroutine);
                currentGimmickCoroutine = null;
            }

            // ギミックリセット処理
            lineGimmick.InitGimmick();

            LevelData levelData = levelCollection.levels[GameManager.I.GetCurrentLevel() - 1];
            playerController.ReseyHitObj();


            // ギミックの処理を開始
            currentGimmickCoroutine = StartCoroutine(TriggerGimmicksWithDelay(levelData.gimmicks));
        }



        private void Level_1Init()
        {
            SetActiveColors();
            StartLevel(GameManager.I.GetCurrentLevel());
        }

        private void Level_2Init()
        {
            SetActiveColors();
            StartLevel(GameManager.I.GetCurrentLevel());

        }

        private void Level_3Init()
        {
            SetActiveColors();

            StartLevel(GameManager.I.GetCurrentLevel());

        }

        private void Level_4Init()
        {
            SetActiveColors();

            StartLevel(GameManager.I.GetCurrentLevel());
        }

        private void Level_5Init()
        {
            SetActiveColors();
            StartLevel(GameManager.I.GetCurrentLevel());
        }
        private void AfterLevel_5Init()
        {
            // 最後のレベルデータを使用
            LevelData finalLevelData = levelCollection.levels[levelCollection.levels.Length - 1];
            StartLevelWithCustomData(finalLevelData);
        }

        private void StartLevelWithCustomData(LevelData levelData)
        {
            // 既存のコルーチンを停止
            if (currentGimmickCoroutine != null)
            {
                StopCoroutine(currentGimmickCoroutine);
                currentGimmickCoroutine = null;
            }

            // ギミックリセット処理
            lineGimmick.InitGimmick();
            playerController.ReseyHitObj();

            // ギミックの処理を開始
            currentGimmickCoroutine = StartCoroutine(TriggerGimmicksWithDelay(levelData.gimmicks));
        }

        private void SetActiveColors(int startColumn, int count)
        {
            if (activeColor != null)
                activeColor.Clear();
            for (int i = 0; i < count; i++)
            {
                activeColor.Add(new Vector2(paletteRow, startColumn + i));
            }
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
        private void AfterLevel_5()
        {
            Debug.Log("Executing AfLevel 5 logic...");
            // レベル5のステート処理
        }
        private void MoveToNextState()
        {
            // 現在のレベルを次のレベルに移行
            switch (currentLevel)
            {
                case Level.Level_1:
                    GameManager.I.ChangeNextLevelState();
                    ChangeLevel_2State();
                    break;
                case Level.Level_2:
                    GameManager.I.ChangeNextLevelState();
                    ChangeLevel_3State();
                    break;
                case Level.Level_3:
                    GameManager.I.ChangeNextLevelState();
                    ChangeLevel_4State();
                    break;
                case Level.Level_4:
                    GameManager.I.ChangeNextLevelState();
                    ChangeLevel_5State();
                    break;
                case Level.Level_5:
                    GameManager.I.ChangeNextLevelState();
                    ChangeAfterLevel_5State();
                    break;
                case Level.AfterLevel_5:
                    GameManager.I.ChangeNextLevelState();

                    ChangeAfterLevel_5State();
                    break;
            }
        }



        private void SetActiveColors()
        {
            activeColor.Clear();

            if (GameManager.I == null)
            {
                Debug.LogError("GameManager is not initialized.");
                return;
            }

            int currentLevel = GameManager.I.GetCurrentLevel() - 1;
            if (levelCollection == null || levelCollection.levels == null || currentLevel < 0 || currentLevel >= levelCollection.levels.Length)
            {
                Debug.LogError("LevelCollection or levels array is not properly set.");
                return;
            }

            for (int i = 0; i < levelCollection.levels[currentLevel].activeColor + 1; i++)
            {
                activeColor.Add(new Vector2(paletteRow, i));
            }
        }

        public List<float> GetActiveColorYs()
        {
            List<float> yValues = new List<float>();
            foreach (var color in activeColor)
            {
                yValues.Add(color.y);
            }
            return yValues;
        }
        private void ChangeLevel_1State() => SetState(Level.Level_1);
        private void ChangeLevel_2State() => SetState(Level.Level_2);
        private void ChangeLevel_3State() => SetState(Level.Level_3);
        private void ChangeLevel_4State() => SetState(Level.Level_4);
        private void ChangeLevel_5State() => SetState(Level.Level_5);
        private void ChangeAfterLevel_5State() => SetState(Level.AfterLevel_5);


    }
}