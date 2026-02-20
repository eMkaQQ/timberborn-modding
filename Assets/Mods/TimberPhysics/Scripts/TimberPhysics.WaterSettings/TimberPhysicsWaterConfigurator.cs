using Bindito.Core;

namespace TimberPhysics.WaterSettings {
  [Context("Game")]
  [Context("MainMenu")]
  internal class TimberPhysicsWaterConfigurator : Configurator {

    protected override void Configure() {
      Bind<FloatingObjectSettingOwner>().AsSingleton();
    }

  }
}