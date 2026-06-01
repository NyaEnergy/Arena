public class EnemyDragRuntime {
    private EnemyConveyorSlot _sourceSlot;
    private EnemyConveyorSlot _hoverSlot;
    private BattlefieldCharacter _draggedEnemy;
    private bool _isDragging;

    public EnemyConveyorSlot SourceSlot => _sourceSlot;
    public EnemyConveyorSlot HoverSlot => _hoverSlot;
    public BattlefieldCharacter DraggedEnemy => _draggedEnemy;
    public bool IsDragging => _isDragging;

    public void BeginDrag(EnemyConveyorSlot sourceSlot,
                          BattlefieldCharacter draggedEnemy) {
        _sourceSlot = sourceSlot;
        _draggedEnemy = draggedEnemy;
        _hoverSlot = null;
        _isDragging = true;
    }

    public void SetHoverSlot(EnemyConveyorSlot hoverSlot) {
        _hoverSlot = hoverSlot;
    }

    public void EndDrag() {
        _sourceSlot = null;
        _hoverSlot = null;
        _draggedEnemy = null;
        _isDragging = false;
    }
}
