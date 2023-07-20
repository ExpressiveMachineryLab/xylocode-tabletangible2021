namespace TE {
  public interface IOnTangibleUpdated {
    /// <summary>
    /// Called when a tangible has been updated
    /// </summary>
    /// <param name="t">The tangible instance</param>
    void OnTangibleUpdated(Tangible t);
  }
}