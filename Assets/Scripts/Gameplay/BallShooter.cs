using System;
using Core;
using Core.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class BallShooter : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BallShooterConfig config;
        
        [Header("References")]
        public TileManager tileManager;
        public TrajectoryPreview trajectoryPreview;

        private TrajectoryCalculator _trajectoryCalculator;

        private float _currentAngle;
        private float _currentPower;
        private float _powerTime;
        private float _reloadTimer;
        
        private GameObject _currentBall;
        private Rigidbody _currentRb;
        private ShooterState _state = ShooterState.Idle;
        private Camera _camera;
        private Plane _aimPlane;

        private InputAction _aimAction;
        private InputAction _shootAction;

        private Vector3 _shootPosition;

        private void Awake()
        {
            _trajectoryCalculator = new TrajectoryCalculator();
            _camera = Camera.main;

            _aimAction = InputSystem.actions.FindAction("Aim");
            _shootAction = InputSystem.actions.FindAction("Shoot");
            
            var tileSize = GetTileSize();
            var startX = tileSize.x * tileManager.config.width / 2 - tileSize.x / 2;
            
            _shootPosition = new Vector3(startX, tileManager.transform.position.y + 0.45f, tileManager.transform.position.z);
            _aimPlane = new Plane(Vector3.up, _shootPosition);
            
            
            LoadBall();
        }

        private void Update()
        {
            switch (_state)
            {
                case ShooterState.Idle:
                    if (_shootAction.WasPressedThisFrame())
                    {
                        _currentAngle = 0f;
                        _powerTime = 0f;
                        _state = ShooterState.Aiming;
                    }
                    break;

                case ShooterState.Aiming:
                    if (_shootAction.IsPressed())
                    {
                        UpdateDirection();
                        UpdatePower();
                        UpdateTrajectory();
                    }

                    if (_shootAction.WasReleasedThisFrame())
                    {
                        Shoot();
                    }
                    break;

                case ShooterState.Cooldown:
                    _reloadTimer -= Time.deltaTime;
                    if (_reloadTimer <= 0f)
                    {
                        LoadBall();
                    }
                    break;
            }
        }

        void Shoot()
        {
            trajectoryPreview.Hide();

            _currentRb.isKinematic = false;

            var direction = UpdateDirectionFromPointer();
            _currentRb.AddForce(direction * _currentPower, ForceMode.Impulse);
            SoundManager.Instance.Play(SoundManager.Instance.ballHitClip, 0.7f);
            
            Destroy(_currentBall, config.ballLifeSpan);

            _currentBall = null;
            _currentRb = null;

            _reloadTimer = config.reloadDelay;
            _state = ShooterState.Cooldown;
            
            _currentAngle = 0f;
            _currentPower = config.powerMin;
        }

        private void UpdateTrajectory()
        {
            var direction = UpdateDirectionFromPointer();
            var velocity = direction * _currentPower;

            var points = _trajectoryCalculator.CalculateTrajectory(
                _shootPosition,
                velocity,
                config.trajectoryPoints,
                config.timeStep
            );

            trajectoryPreview.Show(points, GetTrajectoryColor());
        }
        
        private Vector3 UpdateDirectionFromPointer()
        {
            var screenPos = Pointer.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(screenPos);

            if (_aimPlane.Raycast(ray, out var enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var dir = hitPoint - _shootPosition;
                dir.y = 0f;

                if (dir.sqrMagnitude > 0.001f)
                    return dir.normalized;
            }

            return Vector3.forward;
        }
        
        private void UpdatePower()
        {
            _powerTime += Time.deltaTime * config.powerCycleSpeed;

            var oscillating = Mathf.PingPong(_powerTime, 1f);

            _currentPower = Mathf.Lerp(config.powerMin, config.powerMax, EaseOutCubic(oscillating));
        }

        private void UpdateDirection()
        {
            var horizontal = _aimAction.ReadValue<Vector2>().x;
            _currentAngle += horizontal;
            _currentAngle = Mathf.Clamp(_currentAngle, -90f, 90f);
        }

        Vector3 GetTileSize()
        {
            // Get the renderer bounds if available
            if (config.tileSolid.TryGetComponent<Renderer>(out var renderComponent))
            {
                return renderComponent.bounds.size;
            }
    
            // Fallback: check for collider
            if (config.tileSolid.TryGetComponent<Collider>(out var colliderComponent))
            {
                return colliderComponent.bounds.size;
            }
    
            // Default fallback (1x1 tiles)
            Debug.LogWarning("TileSolid has no Renderer or Collider. Using default size (1, 1, 1)");
            return Vector3.one;
        }

        float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }
        
        Color GetTrajectoryColor()
        {
            var t = Mathf.InverseLerp(config.powerMin, config.powerMax, _currentPower);

            if (t < 0.5f)
                return Color.Lerp(Color.red, Color.yellow, t * 2f);

            return Color.Lerp(Color.yellow, Color.green, (t - 0.5f) * 2f);
        }
        
        void LoadBall()
        {
            _currentBall = Instantiate(config.ball, _shootPosition, Quaternion.identity);
            _currentRb = _currentBall.GetComponent<Rigidbody>();

            _currentRb.isKinematic = true;

            _state = ShooterState.Idle;
        }
    }
}