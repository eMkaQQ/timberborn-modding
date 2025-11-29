using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.Persistence;
using Timberborn.WorkSystem;
using Timberborn.WorldPersistence;

namespace SecondShift.Core {
  internal class WorkerPositionFixer : BaseComponent,
                                       IAwakableComponent,
                                       IPersistentEntity,
                                       IPostInitializableEntity {

    private static readonly ComponentKey WorkerPositionFixerKey = new("WorkerPositionFixer");
    private static readonly PropertyKey<int> WorkerPositionKey = new("WorkerPosition");
    private Worker _worker;
    private int? _loadedPosition;

    public void Awake() {
      _worker = GetComponent<Worker>();
    }

    public void Save(IEntitySaver entitySaver) {
      if (_worker.Workplace != null) {
        var index = _worker.Workplace.AssignedWorkers.IndexOf(_worker);
        if (index != -1) {
          var component = entitySaver.GetComponent(WorkerPositionFixerKey);
          component.Set(WorkerPositionKey, index);
        }
      }
    }

    public void Load(IEntityLoader entityLoader) {
      if (entityLoader.TryGetComponent(WorkerPositionFixerKey, out var component)) {
        _loadedPosition = component.Get(WorkerPositionKey);
      }
    }

    public void PostInitializeEntity() {
      if (_loadedPosition.HasValue && _worker.Workplace != null) {
        if (_worker.Workplace.MaxWorkers > _loadedPosition) {
          var currentIndex = _worker.Workplace.AssignedWorkers.IndexOf(_worker);
          if (currentIndex != _loadedPosition
              && currentIndex != -1
              && _loadedPosition.Value < _worker.Workplace._assignedWorkers.Count) {
            _worker.Workplace._assignedWorkers.RemoveAt(currentIndex);
            _worker.Workplace._assignedWorkers.Insert(_loadedPosition.Value, _worker);
          }
        }
      }
    }

  }
}