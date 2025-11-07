using System.IO;
using System.Linq;
using Timberborn.FileSystem;
using Timberborn.Modding;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace Mods.TextureExtractor.Scripts {
  internal class NormalMapExtractor : ILoadableSingleton {

    private static readonly string ModId = "eMka.TextureExtractor";
    private static readonly string NormalsDirectory = "ExtractedNormals";
    private readonly ModRepository _modRepository;
    private readonly IFileService _fileService;
    private string _targetDirectory;

    public NormalMapExtractor(ModRepository modRepository,
                              IFileService fileService) {
      _modRepository = modRepository;
      _fileService = fileService;
    }

    public void Load() {
      var mod = _modRepository.EnabledMods.Single(mod => mod.Manifest.Id == ModId);
      _targetDirectory = Path.Combine(mod.ModDirectory.OriginPath, NormalsDirectory);
      if (_fileService.DirectoryExistsAndNotEmpty(_targetDirectory, "")) {
        _fileService.DeleteDirectory(_targetDirectory);
      }
      _fileService.CreateDirectory(_targetDirectory);
      ExtractNormalsToDisk();
    }

    private void ExtractNormalsToDisk() {
      var textures = Resources.LoadAll<Texture2D>("");

      var saved = 0;
      foreach (var texture in textures) {
        if (LooksLikeNormal(texture)) {
          var png = EncodeTextureSafely(texture);
          if (png != null) {
            var safeName = MakeSafeFilename(texture.name) + ".png";
            var path = Path.Combine(_targetDirectory, safeName);
            File.WriteAllBytes(path, png);
            saved++;
          }
        }
      }

      Debug.Log($"Saved {saved} textures to: {_targetDirectory}");
    }

    private static bool LooksLikeNormal(Texture2D tex) {
      var n = tex.name.ToLowerInvariant();
      return n.EndsWith("_n")
             || n.EndsWith("_normal")
             || n.Contains("_normal_")
             || n.Contains("_n_")
             || n.Contains("_n.");
    }

    private static string MakeSafeFilename(string textureName) {
      foreach (var invalidCharacter in Path.GetInvalidFileNameChars()) {
        textureName = textureName.Replace(invalidCharacter, '_');
      }
      return textureName;
    }

    private static byte[] EncodeTextureSafely(Texture2D source) {
      var renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0,
                                                     RenderTextureFormat.ARGB32,
                                                     RenderTextureReadWrite.Linear);
      var prev = RenderTexture.active;

      try {
        Graphics.Blit(source, renderTexture);
        RenderTexture.active = renderTexture;

        var tmp = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false, true);
        tmp.ReadPixels(new(0, 0, source.width, source.height), 0, 0, false);
        tmp.Apply(false, false);

        var pixels = tmp.GetPixels32();
        DecodeInPlace(pixels);

        var outTex = new Texture2D(source.width, source.height, TextureFormat.RGB24, false, true);
        outTex.SetPixels32(pixels);
        outTex.Apply(false, false);

        return outTex.EncodeToPNG();
      } finally {
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(renderTexture);
      }
    }

    private static void DecodeInPlace(Color32[] px) {
      for (var i = 0; i < px.Length; i++) {
        var c = px[i];
        
        var nx = c.a / 255f * 2f - 1f;
        var ny = c.g / 255f * 2f - 1f;

        var nz2 = 1f - nx * nx - ny * ny;
        var nz = nz2 > 0f ? Mathf.Sqrt(nz2) : 0f;

        var r = (byte) Mathf.Clamp(Mathf.RoundToInt((nx * 0.5f + 0.5f) * 255f), 0, 255);
        var g = (byte) Mathf.Clamp(Mathf.RoundToInt((ny * 0.5f + 0.5f) * 255f), 0, 255);
        var b = (byte) Mathf.Clamp(Mathf.RoundToInt((nz * 0.5f + 0.5f) * 255f), 0, 255);

        px[i] = new(r, g, b, 0);
      }
    }

  }
}