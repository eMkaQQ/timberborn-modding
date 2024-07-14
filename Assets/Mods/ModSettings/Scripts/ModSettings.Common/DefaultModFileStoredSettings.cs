using System.IO;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class DefaultModFileStoredSettings : FileStoredSettings,
                                              ILoadableSingleton {

    public void Load() {
      var settingsFilePath = Path.Combine(Application.persistentDataPath, "modSettings.json");
      Initialize(new(settingsFilePath));
    }

  }
}