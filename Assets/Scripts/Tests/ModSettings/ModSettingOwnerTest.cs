using JetBrains.Annotations;
using ModSettings.Common;
using ModSettings.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using Version = Timberborn.Versioning.Version;

namespace Tests.ModSettings {
  public class ModSettingOwnerTest {

    [Test]
    public void ShouldLoadDefaultSettings() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new SettingsMock(), new(), modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.AreEqual(2, modSettingOwner.IntSetting.Value);
      Assert.AreEqual(1.1f, modSettingOwner.FloatSetting.Value);
      Assert.AreEqual("default", modSettingOwner.StringSetting.Value);
      Assert.IsTrue(modSettingOwner.BoolSetting.Value);
      Assert.AreEqual(5, modSettingOwner.SmallIntRangeSetting.Value);
      Assert.AreEqual(-50, modSettingOwner.NegativeRangeSetting.Value);
      Assert.AreEqual("value2", modSettingOwner.DropdownSetting.Value);
    }

    [Test]
    public void ShouldLoadCustomSettings() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new FilledSettingsMock(), new(), modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.AreEqual(3, modSettingOwner.IntSetting.Value);
      Assert.AreEqual(2.2f, modSettingOwner.FloatSetting.Value);
      Assert.AreEqual("custom", modSettingOwner.StringSetting.Value);
      Assert.IsFalse(modSettingOwner.BoolSetting.Value);
    }

    [Test]
    public void ShouldAddAllSettingsToModSettings() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new SettingsMock(), new(), modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.AreEqual(7, modSettingOwner.ModSettings.Count);
    }

    [Test]
    public void ShouldThrowExceptionWhenSettingTypeIsNotSupported() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new IncorrectModSettingOwner(new SettingsMock(), new(),
                                                         modRepository);

      // when, then
      Assert.Throws<Exception>(() => modSettingOwner.Load());
    }

    [Test]
    public void ShouldSubscribeToSettingChanges() {
      // given
      var settings = new SettingsMock();
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(settings, new(), modRepository);
      modSettingOwner.Load();

      // when
      modSettingOwner.IntSetting.SetValue(5);
      modSettingOwner.FloatSetting.SetValue(3.3f);
      modSettingOwner.StringSetting.SetValue("new");
      modSettingOwner.BoolSetting.SetValue(false);

      // then
      Assert.AreEqual(5,
                      settings.GetInt(GetModSettingKey("modMock", nameof(ModSettingOwnerMock),
                                                       nameof(modSettingOwner.IntSetting)), 2));
      Assert.AreEqual(3.3f,
                      settings.GetFloat(GetModSettingKey("modMock", nameof(ModSettingOwnerMock),
                                                         nameof(modSettingOwner.FloatSetting)),
                                        1.1f));
      Assert.AreEqual("new",
                      settings.GetString(GetModSettingKey("modMock", nameof(ModSettingOwnerMock),
                                                          nameof(modSettingOwner.StringSetting)),
                                         "default"));
      Assert.IsFalse(settings.GetBool(GetModSettingKey("modMock", nameof(ModSettingOwnerMock),
                                                       nameof(modSettingOwner.BoolSetting)), true));
    }

    [Test]
    public void ShouldIgnoreNonModSettingProperties() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new NoSettingsModSettingOwner(new SettingsMock(), new(), modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.AreEqual(0, modSettingOwner.ModSettings.Count);
    }

    [Test]
    public void ShouldAddModSettingOwnerToRegistry() {
      // given
      var mod = CreateMod("modMock");
      var modRepository = CreateModRepository(new[] { mod });
      var modSettingOwnerRegistry = new ModSettingsOwnerRegistry();
      var modSettingOwner = new ModSettingOwnerMock(new SettingsMock(), modSettingOwnerRegistry,
                                                    modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.IsTrue(modSettingOwnerRegistry.HasModSettings(mod));
    }

    [Test]
    public void ShouldAddModSettingOwnerToCorrectMod() {
      // given
      var mod = CreateMod("modMock");
      var dummyMod = CreateMod("dummyMod");
      var randomMod = CreateMod("randomMod");
      var modRepository = CreateModRepository(new[] { dummyMod, mod, randomMod });
      var modSettingOwnerRegistry = new ModSettingsOwnerRegistry();
      var modSettingOwner = new ModSettingOwnerMock(new SettingsMock(), modSettingOwnerRegistry,
                                                    modRepository);

      // when
      modSettingOwner.Load();

      // then
      var modSettingOwners = modSettingOwnerRegistry.GetModSettingOwners(mod);
      Assert.AreEqual(1, modSettingOwners.Count);
      Assert.AreEqual(modSettingOwner, modSettingOwners[0]);
      Assert.IsFalse(modSettingOwnerRegistry.HasModSettings(dummyMod));
      Assert.IsFalse(modSettingOwnerRegistry.HasModSettings(randomMod));
    }

    [Test]
    public void ShouldLoadSettingsForNonExistingMod() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new NonExistingModSettingOwner(new SettingsMock(), new(),
                                                           modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.AreEqual(2, modSettingOwner.IntSetting.Value);
    }

    [Test]
    public void ShouldNotAddModSettingOwnerToRegistryForNonExistingMod() {
      // given
      var mod = CreateMod("modMock");
      var modRepository = CreateModRepository(new[] { mod });
      var modSettingOwnerRegistry = new ModSettingsOwnerRegistry();
      var modSettingOwner = new NonExistingModSettingOwner(new SettingsMock(),
                                                           modSettingOwnerRegistry,
                                                           modRepository);

      // when
      modSettingOwner.Load();

      // then
      Assert.IsFalse(modSettingOwnerRegistry.HasModSettings(mod));
    }

    [Test]
    public void ShouldAddCustomModSetting() {
      // given
      var mod = CreateMod("modMock");
      var modRepository = CreateModRepository(new[] { mod });
      var modSettingOwnerRegistry = new ModSettingsOwnerRegistry();
      var modSettingOwner = new ModSettingOwnerMock(new SettingsMock(), modSettingOwnerRegistry,
                                                    modRepository);
      var customModSetting = new ModSetting<int>(20, ModSettingDescriptor.Create("custom"));
      Assert.AreEqual(0, customModSetting.Value);

      // when
      modSettingOwner.Load();
      modSettingOwner.AddCustomModSetting(customModSetting, "CustomSetting");

      // then
      Assert.AreEqual(8, modSettingOwner.ModSettings.Count);
      Assert.AreEqual(20, customModSetting.Value);
    }

    [Test]
    public void ShouldHaveSettingAfterAddingCustomModSetting() {
      // given
      var mod = CreateMod("modMock");
      var modRepository = CreateModRepository(new[] { mod });
      var modSettingOwnerRegistry = new ModSettingsOwnerRegistry();
      var modSettingOwner = new NoSettingsModSettingOwner(new SettingsMock(),
                                                          modSettingOwnerRegistry,
                                                          modRepository);
      var customModSetting = new ModSetting<int>(20, ModSettingDescriptor.Create("custom"));

      // when
      modSettingOwner.Load();
      modSettingOwner.AddCustomModSetting(customModSetting, "CustomSetting");

      // then
      Assert.IsTrue(modSettingOwnerRegistry.HasModSettings(mod));
    }

    [Test]
    public void ShouldResetModSettings() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new FilledSettingsMock(), new(), modRepository);
      modSettingOwner.Load();

      // when
      modSettingOwner.ResetModSettings();

      // then
      Assert.AreEqual(2, modSettingOwner.IntSetting.Value);
      Assert.AreEqual(1.1f, modSettingOwner.FloatSetting.Value);
      Assert.AreEqual("default", modSettingOwner.StringSetting.Value);
      Assert.IsTrue(modSettingOwner.BoolSetting.Value);
    }

    [Test]
    public void ShouldDiscardInvalidModSetting() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new FilledSettingsMock(), new(), modRepository);
      var invalidModSetting = new InvalidModSetting(0, ModSettingDescriptor.Create("invalid"));

      // when
      modSettingOwner.Load();
      modSettingOwner.AddCustomModSetting(invalidModSetting, "InvalidSetting");

      // then
      Assert.AreEqual(7, modSettingOwner.ModSettings.Count);
    }

    [Test]
    public void ShouldResetInvalidLimitedStringModSettingValue() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var settings = new SettingsMock();
      var modSettingOwner = new NoSettingsModSettingOwner(settings, new(), modRepository);
      var limitedStringModSettingValue = new LimitedStringModSettingValue("default", "default");
      var limitedStringModSetting =
          new LimitedStringModSetting(0, new[] { limitedStringModSettingValue },
                                      ModSettingDescriptor.Create("default"));
      var key = "ModSetting.modMock.limitedString";

      // when
      settings.SetString(key, "invalid");
      Assert.IsTrue(settings.GetString(key, null) == "invalid");
      modSettingOwner.Load();
      modSettingOwner.AddCustomModSetting(limitedStringModSetting, "limitedString");

      // then
      Assert.AreEqual("default", limitedStringModSetting.Value);
      Assert.IsTrue(settings.GetString(key, null) == null);
    }

    [Test]
    public void ShouldAddNonPersistentModSetting() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new NoSettingsModSettingOwner(new SettingsMock(), new(), modRepository);
      var nonPersistentSetting =
          new NonPeristentDummy(ModSettingDescriptor.Create("nonPersistent"));

      // when
      modSettingOwner.Load();
      modSettingOwner.AddNonPersistentModSetting(nonPersistentSetting);

      // then
      Assert.AreEqual(1, modSettingOwner.ModSettings.Count);
    }
    
    [Test]
    public void ShouldInvokeModSettingChangedEvent() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new FilledSettingsMock(), new(), modRepository);
      modSettingOwner.Load();
      var changed = false;
      modSettingOwner.ModSettingChanged += (_, _) => changed = true;

      // when
      modSettingOwner.IntSetting.SetValue(2);

      // then
      Assert.IsTrue(changed);
    }
    
    [Test]
    public void ShouldPassCorrectModSettingToModSettingChangedEvent() {
      // given
      var modRepository = CreateModRepository(new[] { CreateMod("modMock") });
      var modSettingOwner = new ModSettingOwnerMock(new FilledSettingsMock(), new(), modRepository);
      modSettingOwner.Load();
      ModSetting changedModSetting = null;
      modSettingOwner.ModSettingChanged += (_, e) => changedModSetting = e.ModSetting;

      // when
      modSettingOwner.IntSetting.SetValue(2);

      // then
      Assert.AreEqual(modSettingOwner.IntSetting, changedModSetting);
    }

    private static string GetModSettingKey(string modId, string modSettingOwnerName,
                                           string settingName) {
      return $"ModSetting.{modId}.{modSettingOwnerName}.{settingName}";
    }

    private static Mod CreateMod(string modId) {
      return new(new(), new("", "", Version.Create("0"), modId,
                            Version.Create("0"), new List<VersionedMod>(),
                            new List<VersionedMod>()), true);
    }

    private static ModRepository CreateModRepository(IEnumerable<Mod> mods) {
      var modRepository = new ModRepository(null, null, new List<IModsProvider>());
      var modsProperty = modRepository.GetType()
          .GetProperty("Mods",
                       BindingFlags.Instance
                       | BindingFlags.Public
                       | BindingFlags.GetProperty
                       | BindingFlags.SetProperty);
      Assert.IsNotNull(modsProperty);
      modsProperty.SetValue(modRepository, mods.ToImmutableArray());
      ModdedState.SetOfficialMods(mods);
      return modRepository;
    }

    private class ModSettingOwnerMock : ModSettingsOwner {

      public ModSetting<int> SmallIntRangeSetting { get; } = new RangeIntModSetting(
          5, 0, 10, ModSettingDescriptor.Create("eMka.ModSettingsExamples.SmallIntRange"));

      public ModSetting<int> NegativeRangeSetting { get; } = new RangeIntModSetting(
          -50, -100, 100, ModSettingDescriptor.Create("eMka.ModSettingsExamples.BigIntRange"));

      public ModSetting<string> DropdownSetting { get; } = new LimitedStringModSetting(
          1, new[] {
              new LimitedStringModSettingValue("value1", "eMka.ModSettingsExamples.Dropdown1"),
              new LimitedStringModSettingValue("value2", "eMka.ModSettingsExamples.Dropdown2"),
              new LimitedStringModSettingValue("value3", "eMka.ModSettingsExamples.Dropdown3")
          }, ModSettingDescriptor.Create("eMka.ModSettingsExamples.Dropdown"));

      public ModSetting<int> IntSetting { get; } =
        new(2, ModSettingDescriptor.Create("eMka.ModSettingsExamples.IntSetting"));

      public ModSetting<float> FloatSetting { get; } =
        new(1.1f, ModSettingDescriptor.Create("eMka.ModSettingsExamples.FloatSetting"));

      public ModSetting<string> StringSetting { get; } =
        new("default", ModSettingDescriptor.Create("eMka.ModSettingsExamples.StringSetting"));

      public ModSetting<bool> BoolSetting { get; } =
        new(true, ModSettingDescriptor.Create("eMka.ModSettingsExamples.BoolSetting"));

      public ModSettingOwnerMock(ISettings settings,
                                 ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                 ModRepository modRepository) : base(
          settings, modSettingsOwnerRegistry, modRepository) {
      }

      protected override string ModId => "modMock";

    }

    private class IncorrectModSettingOwner : ModSettingsOwner {

      [UsedImplicitly]
      public ModSetting<char> CharSetting { get; } =
        new('a', ModSettingDescriptor.Create("eMka.ModSettingsExamples.CharSetting"));

      public IncorrectModSettingOwner(ISettings settings,
                                      ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                      ModRepository modRepository) : base(
          settings, modSettingsOwnerRegistry, modRepository) {
      }

      protected override string ModId => "modMock";

    }

    private class NoSettingsModSettingOwner : ModSettingsOwner {

      [UsedImplicitly]
      public List<int> IntSetting { get; } = new();

      public NoSettingsModSettingOwner(ISettings settings,
                                       ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                       ModRepository modRepository) : base(
          settings, modSettingsOwnerRegistry, modRepository) {
      }

      protected override string ModId => "modMock";

    }

    private class NonExistingModSettingOwner : ModSettingsOwner {

      public ModSetting<int> IntSetting { get; } =
        new(2, ModSettingDescriptor.Create("eMka.ModSettingsExamples.IntSetting"));

      public NonExistingModSettingOwner(ISettings settings,
                                        ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                        ModRepository modRepository) : base(
          settings, modSettingsOwnerRegistry, modRepository) {
      }

      protected override string ModId => "nonExistingMod";

    }

    private class SettingsMock : ISettings {

      private readonly Dictionary<string, int> _intSettings = new();
      private readonly Dictionary<string, float> _floatSettings = new();
      private readonly Dictionary<string, string> _stringSettings = new();
      private readonly Dictionary<string, bool> _boolSettings = new();

      public int GetSafeInt(string key, int defaultValue) {
        throw new NotSupportedException();
      }

      public void SetInt(string key, int value) {
        _intSettings[key] = value;
      }

      public int GetInt(string key, int defaultValue) {
        return _intSettings.GetValueOrDefault(key, defaultValue);
      }

      public float GetSafeFloat(string key, float defaultValue) {
        throw new NotSupportedException();
      }

      public void SetFloat(string key, float value) {
        _floatSettings[key] = value;
      }

      public float GetFloat(string key, float defaultValue) {
        return _floatSettings.GetValueOrDefault(key, defaultValue);
      }

      public string GetSafeString(string key, string defaultValue) {
        throw new NotSupportedException();
      }

      public void SetString(string key, string value) {
        _stringSettings[key] = value;
      }

      public string GetString(string key, string defaultValue) {
        return _stringSettings.GetValueOrDefault(key, defaultValue);
      }

      public bool GetSafeBool(string key, bool defaultValue) {
        throw new NotSupportedException();
      }

      public void SetBool(string key, bool value) {
        _boolSettings[key] = value;
      }

      public bool GetBool(string key, bool defaultValue) {
        return _boolSettings.GetValueOrDefault(key, defaultValue);
      }

      public void Clear(string key) {
        _intSettings.Remove(key);
        _floatSettings.Remove(key);
        _stringSettings.Remove(key);
        _boolSettings.Remove(key);
      }

    }

    private class FilledSettingsMock : ISettings {

      private readonly Dictionary<string, int> _intSettings = new();
      private readonly Dictionary<string, float> _floatSettings = new();
      private readonly Dictionary<string, string> _stringSettings = new();
      private readonly Dictionary<string, bool> _boolSettings = new();

      public FilledSettingsMock() {
        _intSettings[GetModSettingKey("modMock", nameof(ModSettingOwnerMock), "IntSetting")] = 3;
        _floatSettings[GetModSettingKey("modMock", nameof(ModSettingOwnerMock), "FloatSetting")] =
            2.2f;
        _stringSettings[GetModSettingKey("modMock", nameof(ModSettingOwnerMock), "StringSetting")] =
            "custom";
        _boolSettings[GetModSettingKey("modMock", nameof(ModSettingOwnerMock), "BoolSetting")] =
            false;
      }

      public int GetSafeInt(string key, int defaultValue) {
        throw new NotSupportedException();
      }

      public void SetInt(string key, int value) {
        _intSettings[key] = value;
      }

      public int GetInt(string key, int defaultValue) {
        return _intSettings.GetValueOrDefault(key, defaultValue);
      }

      public float GetSafeFloat(string key, float defaultValue) {
        throw new NotSupportedException();
      }

      public void SetFloat(string key, float value) {
        _floatSettings[key] = value;
      }

      public float GetFloat(string key, float defaultValue) {
        return _floatSettings.GetValueOrDefault(key, defaultValue);
      }

      public string GetSafeString(string key, string defaultValue) {
        throw new NotSupportedException();
      }

      public void SetString(string key, string value) {
        _stringSettings[key] = value;
      }

      public string GetString(string key, string defaultValue) {
        return _stringSettings.GetValueOrDefault(key, defaultValue);
      }

      public bool GetSafeBool(string key, bool defaultValue) {
        throw new NotSupportedException();
      }

      public void SetBool(string key, bool value) {
        _boolSettings[key] = value;
      }

      public bool GetBool(string key, bool defaultValue) {
        return _boolSettings.GetValueOrDefault(key, defaultValue);
      }

      public void Clear(string key) {
        throw new NotSupportedException();
      }

    }

    private class InvalidModSetting : ModSetting<int> {

      public InvalidModSetting(int defaultValue, ModSettingDescriptor descriptor) : base(
          defaultValue, descriptor) {
      }

      public override bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings,
                                   string key) {
        return false;
      }

    }

    private class NonPeristentDummy : NonPersistentSetting {

      public NonPeristentDummy(ModSettingDescriptor descriptor) : base(descriptor) {
      }

    }

  }
}