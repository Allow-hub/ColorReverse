using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class TitleUi : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button[] colorChangeButton;
        [SerializeField] private float fadeDuration = 1;
        private void Awake()
        {
            startButton.onClick.AddListener(OnStart);
            colorChangeButton[0].onClick.AddListener(() => ColorChange(0));
            colorChangeButton[1].onClick.AddListener(() => ColorChange(1));

        }

        private void Start()
        {
            GameManager.I.ChangeTitleState();
        }
        private void OnStart()
        {
            SeManager.I.PlaySE(0);
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            GameManager.I.ShotFade(fadeDuration/2);
            yield return new WaitForSeconds(fadeDuration/2);
            GameManager.I.ChangePlayModeState();
            GameManager.I.LoadSceneAsync(1);
        }

        private void ColorChange(int n)
        {
            SeManager.I.PlaySE(0);
            GameManager.I.colorRow = n;
        }
    }
}
