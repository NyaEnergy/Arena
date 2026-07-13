using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLifecycleFactory {
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly DetectionService _detectionService;
    private readonly UtilityAIService _utilityAIService;
    private readonly CharacterPresenceTransitionService _transitionService;
    private readonly CharacterCombatPresenceService _combatPresenceService;
    private readonly CharacterDeathPresentationService _deathService;
    private readonly HealthBarPaletteConfig _healthBarPalette;
    private readonly List<ICharacterBehaviorFactory> _behaviorFactories;
    private readonly Camera _camera;

    public CharacterLifecycleFactory(
                BattlefieldRegistry battlefieldRegistry,
                DetectionService detectionService,
                UtilityAIService utilityAIService,
                CharacterPresenceTransitionService transitionService,
                CharacterCombatPresenceService combatPresenceService,
                CharacterDeathPresentationService deathService,
                HealthBarPaletteConfig healthBarPalette,
                List<ICharacterBehaviorFactory> behaviorFactories,
                Camera camera) {

        _battlefieldRegistry = battlefieldRegistry;
        _detectionService = detectionService;
        _utilityAIService = utilityAIService;
        _transitionService = transitionService;
        _combatPresenceService = combatPresenceService;
        _deathService = deathService;
        _healthBarPalette = healthBarPalette;
        _behaviorFactories = behaviorFactories;
        _camera = camera;
    }

    public CharacterLifecycleController Create(
                CharacterView view,
                TeamType teamType,
                ICharacterRuntimeConfig config,
                Action<CharacterView> deathHandler,
                Action<CharacterView> returnToPool) {

        if (view == null ||
            config == null ||
            returnToPool == null) {
            return null;
        }

        CharacterBrain brain = new(view, config, teamType);

        CharacterBehaviorController behaviorController =
            new(brain,
                _detectionService,
                _utilityAIService,
                CreateBehavior(brain));

        CharacterBattlefieldPresenceController presenceController =
            new(view,
                brain,
                _battlefieldRegistry,
                _transitionService,
                _combatPresenceService);

        return new CharacterLifecycleController(
            view, brain,
            behaviorController,
            CreateHealthBarRuntime(brain, view),
            presenceController,
            _deathService,
            deathHandler,
            returnToPool);
    }

    private ICharacterBehavior CreateBehavior(CharacterBrain brain) {
        
        for (int i = 0; i < _behaviorFactories.Count; i++) {

            ICharacterBehaviorFactory factory =
                _behaviorFactories[i];

            if (factory != null &&
                factory.CanCreate(brain))
                    return factory.Create(brain);
        }

        return null;
    }

    private HealthBarRuntime CreateHealthBarRuntime(CharacterBrain brain,
                                                    CharacterView view) {

        if (view.HealthBarView == null) return null;

        Transform cameraTransform =
            _camera != null ?
            _camera.transform : null;

        return new HealthBarRuntime(brain,
                                    view.HealthBarView,
                                    cameraTransform,
                                    _healthBarPalette);
    }
}