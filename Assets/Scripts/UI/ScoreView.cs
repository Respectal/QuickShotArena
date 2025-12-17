using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            UpdateText(scoreManager.CurrentScore);
        }

        private void UpdateText(int score)
        {
            text.text = $"{score:000}";
        }
    }
}