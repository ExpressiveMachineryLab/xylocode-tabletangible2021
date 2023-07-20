using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TE {
  public class TouchPointProvider : MonoBehaviour, ITouchPointProvider {

    public event Action<ICollection<Pointer>> TouchPointsUpdated;

    [NonSerialized]
    private Dictionary<int, Pointer> _touchIdToPointerMap = new Dictionary<int, Pointer>();

    [NonSerialized]
    private List<int> _touchIdsToRemove = new List<int>();

    [NonSerialized]
    private int _minIdValue;

    void Update() {
      var touches = Input.touches;
      foreach (var t in touches) {
        int rawId = t.fingerId;
        Pointer v;
        if (_touchIdToPointerMap.TryGetValue(rawId, out v)) {
          v.X = t.position.x;
          v.Y = t.position.y;
        }
        else {
          v = new Pointer {
            Id = rawId,
            X = t.position.x,
            Y = t.position.y
          };
          _touchIdToPointerMap[rawId] = v;
        }
      }

      foreach (var id in _touchIdToPointerMap.Keys) {
        if (touches.All(p => p.fingerId != id)) {
          _touchIdsToRemove.Add(id);
        }
      }

      foreach (var id in _touchIdsToRemove) {
        _touchIdToPointerMap.Remove(id);
      }

      _touchIdsToRemove.Clear();

      if (TouchPointsUpdated != null) {
        TouchPointsUpdated(_touchIdToPointerMap.Values);
      }
    }
  }
}