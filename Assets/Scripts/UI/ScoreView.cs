using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            UpdateText(ScoreManager.Instance.CurrentScore);
        }

        private void UpdateText(int score)
        {
            text.text = $"{score:000}";
        }
    }
}