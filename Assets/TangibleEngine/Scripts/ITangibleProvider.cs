using System;
using System.Collections.Generic;

namespace TE {
  public interface ITangibleProvider : IDisposable {

    event Action Connected;
    event Action Disconnected;
    event Action FailedToConnect;
    event Action<List<Tangible>> TangiblesUpdated;
    event Action<List<Pattern>> PatternsUpdated;

    void SetPatterns(List<Pattern> patterns);
    void RequestPatterns();
    void UpdatePointers(ICollection<Pointer> pointers);
    void StartProvider();
  }

  public interface IInternalTangibleService {

  }

  public interface IInternalTangibleSimulator {

  }
}
