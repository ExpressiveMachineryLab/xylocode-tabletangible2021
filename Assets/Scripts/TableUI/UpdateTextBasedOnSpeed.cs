using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTextBasedOnSpeed : MonoBehaviour
{
    TMP_Text text;
    string initText;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        initText = text.text;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnGUI()
    {
        text.text = initText.Replace("{0}",((double)gameManager.GetSpeedMultiplier()).ToString("F1"));
    }
}
