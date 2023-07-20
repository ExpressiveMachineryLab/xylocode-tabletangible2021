using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundBankDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public StyleHUD hud;
    public ElemColor color;
    List<SoundBank> sounds;
    public bool initialized;
    // Start is called before the first frame update
    void Awake()
    {
        //dropdown = this.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
    }

    public void AddSounds(SoundBank[] newSounds)
    {
        this.sounds = new List<SoundBank>();
        this.sounds.AddRange(newSounds);
        if (dropdown.isActiveAndEnabled)
        {

        }
        List<string> items = new List<string>();
        for (var i = 0; i < sounds.Count; i++)
        {
            SoundBank bank = sounds[i];
            items.Add(bank.bankName);
        }
        dropdown.AddOptions(items);
    }

    public void UpdateSound()
    {
        string current = dropdown.captionText.text;
        //SoundBank bank = GetSoundFromName(current);
        hud.SetSound(color, current);
        
    }

    public void SetItemFromName(string name)
    {
        for(int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == name)
            {
                dropdown.SetValueWithoutNotify(i);
                break;
            }
        }
    }

    public SoundBank GetSoundFromName(string name)
    {
        foreach (SoundBank bank in sounds)
        {
            if (bank.bankName == name)
            {
                return bank;
            }
        }
        return null;
    }
}
