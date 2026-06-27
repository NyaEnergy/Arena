public class PoolReuseDebugPresenter {
    private readonly PoolReuseDebugView _view;
    private readonly PoolReuseDebugSettings _settings;
    private readonly PoolReuseDebugCharacterService _characterService;
    private readonly PoolReuseDebugTracker _tracker;

    public PoolReuseDebugPresenter(PoolReuseDebugView view,
                                   PoolReuseDebugSettings settings,
                                   PoolReuseDebugCharacterService characterService,
                                   PoolReuseDebugTracker tracker) {
        _view = view;
        _settings = settings;
        _characterService = characterService;
        _tracker = tracker;
    }

    public void ShowWaitingForCharacters() {
        _view.SetText("CORE STABILITY\n\n" +
                      "Ожидание персонажей\n\n" +
                     $"Героев на поле: {_characterService.ActiveAllyCount}\n" +
                     $"Противников на поле: {_characterService.ActiveEnemyCount}");
    }

    public void ShowWaitingBeforeKill(
        int completedRespawns) {
        ShowRunning("Подготовка уничтожения противника",
                    completedRespawns);
    }

    public void ShowWaitingForDespawn(
        int completedRespawns) {
        ShowRunning("Ожидание возврата противника в Pool",
                    completedRespawns);
    }

    public void ShowWaitingBeforeRespawn(int completedRespawns) {
        ShowRunning("Подготовка повторного появления",
                    completedRespawns);
    }

    public void ShowCompleted(int completedRespawns) {
        _view.SetText("CORE STABILITY\n\n" +
                      "Результат: ПРОЙДЕНО\n\n" +
                     $"Повторных появлений: {completedRespawns} / {_settings.RespawnCount}\n" +
                     $"Уникальных объектов: {_tracker.UniqueCharacterCount}\n" +
                      "Повторное использование: ДА\n\n" +
                      "Здоровье восстановлено\n" +
                      "Состояние уничтожения сброшено\n" +
                      "Старая цель очищена\n" +
                      "Registry очищается корректно");
    }

    public void ShowFailed(string message,
                           int completedRespawns) {
        _view.SetText("CORE STABILITY\n\n" +
                      "Результат: НЕ ПРОЙДЕНО\n\n" +
                     $"{message}\n\n" +
                     $"Выполнено циклов: {completedRespawns} / {_settings.RespawnCount}\n" +
                     $"Героев на поле: {_characterService.ActiveAllyCount}\n" +
                     $"Противников на поле: {_characterService.ActiveEnemyCount}");
    }

    private void ShowRunning(string operation,
                             int completedRespawns) {
        string reuseState =
            _tracker.HasReusedCharacter ?
            "ДА" : "ПОКА НЕТ";

        _view.SetText("CORE STABILITY\n\n" +
                      "Результат: ВЫПОЛНЯЕТСЯ\n\n" +
                     $"{operation}\n\n" +
                     $"Повторных появлений: {completedRespawns} / {_settings.RespawnCount}\n" +
                     $"Героев на поле: {_characterService.ActiveAllyCount}\n" +
                     $"Противников на поле: {_characterService.ActiveEnemyCount}\n" +
                     $"Уникальных объектов: {_tracker.UniqueCharacterCount}\n" +
                     $"Повторное использование: {reuseState}");
    }
}