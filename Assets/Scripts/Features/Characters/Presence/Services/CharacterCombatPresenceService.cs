public class CharacterCombatPresenceService {
    public void SetEnabled(CharacterView view,
                           bool isEnabled) {
        if (view == null) return;

        if (view.Collider != null) {
            view.Collider.enabled = isEnabled;
        }

        if (view.HealthBarView != null) {
            view.HealthBarView.gameObject.SetActive(isEnabled);
        }
    }
}