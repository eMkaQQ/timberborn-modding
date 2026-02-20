using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;

namespace Riverborne.CoreUI {
  public class RaftDescriber : BaseComponent,
                               IAwakableComponent,
                               IEntityDescriber {

    private readonly ILoc _loc;
    private LabeledEntitySpec _labeledEntitySpec;

    public RaftDescriber(ILoc loc) {
      _loc = loc;
    }

    public void Awake() {
      _labeledEntitySpec = GetComponent<LabeledEntitySpec>();
    }

    public IEnumerable<EntityDescription> DescribeEntity() {
      yield return EntityDescription.CreateTextSection(_loc.T(_labeledEntitySpec.DescriptionLocKey),
                                                       -1);
      yield return EntityDescription.CreateFlavorSection(
          _loc.T(_labeledEntitySpec.FlavorDescriptionLocKey), 1);
    }

  }
}