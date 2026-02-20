using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.StatusSystem;
using UnityEngine;

namespace TimberPhysics.Core {
  internal class StatusIconFixer : BaseComponent,
                                   IInitializableEntity {

    public void InitializeEntity() {
      var statusIconCycler = GetComponent<StatusIconCycler>();
      statusIconCycler._colliderTransform.GetComponent<SphereCollider>().isTrigger = true;
    }

  }
}