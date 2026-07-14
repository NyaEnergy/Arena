public class DemoBattleCallSpawnService {
    private readonly CharacterFactory _characterFactory;
    private readonly SummonerFactory _summonerFactory;
    private readonly DemoBattleCallPositionService _positionService;

    public DemoBattleCallSpawnService(CharacterFactory characterFactory,
                                      SummonerFactory summonerFactory,
                                      DemoBattleCallPositionService positionService) {

        _characterFactory = characterFactory;
        _summonerFactory = summonerFactory;
        _positionService = positionService;
    }

    public void Spawn(DemoBattleCallEntry entry) {
        if (entry == null ||
            !entry.IsValid)
                return;

        if (!_positionService.TryGetPosition(
                entry.TeamType, out UnityEngine.Vector3 position))
                    return;

        CharacterView view = Create(entry, position);

        if (view == null) return;

        view.transform.rotation =
            _positionService.GetRotation(
                entry.TeamType);
    }

    private CharacterView Create(DemoBattleCallEntry entry,
                                 UnityEngine.Vector3 position) {
        
        if (entry.CallType == DemoBattleCallType.Summoner) {
            return _summonerFactory.Spawn(
                entry.TeamType,
                entry.SummonerConfig,
                position);
        }

        CharacterKey key = new(
                entry.TeamType,
                entry.CharacterType
        );

        return _characterFactory.Spawn(key, position);
    }
}