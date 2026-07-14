using System.Collections.Generic;
using UnityEngine;

public class DemoBattleCombatCenterService {
    private const float ALLY_RADIUS = 8f;
    private const float ENGAGEMENT_RADIUS = 11f;
    private const float ENEMY_GROUP_RADIUS = 8f;
    private const float MAX_CAMERA_RADIUS = 9f;

    private readonly BattlefieldRegistry _registry;
    private readonly CharacterTeamService _teamService;
    private readonly CharacterAnchorService _anchorService;

    private readonly List<Vector3> _positions = new();

    public DemoBattleCombatCenterService(
                BattlefieldRegistry registry,
                CharacterTeamService teamService,
                CharacterAnchorService anchorService) {

        _registry = registry;
        _teamService = teamService;
        _anchorService = anchorService;
    }

    public bool TryGetCenter(out Vector3 center) {
        return TryGetCenterAndRadius(out center, out _);
    }

    public bool TryGetCenterAndRadius(out Vector3 center,
                                      out float radius) {
        _positions.Clear();

        if (_anchorService.TryGet(TeamType.Ally,
                out CharacterBrain allyAnchor)) {

            Vector3 anchor =
                allyAnchor.View.transform.position;

            AddNearby(_registry.GetAllies(TeamType.Ally),
                anchor, ALLY_RADIUS);

            AddNearby(_registry.GetEnemies(TeamType.Ally),
                anchor, ENGAGEMENT_RADIUS);

        } else if (_anchorService.TryGet(TeamType.Enemy,
                       out CharacterBrain enemyAnchor)) {
            AddNearby(_registry.GetAllies(TeamType.Enemy),
                      enemyAnchor.View.transform.position,
                      ENEMY_GROUP_RADIUS);
        }

        if (_positions.Count == 0) {
            center = Vector3.zero;
            radius = 0f;
            return false;
        }

        center = CalculateCenter();

        radius = Mathf.Min(CalculateRadius(center),
                           MAX_CAMERA_RADIUS);

        return true;
    }

    private void AddNearby(IReadOnlyList<CharacterBrain> characters,
                           Vector3 anchor,
                           float maximumDistance) {

        float maximumSqrDistance = maximumDistance * maximumDistance;

        for (int i = 0; i < characters.Count; i++) {
            CharacterBrain character = characters[i];

            if (!_teamService.IsAlive(character))
                continue;

            Vector3 position = character.View.transform.position;

            Vector3 difference = position - anchor;
            difference.y = 0f;

            if (difference.sqrMagnitude <= maximumSqrDistance) {
                _positions.Add(position);
            }
        }
    }

    private Vector3 CalculateCenter() {
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < _positions.Count; i++) {
            sum += _positions[i];
        }

        return sum / _positions.Count;
    }

    private float CalculateRadius(Vector3 center) {
        float maximumSqrDistance = 0f;

        for (int i = 0; i < _positions.Count; i++) {
            Vector3 difference = _positions[i] - center;

            difference.y = 0f;

            maximumSqrDistance =
                Mathf.Max(maximumSqrDistance,
                          difference.sqrMagnitude);
        }

        return Mathf.Sqrt(maximumSqrDistance);
    }
}