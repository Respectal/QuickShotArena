using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TimerManager timer;
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            UpdateText(timer.TimeRemaining);
        }

        private void UpdateText(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60f);
            var seconds = Mathf.FloorToInt(time % 60f);

            text.text = $"{minutes:00}:{seconds:00}";

            text.color = time <= 10f
                ? Color.red
                : Color.white;
        }
    }
}