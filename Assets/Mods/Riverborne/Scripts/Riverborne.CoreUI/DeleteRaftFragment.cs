using Riverborne.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.Coordinates;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.InputSystem;
using Timberborn.InputSystemUI;
using Timberborn.RecoveredGoodSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class DeleteRaftFragment : IEntityPanelFragment {

    private static readonly string DeleteObjectKey = "DeleteObject";
    private static readonly string DeletePromptKey = "Riverborne.Raft.DeletePrompt";
    private static readonly string SkipDeleteConfirmationKey = "SkipDeleteConfirmation";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly DialogBoxShower _dialogBoxShower;
    private readonly InputService _inputService;
    private readonly EntityService _entityService;
    private readonly BindableButtonFactory _bindableButtonFactory;
    private readonly RecoveredGoodStackSpawner _recoveredGoodStackSpawner;
    private BindableButton _deleteButton;
    private VisualElement _root;
    private Raft _raft;

    public DeleteRaftFragment(VisualElementLoader visualElementLoader,
                              DialogBoxShower dialogBoxShower,
                              InputService inputService,
                              EntityService entityService,
                              BindableButtonFactory bindableButtonFactory,
                              RecoveredGoodStackSpawner recoveredGoodStackSpawner) {
      _visualElementLoader = visualElementLoader;
      _dialogBoxShower = dialogBoxShower;
      _inputService = inputService;
      _entityService = entityService;
      _bindableButtonFactory = bindableButtonFactory;
      _recoveredGoodStackSpawner = recoveredGoodStackSpawner;
    }

    public VisualElement InitializeFragment() {
      _root = _visualElementLoader.LoadVisualElement("Common/EntityPanel/DeleteObjectFragment");
      _deleteButton = _bindableButtonFactory.Create(_root.Q<Button>("Button"), DeleteObjectKey,
                                                    DeleteCallback);
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity) {
      _raft = entity.GetComponent<Raft>();
      if (_raft) {
        _root.ToggleDisplayStyle(true);
        _deleteButton.Bind();
      }
    }

    public void ClearFragment() {
      if (_raft) {
        _raft = null;
        _deleteButton.Unbind();
      }
      _root.ToggleDisplayStyle(false);
    }

    public void UpdateFragment() {
    }

    private void DeleteCallback() {
      if (_inputService.IsKeyHeld(SkipDeleteConfirmationKey)) {
        DeleteRaft();
      } else {
        ShowDialogBox();
      }
    }

    private void ShowDialogBox() {
      _dialogBoxShower.Create()
          .SetLocalizedMessage(DeletePromptKey)
          .SetConfirmButton(DeleteRaft)
          .SetDefaultCancelButton()
          .Show();
    }

    private void DeleteRaft() {
      if (_raft) {
        var cargo = _raft.Inventory.Stock;
        var coordinates = CoordinateSystem.WorldToGridInt(_raft.Transform.position);
        _entityService.Delete(_raft);
        _recoveredGoodStackSpawner.AddAwaitingGoods(coordinates, cargo);
      }
    }

  }
}