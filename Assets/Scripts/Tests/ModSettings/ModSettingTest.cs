using ModSettings.Common;
using ModSettings.Core;
using NUnit.Framework;

namespace Tests.ModSettings {
  public class ModSettingTest {

    [Test]
    public void ShouldSetValue() {
      // given
      var modSetting = new ModSetting<int>(0, ModSettingDescriptor.Create("test"));

      // when
      modSetting.SetValue(1);

      // then
      Assert.That(modSetting.Value, Is.EqualTo(1));
    }

    [Test]
    public void ShouldInvokeValueChangedEvent() {
      // given
      var modSetting = new ModSetting<int>(0, ModSettingDescriptor.Create("test"));
      var invoked = false;
      modSetting.ValueChanged += (_, _) => invoked = true;

      // when
      modSetting.SetValue(1);

      // then
      Assert.That(invoked, Is.True);
    }

    [Test]
    public void ShouldNotInvokeValueChangedEvent() {
      // given
      var modSetting = new ModSetting<int>(0, ModSettingDescriptor.Create("test"));
      var invoked = false;
      modSetting.ValueChanged += (_, _) => invoked = true;

      // when
      modSetting.SetValue(0);

      // then
      Assert.That(invoked, Is.False);
    }

    [Test]
    public void ShouldRangeIntModSettingClampValue() {
      // given
      var lowModSetting = new RangeIntModSetting(0, 1, 10, ModSettingDescriptor.Create("test"));
      var highModSetting = new RangeIntModSetting(0, 1, 10, ModSettingDescriptor.Create("test"));

      // when
      lowModSetting.SetValue(0);
      highModSetting.SetValue(11);

      // then
      Assert.That(lowModSetting.Value, Is.EqualTo(1));
      Assert.That(highModSetting.Value, Is.EqualTo(10));
    }

    [Test]
    public void ShouldLimitedStringModSettingThrowException() {
      // given
      var values = new[] {
          new LimitedStringModSettingValue("a", ""),
          new LimitedStringModSettingValue("b", ""),
          new LimitedStringModSettingValue("c", "")
      };
      var modSetting = new LimitedStringModSetting(0, values, ModSettingDescriptor.Create("test"));

      // then
      Assert.That(() => modSetting.SetValue("d"), Throws.ArgumentException);
    }

    [Test]
    public void ShouldLimitedStringModSettingSetValue() {
      // given
      var values = new[] {
          new LimitedStringModSettingValue("a", ""),
          new LimitedStringModSettingValue("b", ""),
          new LimitedStringModSettingValue("c", "")
      };
      var modSetting = new LimitedStringModSetting(0, values, ModSettingDescriptor.Create("test"));

      // when
      modSetting.SetValue("b");

      // then
      Assert.That(modSetting.Value, Is.EqualTo("b"));
    }

  }
}