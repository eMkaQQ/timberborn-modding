Disclaimer: although I work for Mechanistry and I'm one of the developers of Timberborn, this repository is my personal creation, made independently of my job. It shouldn't be considered official game content nor associated with Mechanistry in any way.

# Mod Settings

## Quick start guide

1. Install the Mod Settings via the [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3283831040) or by downloading the latest release from the [mod.io](https://mod.io/g/timberborn/m/mod-settings).
2. Setup your modding environment as described in the [official documentation](https://github.com/mechanistry/timberborn-modding/?tab=readme-ov-file#timberborn-modding-tools-and-examples).
3. Install the Mod Settings package via Unity's Package Manager by using `Install from git URL` option and pasting following URL: `https://github.com/eMkaQQ/timberborn-modding.git?path=/Assets/Mods/ModSettings/Scripts`.
   - Alternatively, you can just copy all DLLs from the mod's `Scripts` folder to your project.
   - If you're modding without Unity, you will need to reference the DLLs mentioned above in your project.
4. Paste the following code somewhere in your mod's codebase:
```csharp
  internal class ExampleModSettingsOwner : ModSettingsOwner {

    public ModSetting<int> ExampleSetting { get; } =
      new(0, ModSettingDescriptor.Create("ExampleSetting"));

    public ExampleModSettingsOwner(ISettings settings,
                                   ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                   ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    protected override string ModId => your_mod_id_here;

  }

  [Context("MainMenu")]
  internal class ExampleSettingConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ExampleModSettingsOwner>().AsSingleton();
    }

  }
```
It should result in a gear icon appearing in the main menu near your mod on the list, which will allow you to change the value of the `ExampleSetting` property.

## Examples
You can find more examples of various features of the ModSettings [here](https://github.com/eMkaQQ/timberborn-modding/tree/main/Assets/Mods/ModSettingsExamples).
The built mod of these examples can be downloaded from [here](https://github.com/eMkaQQ/timberborn-modding/blob/main/Assets/Mods/ModSettingsExamples/ModSettingsExamples.zip).