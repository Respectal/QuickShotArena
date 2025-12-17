using Core.Enums;
using Gameplay;
using ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class HoleTile : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] public HoleTileConfig config;
        
        [Header("References")]
        [SerializeField] private Renderer holeRenderer;
        
        [Header("Popup")]
        [SerializeField] private TimePopup timePopupPrefab;
        
        public HoleType CurrentType { get; private set; }

        private float _switchTimer;
        private bool _isFlashing;

        public void FullHoleReset()
        {
            _isFlashing = false;
            SetRandomType(config.goodOnSwitch);
            ResetSwitchTimer();
            UpdateColor();
        }
        
        private void Start()
        {
            SetRandomType(config.goodOnSpawn);
            ResetSwitchTimer();
            UpdateColor();
        }

        private void Update()
        {
            _switchTimer -= Time.deltaTime;

            if (_switchTimer <= config.flashDuration && !_isFlashing)
            {
                _isFlashing = true;
            }

            if (_switchTimer <= 0f)
            {
                SwitchType();
            }

            if (_isFlashing)
            {
                Flash();
            }
        }
        
        private void OnEnable()
        {
            GameEvents.TimeChanged += OnTimeChanged;
        }

        private void OnDisable()
        {
            GameEvents.TimeChanged -= OnTimeChanged;
        }
        
        private void OnTimeChanged(HoleTile holeTile, Vector3 popupPosition, float delta)
        {
            if (holeTile != this)
            {
                return;
            }
            
            SpawnPopup(popupPosition, delta);
        }
        
        void SpawnPopup(Vector3 popupPosition, float delta)
        {
            var popup = Instantiate(timePopupPrefab, popupPosition, Quaternion.identity);
            popup.Init(delta);
        }

        void SwitchType()
        {
            SetRandomType(config.goodOnSwitch);

            _isFlashing = false;
            ResetSwitchTimer();
            UpdateColor();
        }

        void SetRandomType(float probability)
        {
            CurrentType = Random.value < probability
                ? HoleType.Good
                : HoleType.Bad;
        }

        void ResetSwitchTimer()
        {
            _switchTimer = Random.Range(config.minSwitchTime, config.maxSwitchTime);
        }

        void UpdateColor()
        {
            holeRenderer.material.color =
                CurrentType == HoleType.Good ? config.goodColor : config.badColor;
        }

        void Flash()
        {
            var baseColor = CurrentType == HoleType.Good ? config.goodColor : config.badColor;
            var flashColor = CurrentType == HoleType.Good ? config.goodFlashColor : config.badFlashColor;
            var t = Mathf.Abs(Mathf.Sin(Time.time * config.flashSpeed));
            holeRenderer.material.color = Color.Lerp(baseColor, flashColor, t);
        }
    }
}