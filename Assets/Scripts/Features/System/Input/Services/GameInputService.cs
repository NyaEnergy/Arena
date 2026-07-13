using UnityEngine;

public class GameInputService {
    private readonly GameInput _gameInput;

    public Vector2 PointerPosition => _gameInput.Gameplay.Point.ReadValue<Vector2>();
    public bool IsPointerPressed => _gameInput.Gameplay.Click.IsPressed();
    public bool IsPointerPressedThisFrame => _gameInput.Gameplay.Click.WasPressedThisFrame();
    public bool IsPointerReleasedThisFrame => _gameInput.Gameplay.Click.WasReleasedThisFrame();

    public GameInputService() {
        _gameInput = new GameInput();
        _gameInput.Enable();
    }
}
