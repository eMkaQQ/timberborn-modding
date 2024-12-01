using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.Modding;

namespace ModSettings.Core {
  public class ModSettingsOwnerRegistry {

    private readonly Dictionary<Mod, List<ModSettingsOwner>> _modSettingOwners = new();

    public void RegisterModSettingOwner(Mod mod,
                                        ModSettingsOwner modSettingsOwner) {
      var list = _modSettingOwners.GetOrAdd(mod);
      list.Add(modSettingsOwner);
      list.Sort((a, b) => string.Compare(a.GetType().Name, b.GetType().Name,
                                         StringComparison.Ordinal));
    }

    public bool HasModSettings(Mod mod) {
      return _modSettingOwners.TryGetValue(mod, out var modSettingsOwners)
             && modSettingsOwners.Any(modSettingsOwner => modSettingsOwner.ModSettings.Any());
    }

    public ReadOnlyList<ModSettingsOwner> GetModSettingOwners(Mod mod) {
      return new(_modSettingOwners[mod]);
    }

  }
}