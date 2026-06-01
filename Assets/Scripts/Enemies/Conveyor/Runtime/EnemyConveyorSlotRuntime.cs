public class EnemyConveyorSlotRuntime {
    private readonly EnemyPlatformView _platform;
    private readonly BattlefieldCharacter _enemy;

    public EnemyPlatformView Platform => _platform;
    public BattlefieldCharacter Enemy => _enemy;

    public EnemyConveyorSlotRuntime(EnemyPlatformView platform,
                                    BattlefieldCharacter enemy) {
        _platform = platform;
        _enemy = enemy;
    }
}
