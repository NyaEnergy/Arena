using System.Collections.Generic;

public class UtilityAIService {
    private readonly List<UtilityAction> _actions;

    public UtilityAIService() {
        _actions = new List<UtilityAction> {
            new IdleAction(),
            new ChaseAction(),
            new AttackAction(),
            new RetreatAction()
        };
    }

    public AIActionType Evaluate(AIContext context) {
        float bestScore = float.MinValue;

        AIActionType bestAction = AIActionType.Idle;

        for (int i = 0; i < _actions.Count; ++i) {
            float score = _actions[i].CalculateScore(context);
            if (score <= bestScore) continue;
            bestScore = score;
            bestAction = _actions[i].ActionType;
        }

        return bestAction;
    }
}
