public struct DemoBattleAutoRespawnRequest {
    public TeamType TeamType;
    public float RemainingTime;

    public DemoBattleAutoRespawnRequest(TeamType teamType,
                                        float remainingTime) {
        TeamType = teamType;
        RemainingTime = remainingTime;
    }
}