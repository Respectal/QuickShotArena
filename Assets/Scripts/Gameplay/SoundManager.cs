using UnityEngine;

namespace Gameplay
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Clips")]
        public AudioClip ballHitClip;
        public AudioClip scoreClip;
        public AudioClip penaltyClip;
        public AudioClip uiClickClip;

        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Play(AudioClip clip, float volume = 1f)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}