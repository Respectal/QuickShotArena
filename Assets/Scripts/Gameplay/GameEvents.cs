using System;
using Core;
using Core.Enums;
using UnityEngine;

namespace Gameplay
{
    public class GameEvents : MonoBehaviour
    {
        public static event Action<GameObject, HoleTile, HoleType, Vector3> BallScored;
        public static event Action<HoleTile, Vector3, float> TimeChanged;
        public static event Action GameOver;
        
        public static void RaiseBallScored(GameObject ball, HoleTile holeTile, HoleType holeType, Vector3 holePosition)
        {
            BallScored?.Invoke(ball, holeTile, holeType, holePosition);
        }

        public static void RaiseTimeChanged(HoleTile holeTile, Vector3 popupPosition, float delta)
        {
            TimeChanged?.Invoke(holeTile, popupPosition, delta);
        }

        public static void RaiseGameOver()
        {
            GameOver?.Invoke();
        }
    }
}