using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Director/Enemy Director Config",
                 fileName = "EnemyDirectorConfig")]
public class EnemyDirectorConfig : ScriptableObject {
    [SerializeField] private EnemyDirectorProfile _calm = new();
    [SerializeField] private EnemyDirectorProfile _pressure = new();

    [SerializeField, Min(0.1f)] private float _evaluationInterval = 0.5f;
    [SerializeField] private bool _logStateChanges;

    public float EvaluationInterval => Mathf.Max(0.1f, _evaluationInterval);
    public bool LogStateChanges => _logStateChanges;

    public EnemyDirectorProfile GetProfile(EnemyDirectorState state) {
        return state == EnemyDirectorState.Pressure ?
                        _pressure : _calm;
    }
}
