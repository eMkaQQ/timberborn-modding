using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class FileStoredSettings : ISettings {

    private Dictionary<string, string> _data;
    private FileInfo _fileInfo;

    public void Initialize(FileInfo fileInfo) {
      _fileInfo = fileInfo;
      _data = new();
      if (_fileInfo.Exists) {
        try {
          var jsonContent = File.ReadAllText(_fileInfo.FullName);
          _data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
        } catch (Exception e) {
          Debug.LogWarning(
              $"Failed loading settings from {_fileInfo.FullName}. Deleting it. Details: {e}");
          File.Delete(_fileInfo.FullName);
        }
      }
    }

    public int GetInt(string key, int defaultValue) {
      if (_data.TryGetValue(key, out var value) && int.TryParse(value, out var result)) {
        return result;
      }
      return defaultValue;
    }

    public int GetSafeInt(string key, int defaultValue) {
      return GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value) {
      _data[key] = value.ToString();
      Save();
    }

    public bool GetBool(string key, bool defaultValue = false) {
      if (_data.TryGetValue(key, out var value) && bool.TryParse(value, out var result)) {
        return result;
      }
      return defaultValue;
    }

    public bool GetSafeBool(string key, bool defaultValue = false) {
      return GetBool(key, defaultValue);
    }

    public void SetBool(string key, bool value) {
      _data[key] = value.ToString();
      Save();
    }

    public float GetFloat(string key, float defaultValue) {
      if (_data.TryGetValue(key, out var value)
          && float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out var result)) {
        return result;
      }
      return defaultValue;
    }

    public float GetSafeFloat(string key, float defaultValue) {
      return GetFloat(key, defaultValue);
    }

    public void SetFloat(string key, float value) {
      _data[key] = value.ToString(CultureInfo.InvariantCulture);
      Save();
    }

    public string GetString(string key, string defaultValue) {
      return _data.GetValueOrDefault(key, defaultValue);
    }

    public string GetSafeString(string key, string defaultValue) {
      return GetString(key, defaultValue);
    }

    public void SetString(string key, string value) {
      _data[key] = value;
      Save();
    }

    public void Clear(string key) {
      if (_data.ContainsKey(key)) {
        _data.Remove(key);
      }
    }

    private void Save() {
      var jsonContent = JsonConvert.SerializeObject(_data, Formatting.Indented);
      File.WriteAllText(_fileInfo.FullName, jsonContent);
    }

  }
}