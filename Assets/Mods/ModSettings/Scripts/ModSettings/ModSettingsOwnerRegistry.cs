using System.Collections.Generic;
using Timberborn.Common;
using Timberborn.Modding;

namespace ModSettings {
  public class ModSettingsOwnerRegistry {

    private readonly Dictionary<Mod, List<ModSettingsOwner>> _modSettingOwners = new();

    public void RegisterModSettingOwner(Mod mod, 
                                        ModSettingsOwner modSettingsOwner) {
      _modSettingOwners.GetOrAdd(mod).Add(modSettingsOwner);
    }

    public bool HasModSettingOwners(Mod mod) {
      return _modSettingOwners.ContainsKey(mod);
    }

    public ReadOnlyList<ModSettingsOwner> GetModSettingOwners(Mod mod) {
      return new(_modSettingOwners[mod]);
    }

  }
}