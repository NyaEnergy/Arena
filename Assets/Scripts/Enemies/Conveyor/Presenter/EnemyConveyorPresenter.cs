using Zenject;

public class EnemyConveyorPresenter : ITickable {
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorView _view;

    public EnemyConveyorPresenter(EnemyConveyorRuntime runtime,
                                  EnemyConveyorView view) {
        _runtime = runtime;
        _view = view;
    }

    public void Tick() {
        _view.Render(_runtime.Queue);
    }
}
