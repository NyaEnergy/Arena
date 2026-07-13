using UnityEngine;

public class MedicPositioningService {
    private const float MINIMUM_SQR_DIRECTION = 0.001f;

    private readonly DetectionService _detectionService;
    private readonly MedicConfig _config;

    public MedicPositioningService(DetectionService detectionService,
                                   MedicConfig config) {
        _detectionService = detectionService;
        _config = config;
    }

    public Vector3 GetSupportPosition(CharacterBrain medic,
                                      CharacterBrain ally) {
        Vector3 allyPosition = ally.View.transform.position;
        CharacterBrain threat = _detectionService.FindClosestTarget(ally);

        Vector3 supportDirection = threat == null ?
            medic.View.transform.position - allyPosition :
            allyPosition - threat.View.transform.position;
        
        supportDirection.y = 0f;

        if(supportDirection.sqrMagnitude < MINIMUM_SQR_DIRECTION) {
            supportDirection = -ally.View.transform.forward;
            supportDirection.y = 0f;
        }

        supportDirection.Normalize();

        return allyPosition +
               supportDirection *
               _config.SupportDistance;
    }
}
