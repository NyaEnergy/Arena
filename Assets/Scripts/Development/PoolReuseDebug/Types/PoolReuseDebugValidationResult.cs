public struct PoolReuseDebugValidationResult {
    public bool IsValid { get; }
    public string Message { get; }

    public PoolReuseDebugValidationResult(bool isValid, string message) {
        IsValid = isValid;
        Message = message;
    }
}
