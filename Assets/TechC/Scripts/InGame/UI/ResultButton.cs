using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class ResultButton : MonoBehaviour
    {
        [SerializeField] private Button titleButton, retryButton;
        [SerializeField] private Color color;
        [SerializeField] private TextMeshProUGUI titleText, retryText;

        private void Awake()
        {
            titleButton.onClick.AddListener(() => OnTitle());
            retryButton.onClick.AddListener(() => OnRetry());   
        }


        private void OnTitle()
        {
            GameManager.I.LoadSceneAsync(0);
            titleText.color = color;
            GameManager.I.ChangeTitleState();

        }
        private void OnRetry()
        {
            GameManager.I.LoadSceneAsync(1);
            retryText.color = color;
            GameManager.I.ChangePlayModeState();
        }
    }
}
