using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class CommanderSkillService : IInitializable,
                                            ITickable,
                                            IDisposable {
    private readonly CommanderProgressionService _progressionService;
    private readonly CommanderSkillEffectState _effectState;
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly ControllerSlowService _slowService;
    private readonly EnemyGroupDeploymentService _groupDeploymentService;
    private readonly TerritorySpawnGate _spawnGate;

    private readonly List<CommanderProgressionRuntime> _skills = new();
    private readonly List<EnemyGroupConfig> _matchingGroups = new();

    public event Action<CommanderProgressionRuntime> SkillActivated;

    public IReadOnlyList<CommanderProgressionRuntime> Skills => _skills;

    public CommanderSkillService(
        CommanderProgressionService progressionService,
        CommanderSkillEffectState effectState,
        BattlefieldRegistry battlefieldRegistry,
        ControllerSlowService slowService,
        EnemyGroupDeploymentService groupDeploymentService,
        TerritorySpawnGate spawnGate) {
        _progressionService = progressionService;
        _effectState = effectState;
        _battlefieldRegistry = battlefieldRegistry;
        _slowService = slowService;
        _groupDeploymentService = groupDeploymentService;
        _spawnGate = spawnGate;

        IReadOnlyList<CommanderProgressionRuntime> nodes =
            progressionService.Nodes;

        for (int i = 0; i < nodes.Count; ++i) {
            if (nodes[i].Node.NodeType ==
                CommanderProgressionNodeType.Skill) {
                _skills.Add(nodes[i]);
            }
        }
    }

    public void Initialize() {
        _progressionService.NodeUnlocked += OnNodeUnlocked;

        for (int i = 0; i < _skills.Count; ++i) {
            if (_skills[i].IsUnlocked) LogReady(_skills[i]);
        }
    }

    public void Dispose() {
        _progressionService.NodeUnlocked -= OnNodeUnlocked;
    }

    public bool TryActivate(TeamType teamType,
                            TerritoryRuntime territory,
                            Vector3 position) {
        if (!TryGetUnlockedSkill(
                teamType,
                out CommanderProgressionRuntime skill)) {
            return false;
        }

        bool activated = skill.Node.SkillEffectType switch {
            CommanderSkillEffectType.DamageTakenMultiplier =>
                _effectState.TryActivate(skill, position),

            CommanderSkillEffectType.AreaSlow =>
                IsValidSpatialTarget(territory, position) &&
                _effectState.TryActivate(skill, position),

            CommanderSkillEffectType.VanguardAssault =>
                TryDeploy(skill, territory, position,
                          CharacterType.Vanguard),

            CommanderSkillEffectType.SummonerNetwork =>
                TryDeploy(skill, territory, position,
                          CharacterType.Summoner),

            _ => false
        };

        if (!activated) return false;

        Debug.Log($"[CommanderSkill] Activated: " +
                  $"{skill.Commander.DisplayName} / " +
                  $"{skill.Node.DisplayName}");

        SkillActivated?.Invoke(skill);
        return true;
    }

    public void Tick() {
        _effectState.Tick(Time.deltaTime);
        ApplyAreaSlow();
    }

    private void OnNodeUnlocked(CommanderProgressionRuntime runtime) {
        if (runtime != null &&
            runtime.Node.NodeType ==
            CommanderProgressionNodeType.Skill) {
            LogReady(runtime);
        }
    }

    private static void LogReady(CommanderProgressionRuntime skill) {
        Debug.Log($"[CommanderSkill] Ready: " +
                  $"{skill.Commander.DisplayName} / " +
                  $"{skill.Node.DisplayName}");
    }

    private bool TryGetUnlockedSkill(
        TeamType teamType,
        out CommanderProgressionRuntime skill) {
        for (int i = 0; i < _skills.Count; ++i) {
            CommanderProgressionRuntime current = _skills[i];

            if (current.Commander.TeamType == teamType &&
                current.IsUnlocked) {
                skill = current;
                return true;
            }
        }

        skill = null;
        return false;
    }

    private void ApplyAreaSlow() {
        IReadOnlyList<CommanderSkillEffectRuntime> effects =
            _effectState.ActiveEffects;

        for (int effectIndex = 0;
             effectIndex < effects.Count;
             ++effectIndex) {
            CommanderSkillEffectRuntime effect = effects[effectIndex];

            if (effect.EffectType !=
                CommanderSkillEffectType.AreaSlow) {
                continue;
            }

            IReadOnlyList<CharacterBrain> targets =
                _battlefieldRegistry.GetEnemies(effect.TeamType);

            float sqrRadius = effect.Radius * effect.Radius;

            for (int targetIndex = 0;
                 targetIndex < targets.Count;
                 ++targetIndex) {
                CharacterBrain target = targets[targetIndex];

                if (!IsAvailable(target)) continue;

                Vector3 difference =
                    target.View.transform.position - effect.Position;

                difference.y = 0f;

                if (difference.sqrMagnitude <= sqrRadius) {
                    _slowService.Apply(target, effect.Power);
                }
            }
        }
    }

    private bool TryDeploy(
        CommanderProgressionRuntime skill,
        TerritoryRuntime territory,
        Vector3 position,
        CharacterType characterType) {
        if (!IsValidSpatialTarget(territory, position) ||
            skill.Commander is not EnemyCommanderConfig commander) {
            return false;
        }

        CollectGroups(commander, characterType);

        if (_matchingGroups.Count == 0) return false;

        int requestedCount = skill.Node.SkillDeploymentCount;
        int deployedCount = 0;

        for (int i = 0; i < requestedCount; ++i) {
            EnemyGroupConfig group =
                _matchingGroups[i % _matchingGroups.Count];

            Vector3 deploymentPosition = position +
                GetDeploymentOffset(
                    i,
                    requestedCount,
                    skill.Node.SkillEffectRadius);

            bool deployed = _groupDeploymentService.TryDeploy(
                group.CreateItem(),
                territory,
                deploymentPosition);

            if (!deployed && deploymentPosition != position) {
                deployed = _groupDeploymentService.TryDeploy(
                    group.CreateItem(),
                    territory,
                    position);
            }

            if (deployed) ++deployedCount;
        }

        if (deployedCount > 0 &&
            deployedCount < requestedCount) {
            Debug.LogWarning(
                $"[CommanderSkill] Deployed {deployedCount}/" +
                $"{requestedCount} groups for " +
                $"{skill.Node.DisplayName}.");
        }

        return deployedCount > 0;
    }

    private void CollectGroups(EnemyCommanderConfig commander,
                               CharacterType characterType) {
        _matchingGroups.Clear();

        for (int i = 0; i < commander.Groups.Count; ++i) {
            EnemyGroupConfig group = commander.Groups[i];

            if (group != null &&
                group.IsValid &&
                group.CharacterType == characterType) {
                _matchingGroups.Add(group);
            }
        }
    }

    private bool IsValidSpatialTarget(TerritoryRuntime territory,
                                      Vector3 position) {
        return _spawnGate.CanSpawn(territory) &&
               territory.View.Contains(position);
    }

    private static Vector3 GetDeploymentOffset(int index,
                                               int count,
                                               float radius) {
        if (count <= 1 || radius <= 0f) return Vector3.zero;

        float angle = Mathf.PI * 2f * index / count;

        return new Vector3(Mathf.Cos(angle) * radius,
                           0f,
                           Mathf.Sin(angle) * radius);
    }

    private static bool IsAvailable(CharacterBrain target) {
        return target != null &&
               target.View != null &&
               target.Runtime != null &&
               target.View.gameObject.activeInHierarchy &&
               !target.Runtime.IsDead.CurrentValue;
    }
}
