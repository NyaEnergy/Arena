using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Zenject;

public sealed class CommanderSkillHudController : IInitializable,
                                                  ITickable,
                                                  IDisposable {
    private const float MINIMUM_TARGET_RADIUS = 0.75f;

    private readonly CommanderSkillService _skillService;
    private readonly TerritoryDropService _dropService;
    private readonly GameInputService _inputService;
    private readonly CommanderSkillHudView _view;
    private readonly CommanderSkillTargetView _targetView;

    private CommanderProgressionRuntime _allySkill;
    private CommanderProgressionRuntime _enemySkill;
    private CommanderProgressionRuntime _targetingSkill;

    private UnityAction _allyAction;
    private UnityAction _enemyAction;

    public CommanderSkillHudController(
        CommanderSkillService skillService,
        TerritoryDropService dropService,
        GameInputService inputService,
        CommanderSkillHudView view,
        CommanderSkillTargetView targetView) {
        _skillService = skillService;
        _dropService = dropService;
        _inputService = inputService;
        _view = view;
        _targetView = targetView;
    }

    public void Initialize() {
        _skillService.TryGetSkill(TeamType.Ally,
                                  out _allySkill);
        _skillService.TryGetSkill(TeamType.Enemy,
                                  out _enemySkill);

        _allyAction = () => OnSkillClicked(_allySkill);
        _enemyAction = () => OnSkillClicked(_enemySkill);

        AddListener(_view?.AllySkillButton, _allyAction);
        AddListener(_view?.EnemySkillButton, _enemyAction);

        CancelTargeting();
        Refresh();
    }

    public void Tick() {
        HandleTargeting();
        Refresh();
    }

    public void Dispose() {
        RemoveListener(_view?.AllySkillButton, _allyAction);
        RemoveListener(_view?.EnemySkillButton, _enemyAction);

        _allyAction = null;
        _enemyAction = null;

        CancelTargeting();
    }

    private void OnSkillClicked(CommanderProgressionRuntime skill) {
        if (!_skillService.IsReady(skill)) return;

        if (!_skillService.RequiresTarget(skill)) {
            CancelTargeting();

            _skillService.TryActivate(
                skill.Commander.TeamType,
                null,
                Vector3.zero);

            return;
        }

        if (_targetingSkill == skill) {
            CancelTargeting();
            return;
        }

        _targetingSkill = skill;
        _view?.SetTargeting(skill);
    }

    private void HandleTargeting() {
        if (_targetingSkill == null) return;

        if (!_skillService.IsReady(_targetingSkill)) {
            CancelTargeting();
            return;
        }

        bool hasPosition = _dropService.TryGet(
            _inputService.PointerPosition,
            out TerritoryRuntime territory,
            out Vector3 position);

        bool isValid = hasPosition &&
            _skillService.CanActivate(
                _targetingSkill,
                territory,
                position);

        if (hasPosition) {
            _targetView?.Show(
                position,
                Mathf.Max(MINIMUM_TARGET_RADIUS,
                          _targetingSkill.Node.SkillEffectRadius),
                isValid);
        } else {
            _targetView?.Hide();
        }

        if (!_inputService.IsPointerPressedThisFrame ||
            IsPointerOverUi() ||
            !isValid) {
            return;
        }

        if (_skillService.TryActivate(
                _targetingSkill.Commander.TeamType,
                territory,
                position)) {
            CancelTargeting();
        }
    }

    private void Refresh() {
        Render(_view?.AllySkillButton, _allySkill);
        Render(_view?.EnemySkillButton, _enemySkill);
        _view?.SetTargeting(_targetingSkill);
    }

    private void Render(CommanderSkillButtonView button,
                        CommanderProgressionRuntime skill) {
        button?.Render(
            skill,
            _skillService.IsReady(skill),
            _skillService.GetCooldownRemaining(skill),
            _targetingSkill == skill);
    }

    private void CancelTargeting() {
        _targetingSkill = null;
        _targetView?.Hide();
        _view?.SetTargeting(null);
    }

    private static bool IsPointerOverUi() {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }

    private static void AddListener(CommanderSkillButtonView view,
                                    UnityAction action) {
        if (view?.Button != null && action != null) {
            view.Button.onClick.AddListener(action);
        }
    }

    private static void RemoveListener(CommanderSkillButtonView view,
                                       UnityAction action) {
        if (view?.Button != null && action != null) {
            view.Button.onClick.RemoveListener(action);
        }
    }
}
