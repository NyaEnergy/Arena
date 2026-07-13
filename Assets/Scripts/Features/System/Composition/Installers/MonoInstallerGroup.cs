using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonoInstallerGroup : MonoInstaller {
    [SerializeField] private List<MonoInstaller> _installers = new();
    public override void InstallBindings() {
        HashSet<MonoInstaller> installedInstaller = new();

        for(int i = 0; i < _installers.Count; ++i) {
            MonoInstaller installer = _installers[i];

            if(installer == null || installer == this) continue;

            Container.Inject(installer);
            installer.InstallBindings();
        }
    }
}
