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
           !entry.IsValid) return;

        CharacterView view = Create(entry);

        if (view == null) return;

        view.transform.rotation =
            _positionService.GetRotation(
                entry.TeamType);
    }

    private CharacterView Create(DemoBattleCallEntry entry) {
        var position = _positionService.GetPosition(
                entry.TeamType);

        if (entry.CallType == DemoBattleCallType.Summoner) {
            return _summonerFactory.Spawn(
                entry.TeamType,
                entry.SummonerConfig,
                position);
        }

        CharacterKey key = new(
                entry.TeamType,
                entry.CharacterType);

        return _characterFactory.Spawn(
            key, position);
    }
}