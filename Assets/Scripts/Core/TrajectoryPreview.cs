using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryPreview : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
            _lineRenderer.startWidth = 0.3f;
            _lineRenderer.endWidth = 0.3f;
            _lineRenderer.positionCount = 0;
        }

        public void Show(List<Vector3> points, Color color)
        {
            _lineRenderer.enabled = true;

            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());

            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
        
        public void Hide()
        {
            _lineRenderer.enabled = false;
        }
    }
}