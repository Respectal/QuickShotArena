using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnPlayClicked()
        {
            SoundManager.Instance.Play(SoundManager.Instance.uiClickClip, 0.7f);
            SceneManager.LoadScene("GameScene");
        }
        
        public void OnQuitClicked()
        {
            SoundManager.Instance.Play(SoundManager.Instance.uiClickClip, 0.7f);
            Application.Quit();
        }
    }
}