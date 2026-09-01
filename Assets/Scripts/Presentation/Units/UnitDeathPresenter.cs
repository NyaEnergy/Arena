namespace ConveyorWars.Presentation.Units {
    public sealed class UnitDeathPresenter {
        private readonly UnitView _view;

        private bool _isApplied;

        public UnitDeathPresenter(UnitView view) {
            _view = view;
        }

        public void Apply() {
            if (_isApplied) return;

            _isApplied = true;
            _view.gameObject.SetActive(false);
        }
    }
}