using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyConveyorSlot {
    private EnemyPlatformView _platform;
    private BattlefieldCharacter _character;

    private Vector3 _worldPosition;

    public EnemyPlatformView Platform => _platform;
    public BattlefieldCharacter Character => _character;

    public bool HasCharacter => _character != null;
    public bool IsEmpty => _platform == null;

    public Vector3 WorldPosition => _worldPosition;

    public EnemyConveyorSlot(Vector3 worldPosition) {
        _worldPosition = worldPosition;
    }

    public void AttachPlatform(EnemyPlatformView platform) {
        _platform = platform;
        _platform.transform.position = _worldPosition;
    }

    public void AttachCharacter(BattlefieldCharacter character) {
        _character = character;

        if (_platform != null && character != null) {
            character.transform.position = _platform.EnemyAnchor.position;
        }
    }

    public BattlefieldCharacter DetachCharacter() {
        BattlefieldCharacter temp = _character;
        _character = null;
        return temp;
    }

    public void SetPosition(Vector3 position) {
        _worldPosition = position;
        
        if(_platform != null) {
            _platform.transform.position = position;
        }

        if(_character != null) {
            _character.transform.position =
                _platform != null ?
                _platform.EnemyAnchor.position :
                position;
        }
    }
}
