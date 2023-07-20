using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChordIndicator : MonoBehaviour
{
    public GameObject plus;
    public GameObject minus;
    public GameObject none;

    public void SetIndicator(int val)
    {
        if (val > 0)
        {
            plus.SetActive(true);
            minus.SetActive(false);
            none.SetActive(false);
        }
        else if (val < 0)
        {
            plus.SetActive(false);
            minus.SetActive(true);
            none.SetActive(false);
        }
        else
        {
            plus.SetActive(false);
            minus.SetActive(false);
            none.SetActive(true);
        }
    }
}
