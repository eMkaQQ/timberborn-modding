using ModSettings.Common;
using ModSettings.Core;
using NUnit.Framework;
using UnityEngine;

namespace Tests.ModSettings {
  public class ColorSettingTest {

    [Test]
    public void ShouldCreateColorModSettingFromString() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.Reset();

      // then
      Assert.AreEqual(modSetting.Value, "808080");
      var grey = new Color(0.5f, 0.5f, 0.5f, 1);
      Assert.AreEqual(grey.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(grey.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(grey.b, modSetting.Color.b, 0.01);
    }

    [Test]
    public void ShouldCreateColorModSettingFromColor() {
      // given
      var modSetting = new ColorModSetting(new Color(0.5f, 0.5f, 0.5f, 1),
                                           ModSettingDescriptor.Create("test"), false);

      // when
      modSetting.Reset();

      // then
      Assert.AreEqual(modSetting.Value, "808080");
      var grey = new Color(0.5f, 0.5f, 0.5f, 1);
      Assert.AreEqual(grey.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(grey.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(grey.b, modSetting.Color.b, 0.01);
    }

    [Test]
    public void ShouldSetColorModSettingValue() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.SetValue("FF0000");

      // then
      Assert.AreEqual(modSetting.Value, "FF0000");
      var red = new Color(1, 0, 0, 1);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
    }

    [Test]
    public void ShouldNotSetColorModSettingValue() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.SetValue("invalid");

      // then
      Assert.AreEqual(modSetting.Value, "808080");
      var grey = new Color(0.5f, 0.5f, 0.5f, 1);
      Assert.AreEqual(grey.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(grey.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(grey.b, modSetting.Color.b, 0.01);
    }

    [Test]
    public void ShouldNotIgnoreAlpha() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           true);

      // when
      modSetting.SetValue("FF000080");

      // then
      Assert.AreEqual(modSetting.Value, "FF000080");
      var red = new Color(1, 0, 0, 0.5f);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
      Assert.AreEqual(red.a, modSetting.Color.a, 0.01);
    }

    [Test]
    public void ShouldIgnoreAlpha() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.SetValue("FF000080");

      // then
      Assert.AreEqual(modSetting.Value, "FF0000");
      var red = new Color(1, 0, 0, 1);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
      Assert.AreEqual(red.a, modSetting.Color.a, 0.01);
    }

    [Test]
    public void ShouldSetColorModSettingValueUsingColor() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.SetValue(new(1, 0, 0, 1));

      // then
      Assert.AreEqual(modSetting.Value, "FF0000");
      var red = new Color(1, 0, 0, 1);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
    }

    [Test]
    public void ShouldSetColorModSettingValueUsingColorWithAlpha() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           true);

      // when
      modSetting.SetValue(new(1, 0, 0, 0.5f));

      // then
      Assert.AreEqual(modSetting.Value, "FF000080");
      var red = new Color(1, 0, 0, 0.5f);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
      Assert.AreEqual(red.a, modSetting.Color.a, 0.01);
    }

    [Test]
    public void ShouldSetColorModSettingValueUsingColorIgnoringAlpha() {
      // given
      var modSetting = new ColorModSetting("808080", ModSettingDescriptor.Create("test"),
                                           false);

      // when
      modSetting.SetValue(new(1, 0, 0, 0.5f));

      // then
      Assert.AreEqual(modSetting.Value, "FF0000");
      var red = new Color(1, 0, 0, 1);
      Assert.AreEqual(red.r, modSetting.Color.r, 0.01);
      Assert.AreEqual(red.g, modSetting.Color.g, 0.01);
      Assert.AreEqual(red.b, modSetting.Color.b, 0.01);
      Assert.AreEqual(red.a, modSetting.Color.a, 0.01);
    }

  }
}