using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftDockPreview : BaseComponent,
                                   IAwakableComponent,
                                   IUnfinishedStateListener,
                                   IFinishedStateListener,
                                   IPreviewStateListener {

    private GameObject _staticRaft;
    private GameObject _previewRaft;

    public void Awake() {
      var spec = GetComponent<RaftDockSpec>();
      _staticRaft = GameObject.FindChild(spec.StaticRaftName).gameObject;
      _previewRaft = GameObject.FindChild(spec.PreviewRaftName).gameObject;
    }

    public void OnEnterPreviewState() {
      _staticRaft.SetActive(false);
      _previewRaft.SetActive(true);
    }

    public void OnEnterUnfinishedState() {
      _staticRaft.SetActive(false);
      _previewRaft.SetActive(true);
    }

    public void OnExitUnfinishedState() {
    }

    public void OnEnterFinishedState() {
      _staticRaft.SetActive(true);
      _previewRaft.SetActive(false);
    }

    public void OnExitFinishedState() {
    }

  }
}