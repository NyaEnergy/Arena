public class CharacterGroupRuntime {
    public bool IsReturning { get; private set; }

    public void StartReturn() {
        IsReturning = true;
    }

    public void CompleteReturn() {
        IsReturning = false;
    }

    public void Reset() {
        IsReturning = false;
    }
}