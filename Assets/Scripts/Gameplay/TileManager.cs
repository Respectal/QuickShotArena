using System.Collections.Generic;
using Core;
using Core.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class TileManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] public TileManagerConfig config;

        private readonly List<Vector2> _holePositions = new();
        private InputAction _reloadAction;

        void Start()
        {
            var tileSize = GetTileSize();
            var startX = tileSize.x * config.width * 0.5f;
            transform.position = new Vector3(startX, 2, 0);
            if (Camera.main != null)
            {
                Camera.main.transform.position = new Vector3(startX, 17, -2);
            }

            GenerateMesh();
        }
        
        private void OnEnable()
        {
            GameEvents.BallScored += OnBallScored;
        }

        private void OnDisable()
        {
            GameEvents.BallScored -= OnBallScored;
        }
        
        private void OnBallScored(GameObject ball, HoleTile hole, HoleType holeType, Vector3 holePosition)
        {
            RelocateHole(hole.transform);
            hole.FullHoleReset();
        }

        void GenerateMesh()
        {
            // Get the tile dimensions from the TileSolid's bounds
            var tileSize = GetTileSize();

            for (var i = 0; i < config.width; i++)
            {
                for (var k = 0; k < config.height; k++)
                {
                    var pos = new Vector3(i * tileSize.x, 2, k * tileSize.z);
                    Instantiate(config.tileSolid, pos, Quaternion.identity, transform);
                }
            }

            GenerateHoles(tileSize);
            GenerateWalls(tileSize);
        }

        void GenerateHoles(Vector3 tileSize)
        {
            var attempts = 0;
            var maxAttempts = config.holeCount * 20;

            while (_holePositions.Count < config.holeCount && attempts < maxAttempts)
            {
                attempts++;

                var newPos = GenerateRandomHolePosition(tileSize);

                // Check collision with existing holes
                if (IsValidHolePosition(newPos))
                {
                    _holePositions.Add(newPos);
                    var pos = new Vector3(newPos.x, 0.0001f, newPos.y);
                    Instantiate(config.tileHole, pos, Quaternion.identity, transform);
                }
            }

            if (_holePositions.Count < config.holeCount)
            {
                Debug.LogWarning($"Created {_holePositions.Count} out of {config.holeCount} holes");
            }
        }

        private Vector2 GenerateRandomHolePosition(Vector3 tileSize)
        {
            // Random float position in grid range (reserve first row for ball)
            var x = Random.Range(config.holeRadius, config.width * tileSize.x - tileSize.x);
            var z = Random.Range(config.holeRadius + tileSize.z, config.height * tileSize.z - tileSize.z);
            return new Vector2(x, z);
        }

        void RelocateHole(Transform hole)
        {
            var tileSize = GetTileSize();

            var attempts = 0;
            var maxAttempts = 20;

            while (attempts < maxAttempts)
            {
                attempts++;

                var newPos = GenerateRandomHolePosition(tileSize);

                if (IsValidHolePosition(newPos))
                {
                    UpdateHolePosition(hole, newPos);
                    return;
                }
            }
        }

        void UpdateHolePosition(Transform hole, Vector2 pos)
        {
            // remove old position
            _holePositions.RemoveAll(p =>
                Vector2.Distance(p, new Vector2(hole.position.x, hole.position.z)) < 0.01f);

            // add new
            _holePositions.Add(pos);

            hole.position = new Vector3(pos.x, hole.position.y, pos.y);
        }

        bool IsValidHolePosition(Vector2 newPos)
        {
            foreach (var existingPos in _holePositions)
            {
                var distance = Vector2.Distance(newPos, existingPos);
                if (distance < config.minHoleDistance)
                {
                    return false;
                }
            }
            return true;
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var hole in _holePositions)
            {
                Gizmos.DrawWireSphere(new Vector3(hole.x, 2f, hole.y), config.holeRadius);
            }
        }
    
        void GenerateWalls(Vector3 tileSize)
        {
            var floorWidth = config.width * tileSize.x;
            var floorDepth = config.height * tileSize.z;

            var centerX = (floorWidth - tileSize.x) * 0.5f;
            var centerZ = (floorDepth - tileSize.z) * 0.5f;

            var yPos = 2f + config.wallHeight * 0.5f;

            // Top
            CreateWall(
                new Vector3(centerX, yPos, floorDepth - tileSize.z * 0.5f + config.wallThickness * 0.5f),
                new Vector3(floorWidth, config.wallHeight, config.wallThickness)
            );

            // Bottom
            CreateWall(
                new Vector3(centerX, yPos, -tileSize.z * 0.5f - config.wallThickness * 0.5f),
                new Vector3(floorWidth, config.wallHeight, config.wallThickness)
            );
        
            // Right
            CreateWall(
                new Vector3(floorWidth - tileSize.x * 0.5f + config.wallThickness * 0.5f, yPos, centerZ),
                new Vector3(config.wallThickness, config.wallHeight, floorDepth)
            );
        
            // Left
            CreateWall(
                new Vector3(-tileSize.x * 0.5f - config.wallThickness * 0.5f, yPos, centerZ),
                new Vector3(config.wallThickness, config.wallHeight, floorDepth)
            );
        }
    
        void CreateWall(Vector3 position, Vector3 scale)
        {
            var wall = Instantiate(config.wallPrefab, position, Quaternion.identity, transform);
            wall.transform.localScale = scale;
        }
    }
}