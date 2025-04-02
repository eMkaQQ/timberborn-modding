using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Timberborn.Common;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModSettings.Core {
  public abstract class ModSettingsOwner : ILoadableSingleton {

    public event EventHandler<ModSettingChangedEvent> ModSettingChanged;
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
    public virtual ModSettingsContext ChangeableOn => ModSettingsContext.MainMenu;

    public void Load() {
      OnBeforeLoad();
      InitializePropertyModSettings();
      RegisterModSettingOwner();
      OnAfterLoad();
    }

    [UsedImplicitly]
    public void AddCustomModSetting<T>(ModSetting<T> modSetting, string id) {
      var key = $"ModSetting.{ModId}.{id}";
      InitializeModSetting(modSetting, typeof(T), key);
    }

    [UsedImplicitly]
    public void AddNonPersistentModSetting(NonPersistentSetting nonPersistentSetting) {
      _modSettings.Add(nonPersistentSetting);
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
          if (typeof(ModSetting).IsAssignableFrom(propertyInfo.PropertyType)) {
            InitializePropertyModSetting(type, propertyInfo);
          }
        }
      }
    }

    private void InitializePropertyModSetting(Type type, PropertyInfo propertyInfo) {
      var key = $"ModSetting.{ModId}.{type.Name}.{propertyInfo.Name}";
      var settingObject = propertyInfo.GetValue(this);
      if (typeof(NonPersistentSetting).IsAssignableFrom(propertyInfo.PropertyType)) {
        var modSetting = (ModSetting) settingObject;
        _modSettings.Add(modSetting);
      } else {
        var genericType = GetGenericType(propertyInfo.PropertyType);
        InitializeModSetting(settingObject, genericType, key);
      }
    }

    private static Type GetGenericType(Type type) {
      var originalType = type;
      while (true) {
        if (type.IsGenericType) {
          return type.GetGenericArguments()[0];
        }
        if (type.BaseType != null) {
          type = type.BaseType;
        } else {
          throw new($"Could not find generic type for {originalType}");
        }
      }
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
      if (modSetting.IsValid(this, _settings, key)) {
        _modSettings.Add(modSetting);
        modSetting.SetValue(valueGetter(key, modSetting.DefaultValue));
        modSetting.ValueChanged += (_, value) => {
          valueSetter(key, value);
          ModSettingChanged?.Invoke(this, new(modSetting));
        };
      }
    }

    private void RegisterModSettingOwner() {
      var mod = _modRepository.EnabledMods.FirstOrDefault(m => m.Manifest.Id == ModId);
      if (mod != null) {
        _modSettingsOwnerRegistry.RegisterModSettingOwner(mod, this);
      } else {
        Debug.LogWarning($"Could not find mod with id {ModId} for {this}");
      }
    }

  }
}