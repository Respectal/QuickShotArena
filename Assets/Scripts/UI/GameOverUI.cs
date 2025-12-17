using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            scoreText.text = $"{ScoreManager.LastScore:000}";
        }
        
        public void OnRestartClicked()
        {
            SoundManager.Instance.Play(SoundManager.Instance.uiClickClip, 0.7f);
            SceneManager.LoadScene("GameScene");
        }
        
        public void OnMainMenuClicked()
        {
            SoundManager.Instance.Play(SoundManager.Instance.uiClickClip, 0.7f);
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}