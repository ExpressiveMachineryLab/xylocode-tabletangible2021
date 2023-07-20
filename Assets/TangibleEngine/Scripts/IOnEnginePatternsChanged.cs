using System.Collections.Generic;

namespace TE {
  public interface IOnEnginePatternsChanged {
    void OnEnginePatternsChanged(List<Pattern> patterns);
  }
}