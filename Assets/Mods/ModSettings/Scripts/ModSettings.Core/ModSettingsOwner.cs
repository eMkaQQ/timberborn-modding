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
    private readonly List<object> _modSettings = new();

    protected ModSettingsOwner(ISettings settings,
                               ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                               ModRepository modRepository) {
      _settings = settings;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _modRepository = modRepository;
    }

    public ReadOnlyList<object> ModSettings => new(_modSettings);
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
      _modSettings.Add(settingObject);
      if (genericType == typeof(int)) {
        var intSetting = (ModSetting<int>) settingObject;
        intSetting.SetValue(_settings.GetInt(key, intSetting.DefaultValue));
        intSetting.ValueChanged += (_, value) => _settings.SetInt(key, value);
      } else if (genericType == typeof(float)) {
        var floatSetting = (ModSetting<float>) settingObject;
        floatSetting.SetValue(_settings.GetFloat(key, floatSetting.DefaultValue));
        floatSetting.ValueChanged += (_, value) => _settings.SetFloat(key, value);
      } else if (genericType == typeof(string)) {
        var stringSetting = (ModSetting<string>) settingObject;
        stringSetting.SetValue(_settings.GetString(key, stringSetting.DefaultValue));
        stringSetting.ValueChanged += (_, value) => _settings.SetString(key, value);
      } else if (genericType == typeof(bool)) {
        var boolSetting = (ModSetting<bool>) settingObject;
        boolSetting.SetValue(_settings.GetBool(key, boolSetting.DefaultValue));
        boolSetting.ValueChanged += (_, value) => _settings.SetBool(key, value);
      } else {
        throw new($"Unsupported ModSetting type {genericType}");
      }
    }

    private void RegisterModSettingOwner() {
      foreach (var mod in _modRepository.Mods.Where(m => m.Manifest.Id == ModId)) {
        _modSettingsOwnerRegistry.RegisterModSettingOwner(mod, this);
      }
    }

  }
}