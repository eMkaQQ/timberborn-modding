namespace Minimap.Settings {
  public class MinimapSettingsChangedEvent {

    public bool ChunkSizeChanged { get; }

    public MinimapSettingsChangedEvent(bool chunkSizeChanged) {
      ChunkSizeChanged = chunkSizeChanged;
    }

  }
}