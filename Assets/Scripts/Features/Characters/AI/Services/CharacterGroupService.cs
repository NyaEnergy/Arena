public class CharacterGroupService {
    private const float MAX_ALLY_DISTANCE = 7f;
    private const float RETURN_DISTANCE = 4.5f;

    private readonly CharacterTeamService _teamService;
    private readonly CharacterAnchorService _anchorService;
    private readonly CharacterFormationService _formationService;

    public CharacterGroupService(CharacterTeamService teamService,
                                 CharacterAnchorService anchorService,
                                 CharacterFormationService formationService) {
        _teamService = teamService;
        _anchorService = anchorService;
        _formationService = formationService;
    }

    public bool Tick(CharacterBrain brain,
        CharacterGroupRuntime runtime) {
        if (!CanMove(brain)) {
            runtime.Reset();
            return false;
        }

        TeamType teamType = brain.Runtime.TeamType;
        bool hasOpponents = _teamService.HasLivingOpponents(teamType);

        if (teamType == TeamType.Enemy &&
            hasOpponents) {
                runtime.Reset();
                return false;
        }

        if (!_anchorService.TryGet(teamType,
                out CharacterBrain anchor)) {

            runtime.Reset();
            return false;
        }

        if (!hasOpponents) {
            return HoldGroup(brain, anchor, runtime);
        }

        return KeepAllyGroup(brain, anchor, runtime);
    }

    private bool HoldGroup(CharacterBrain brain,
                           CharacterBrain anchor,
                           CharacterGroupRuntime runtime) {
        runtime.Reset();
        brain.TargetComponent.ClearTarget();

        if (brain == anchor) {
            brain.MovementComponent.Stop();
            return false;
        }

        return _formationService.MoveToSlot(brain, anchor);
    }

    private bool KeepAllyGroup(CharacterBrain brain,
                               CharacterBrain anchor,
                               CharacterGroupRuntime runtime) {
        if (brain == anchor) {
            runtime.Reset();
            return false;
        }

        float distance = _formationService.GetDistance(brain, anchor);

        if (distance >= MAX_ALLY_DISTANCE)
            runtime.StartReturn();

        if (!runtime.IsReturning)
            return false;

        if (distance <= RETURN_DISTANCE) {
            runtime.CompleteReturn();
            return false;
        }

        brain.TargetComponent.ClearTarget();

        bool isMoving = _formationService.MoveToSlot(brain, anchor);

        if (!isMoving) runtime.CompleteReturn();

        return isMoving;
    }

    private bool CanMove(CharacterBrain brain) {
        return brain != null &&
               brain.View != null &&
               brain.View.Agent != null &&
               brain.Config.MoveSpeed > 0.01f;
    }
}