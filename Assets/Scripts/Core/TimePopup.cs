using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Core
{
    public class TimePopup : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] private TimerPopupConfigConfig config;
        
        [Header("References")]
        [SerializeField] private TextMeshPro text;

        private float _timer;

        public void Init(float value)
        {
            text.text = value > 0
                ? $"+{value:0}s"
                : $"{value:0}s";

            text.color = value > 0
                ? config.goodColor
                : config.badColor;
        }

        private void Update()
        {
            transform.position += Vector3.up * config.floatSpeed * Time.deltaTime;

            _timer += Time.deltaTime;
            if (_timer >= config.lifetime)
                Destroy(gameObject);
        }
    }
}