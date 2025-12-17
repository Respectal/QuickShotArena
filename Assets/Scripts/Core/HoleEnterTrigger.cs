using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class HoleEnterTrigger : MonoBehaviour
    {
        [SerializeField] private float affectRadius = 0.7f;

        private readonly Dictionary<Rigidbody, Collider[]> _ignored = new();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Rigidbody>(out var rb))
                return;

            var colliders = Physics.OverlapSphere(
                rb.position,
                affectRadius,
                LayerMask.GetMask("Floor")
            );

            foreach (var col in colliders)
                Physics.IgnoreCollision(col, other, true);

            _ignored[rb] = colliders;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Rigidbody>(out var rb))
            {
                return;
            }

            if (!_ignored.TryGetValue(rb, out var colliders))
            {
                return;
            }

            foreach (var col in colliders)
            {
                Physics.IgnoreCollision(col, other, false);
            }

            _ignored.Remove(rb);
        }
    
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, 2f, transform.position.z), affectRadius);
        }
    }
}