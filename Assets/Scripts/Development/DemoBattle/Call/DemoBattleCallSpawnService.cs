public class DemoBattleCallSpawnService {
    private readonly CharacterDeploymentService _deploymentService;

    public DemoBattleCallSpawnService(CharacterDeploymentService deploymentService) {
        _deploymentService = deploymentService;
    }

    public void Spawn(
        DemoBattleCallEntry entry) {
        if (entry == null ||
            !entry.IsValid) {
            return;
        }

        CharacterDeploymentRequest request = CreateRequest(entry);

        _deploymentService.Deploy(request);
    }

    private CharacterDeploymentRequest CreateRequest(DemoBattleCallEntry entry) {
        if (entry.CallType == DemoBattleCallType.Summoner) {
            return CharacterDeploymentRequest.ForSummoner(
                entry.TeamType, entry.SummonerConfig);
        }

        return CharacterDeploymentRequest.ForCharacter(
            entry.TeamType, entry.CharacterType);
    }
}