using UnityEngine;
using UnityEngine.AI;

public class CharacterFormationService {
    private const float SLOT_DISTANCE = 1.5f;
    private const float SLOT_STOP_DISTANCE = 0.35f;
    private const float SAMPLE_DISTANCE = 2f;
    private const float GOLDEN_ANGLE = 137.5f;

    private readonly CharacterAnchorService _anchorService;

    public CharacterFormationService(CharacterAnchorService anchorService) {
        _anchorService = anchorService;
    }

    public bool MoveToSlot(CharacterBrain brain,
                           CharacterBrain anchor) {
        Vector3 slot = GetSlot(brain, anchor);

        if (NavMesh.SamplePosition(
                slot, out NavMeshHit hit,
                SAMPLE_DISTANCE,
                NavMesh.AllAreas)) {
            slot = hit.position;
        }

        Vector3 difference =
            brain.View.transform.position - slot;

        difference.y = 0f;

        if (difference.sqrMagnitude <= SLOT_STOP_DISTANCE *
                                       SLOT_STOP_DISTANCE) {

            brain.MovementComponent.Stop();
            return false;
        }

        brain.MovementComponent.MoveToPosition(slot);
        return true;
    }

    public float GetDistance(CharacterBrain first,
                             CharacterBrain second) {

        Vector3 difference = first.View.transform.position -
                             second.View.transform.position;

        difference.y = 0f;

        return difference.magnitude;
    }

    private Vector3 GetSlot(CharacterBrain brain,
                            CharacterBrain anchor) {

        int index = _anchorService.GetMemberIndex(brain, anchor);
        int ring = Mathf.Max(0, (index - 1) / 6);
        float radius = SLOT_DISTANCE * (ring + 1);
        float angle = index * GOLDEN_ANGLE;

        Vector3 direction = Quaternion.Euler(0f, angle, 0f) *
                            Vector3.forward;

        return anchor.View.transform.position +
               direction * radius;
    }
}