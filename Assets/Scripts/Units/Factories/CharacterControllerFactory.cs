using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerFactory {
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly DetectionService _detectionService;
    private readonly UtilityAIService _utilityAIService;
    private readonly List<ICharacterAIBehaviorFactory> _behaviorFactories;

    private readonly Camera _camera;

    public CharacterControllerFactory(BattlefieldRegistry battlefieldRegistry,
                                      DetectionService detectionService,
                                      UtilityAIService utilityAIService,
                                      List<ICharacterAIBehaviorFactory> behaviorFactories,
                                      Camera camera) {
        _battlefieldRegistry = battlefieldRegistry;
        _detectionService = detectionService;
        _utilityAIService = utilityAIService;
        _behaviorFactories = behaviorFactories;
        _camera = camera;
    }

    public CharacterController Create(CharacterView view,
                                      CharacterKey characterKey,
                                      ICharacterConfig config,
                                      Action<CharacterView> returnToPool) {
        if (view == null ||
            config == null ||
            view.CharacterType != characterKey.CharacterType ||
            config.CharacterType != characterKey.CharacterType) {

            return null;
        }

        CharacterBrain brain =
            new(view, config, characterKey.TeamType);

        ICharacterAIBehavior behavior = CreateBehavior(brain);

        CharacterAIController aiController =
            new(brain, _detectionService,
                _utilityAIService, behavior);

        HealthBarRuntime healthBarRuntime =
            CreateHealthBarRuntime(brain, view);

        return new CharacterController(view,
                                       characterKey,
                                       brain,
                                       aiController,
                                       _battlefieldRegistry,
                                       healthBarRuntime,
                                       returnToPool);
    }

    private ICharacterAIBehavior CreateBehavior(
        CharacterBrain brain) {

        for (int i = 0; i < _behaviorFactories.Count; i++) {
            ICharacterAIBehaviorFactory factory =
                _behaviorFactories[i];

            if (factory == null || !factory.CanCreate(brain)) {
                continue;
            }

            return factory.Create(brain);
        }

        return null;
    }

    private HealthBarRuntime CreateHealthBarRuntime(CharacterBrain brain,
                                                    CharacterView view) {
        HealthBarView healthBarView = view.HealthBarView;

        if (healthBarView == null) return null;

        Transform cameraTransform = _camera == null ?
                                    null : _camera.transform;

        return new HealthBarRuntime(brain,
                                             healthBarView,
                                             cameraTransform);
    }
}