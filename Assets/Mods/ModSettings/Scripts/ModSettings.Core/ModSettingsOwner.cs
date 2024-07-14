using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Timberborn.Common;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace ModSettings.Core {
  public abstract class ModSettingsOwner : ILoadableSingleton {

    private readonly ISettings _settings;
    private readonly ModSettingsOwnerRegistry _modSettingsOwnerRegistry;
    private readonly ModRepository _modRepository;
    private readonly List<ModSetting> _modSettings = new();

    protected ModSettingsOwner(ISettings settings,
                               ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                               ModRepository modRepository) {
      _settings = settings;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _modRepository = modRepository;
    }

    public ReadOnlyList<ModSetting> ModSettings => new(_modSettings);
    public virtual int Order => 0;
    public virtual string HeaderLocKey => null;

    public void Load() {
      OnBeforeLoad();
      InitializePropertyModSettings();
      RegisterModSettingOwner();
      OnAfterLoad();
    }

    public void AddCustomModSetting<T>(ModSetting<T> modSetting, string id) {
      var key = $"ModSetting.{ModId}.{id}";
      InitializeModSetting(modSetting, typeof(T), key);
    }

    public void ResetModSettings() {
      foreach (var modSetting in _modSettings) {
        modSetting.Reset();
      }
    }

    protected abstract string ModId { get; }

    protected virtual void OnBeforeLoad() {
    }

    protected virtual void OnAfterLoad() {
    }

    private void InitializePropertyModSettings() {
      var type = GetType();
      var properties =
          type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
      foreach (var memberInfo in properties) {
        if (memberInfo is PropertyInfo propertyInfo) {
          if (propertyInfo.PropertyType.IsGenericType
              && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ModSetting<>)) {
            InitializePropertyModSetting(type, propertyInfo);
          }
        }
      }
    }

    private void InitializePropertyModSetting(Type type, PropertyInfo propertyInfo) {
      var key = $"ModSetting.{ModId}.{type.Name}.{propertyInfo.Name}";
      var genericType = propertyInfo.PropertyType.GetGenericArguments()[0];
      var settingObject = propertyInfo.GetValue(this);
      InitializeModSetting(settingObject, genericType, key);
    }

    private void InitializeModSetting(object settingObject, Type genericType, string key) {
      if (genericType == typeof(int)) {
        InitializeModSetting(
            (ModSetting<int>) settingObject, key, _settings.GetInt, _settings.SetInt);
      } else if (genericType == typeof(float)) {
        InitializeModSetting(
            (ModSetting<float>) settingObject, key, _settings.GetFloat, _settings.SetFloat);
      } else if (genericType == typeof(string)) {
        InitializeModSetting(
            (ModSetting<string>) settingObject, key, _settings.GetString, _settings.SetString);
      } else if (genericType == typeof(bool)) {
        InitializeModSetting(
            (ModSetting<bool>) settingObject, key, _settings.GetBool, _settings.SetBool);
      } else {
        throw new($"Unsupported ModSetting type {genericType}");
      }
    }

    private void InitializeModSetting<T>(ModSetting<T> modSetting, string key,
                                         Func<string, T, T> valueGetter,
                                         Action<string, T> valueSetter) {
      _modSettings.Add(modSetting);
      modSetting.SetValue(valueGetter(key, modSetting.DefaultValue));
      modSetting.ValueChanged += (_, value) => valueSetter(key, value);
    }

    private void RegisterModSettingOwner() {
      foreach (var mod in _modRepository.Mods.Where(m => m.Manifest.Id == ModId)) {
        _modSettingsOwnerRegistry.RegisterModSettingOwner(mod, this);
      }
    }

  }
}