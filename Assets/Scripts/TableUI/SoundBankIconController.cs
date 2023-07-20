using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundBankIconController : MonoBehaviour
{
    public bool updateOnStart = true;
    public TMP_Text bankName;
    public Image spriteR;
    public Image spriteY;
    public Image spriteB;
    public Image spriteG;

    public string bank;
    public void Start()
    {
        bank = bankName.text;
        if (updateOnStart) UpdateIcons();
    }

    public void UpdateIcons()
    {
        bankName.text = bank;
        if (SoundToIconHelper.bankToIconNames.TryGetValue(bank.ToLower(), out string[] iconNames))
        {
            if (SoundToIconHelper.iconNamesToSprite.TryGetValue(iconNames[0], out Sprite r))
            {
                spriteR.sprite = r;
            }
            if (SoundToIconHelper.iconNamesToSprite.TryGetValue(iconNames[1], out Sprite y))
            {
                spriteY.sprite = y;
            }
            if (SoundToIconHelper.iconNamesToSprite.TryGetValue(iconNames[2], out Sprite b))
            {
                spriteB.sprite = b;
            }
            if (SoundToIconHelper.iconNamesToSprite.TryGetValue(iconNames[3], out Sprite g))
            {
                spriteG.sprite = g;
            }
        }
    }
}
