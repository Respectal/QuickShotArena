using Core.Enums;
using Gameplay;
using UnityEngine;

namespace Core
{
    public class HoleScoreTrigger : MonoBehaviour
    {
        private HoleTile _holeTile;

        private void Awake()
        {
            _holeTile = GetComponentInParent<HoleTile>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Ball"))
            {
                return;
            }
            
            Destroy(other.gameObject, 2f);

            var scoredType = _holeTile.CurrentType;
            var holePosition = _holeTile.GetComponentInChildren<TimeTextPopup>().transform.position;

            switch (scoredType)
            {
                case HoleType.Good:
                    SoundManager.Instance.Play(SoundManager.Instance.scoreClip);
                    break;
                case HoleType.Bad:
                    SoundManager.Instance.Play(SoundManager.Instance.penaltyClip);
                    break;
            }

            GameEvents.RaiseBallScored(other.gameObject, _holeTile, scoredType, holePosition);
        }
    }
}