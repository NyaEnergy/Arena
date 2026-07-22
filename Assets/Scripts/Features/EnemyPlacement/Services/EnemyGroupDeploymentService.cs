using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyGroupDeploymentService {
    private readonly CharacterDeploymentService _deploymentService;
    private readonly EnemyGroupFormationService _formationService;
    private readonly CommanderQuestService _questService;

    private readonly List<Vector3> _positions = new();

    public EnemyGroupDeploymentService(
                CharacterDeploymentService deploymentService,
                EnemyGroupFormationService formationService,
                CommanderQuestService questService) {

        _deploymentService = deploymentService;
        _formationService = formationService;
        _questService = questService;
    }

    public bool TryDeploy(EnemyQueueItem item,
                          TerritoryRuntime territory,
                          Vector3 center) {
        if (item == null ||
            !item.IsValid ||
            !_formationService.TryCreate(
                territory,
                center,
                item.Count,
                item.FormationSpacing,
                _positions)) {
            return false;
        }

        CharacterDeploymentRequest request = item.CreateRequest();
        int deployedCount = 0;

        for (int i = 0; i < _positions.Count; i++) {
            CharacterView view =
                _deploymentService.Deploy(
                    request,
                    _positions[i]);

            if (view == null) break;

            deployedCount++;
        }

        if (deployedCount > 0 &&
            deployedCount < item.Count) {
            Debug.LogWarning(
                $"[EnemyGroup] Deployed {deployedCount}/" +
                $"{item.Count}. Check character pool capacity.");
        }

        if (deployedCount > 0) {
            _questService.Report(new CommanderQuestEvent(
                CommanderQuestEventType.EnemyGroupDeployed,
                TeamType.Enemy,
                item.CharacterType));
        }

        return deployedCount > 0;
    }
}