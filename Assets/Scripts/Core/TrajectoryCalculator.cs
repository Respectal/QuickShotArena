using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TrajectoryCalculator
    {
        public List<Vector3> CalculateTrajectory(Vector3 startPos, Vector3 startVel, int points, float timeStep)
        {
            var result = new List<Vector3>();
            
            for (var i = 0; i < points; i++)
            {
                var time = i * timeStep;
                
                var position = startPos + startVel * time;
                
                result.Add(position);
            }
            
            return result;
        } 
    }
}
