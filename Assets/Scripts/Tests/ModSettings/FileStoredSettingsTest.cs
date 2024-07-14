using ModSettings.Common;
using NUnit.Framework;
using System.IO;

namespace Tests.ModSettings {
  public class FileStoredSettingsTest {

    private FileStoredSettings _fileStoredSettings;
    private string _path;

    [SetUp]
    public void SetUp() {
      _fileStoredSettings = new();
      _path = Path.GetTempFileName();
      File.WriteAllText(_path, GetDefaultSettingContent());
      _fileStoredSettings.Initialize(new(_path));
    }

    [TearDown]
    public void TearDown() {
      _fileStoredSettings = null;
      if (!string.IsNullOrEmpty(_path)) {
        File.Delete(_path);
      }
    }

    [Test]
    public void ShouldSaveSettings() {
      // when
      _fileStoredSettings.SetInt("key", 42);
      _fileStoredSettings.SetFloat("key2", 42.42f);
      _fileStoredSettings.SetBool("key3", true);
      _fileStoredSettings.SetString("key4", "value");

      // then
      Assert.AreEqual(42, _fileStoredSettings.GetInt("key", 0));
      Assert.AreEqual(42.42f, _fileStoredSettings.GetFloat("key2", 0f));
      Assert.AreEqual(true, _fileStoredSettings.GetBool("key3"));
      Assert.AreEqual("value", _fileStoredSettings.GetString("key4", ""));
    }

    [Test]
    public void ShouldReturnDefaultValues() {
      // then
      Assert.AreEqual(3, _fileStoredSettings.GetInt("key", 3));
      Assert.AreEqual(0.1f, _fileStoredSettings.GetFloat("key2", 0.1f));
      Assert.AreEqual(true, _fileStoredSettings.GetBool("key3", true));
      Assert.AreEqual("value", _fileStoredSettings.GetString("key4", "value"));
    }

    [Test]
    public void ShouldLoadValuesFromFile() {
      // then
      Assert.AreEqual(-5, _fileStoredSettings.GetInt("initial.key", 0));
      Assert.AreEqual(5.5f, _fileStoredSettings.GetFloat("initial.key2", 0f));
      Assert.AreEqual(true, _fileStoredSettings.GetBool("initial.key3"));
      Assert.AreEqual("savedValue", _fileStoredSettings.GetString("initial.key4", ""));
    }

    [Test]
    public void ShouldOverwriteValues() {
      // when
      _fileStoredSettings.SetInt("initial.key", 42);
      _fileStoredSettings.SetFloat("initial.key2", 42.42f);
      _fileStoredSettings.SetBool("initial.key3", false);
      _fileStoredSettings.SetString("initial.key4", "newValue");

      // then
      Assert.AreEqual(42, _fileStoredSettings.GetInt("initial.key", 0));
      Assert.AreEqual(42.42f, _fileStoredSettings.GetFloat("initial.key2", 0f));
      Assert.AreEqual(false, _fileStoredSettings.GetBool("initial.key3"));
      Assert.AreEqual("newValue", _fileStoredSettings.GetString("initial.key4", ""));
    }

    private static string GetDefaultSettingContent() {
      return
          "{\"initial.key\":\"-5\",\"initial.key2\":\"5.5\","
          + "\"initial.key3\":\"True\",\"initial.key4\":\"savedValue\"}";
    }

  }
}