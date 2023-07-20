using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressBar : MonoBehaviour
{

    public int fullWidth;

    int totalTutorialPages;
    int progress;

    public RectTransform barTransform;

    public TMP_Text progressText;

    // Start is called before the first frame update
    public void updateTutorialProgress(int latestProgress, int totalPages)
    {
        //math yay
        float progressPercent = (float)latestProgress / (float)totalPages;
        float widthToSet = progressPercent * fullWidth;

        barTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthToSet);
        string progressTextValue = latestProgress.ToString() + "/" + totalPages.ToString();
        progressText.text = progressTextValue;

    }
}
