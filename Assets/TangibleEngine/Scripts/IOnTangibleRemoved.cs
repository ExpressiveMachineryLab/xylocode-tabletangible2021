namespace TE {
  public interface IOnTangibleRemoved {
    /// <summary>
    /// Called when a tangible has been removed
    /// </summary>
    /// <param name="t">The soon to be deleted tangible instance</param>
    void OnTangibleRemoved(Tangible t);
  }
}