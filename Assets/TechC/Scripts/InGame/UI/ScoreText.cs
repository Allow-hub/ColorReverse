using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private float appearTime = 0.7f;
        private ObjectPool objectPool;
        private TextMeshProUGUI tex;
        private float elapsedTime;

        private void Awake()
        {
            tex = GetComponent<TextMeshProUGUI>();
            GameObject objPool = GameObject.FindWithTag("ObjectPool");
            objectPool = objPool.GetComponent<ObjectPool>();
        }
        private void OnEnable()
        {
            elapsedTime = 0;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > appearTime)
            {
                objectPool.ReturnObject(gameObject);
                elapsedTime = 0;
            }
        }

        public void SetText(string str)=>tex.text = str;
    }
}
