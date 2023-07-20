using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableUIController : MonoBehaviour
{
    public static List<TableUIController> instances = new List<TableUIController>();
    GameManager gameManager;
    public float rotation = 0f;
    [Header("Global Speed Multiplier")]
    public float speed;
    public float speedMultiplier = 5f;
    public Slider speedSlider;
    public TMP_Text speedText;
    float speedTimer;
    [Header("Volume")]
    public float volume;
    public Slider volumeSlider;
    public Image volumeImage;
    [Space(5)]
    public Sprite volumeHi;
    public Sprite volumeLo;
    public Sprite volumeMute;
    float volumeTimer;
    [Header("Pause")]
    public Image pauseImage;
    [Space(5)]
    public Sprite pausedSprite;
    public Sprite playSprite;
    float timeScaleAtStart;
    [Header("Sound Bank")]
    public SoundBankIconController soundIcons;
    public GameObject soundBankFooter;
    [Header("Backgrounds")]
    public GameObject bgDividers;
    public GameObject bgControl;
    public GameObject bgGlobalVar;
    public GameObject bgSoundBank;
    // Start is called before the first frame update
    void Awake()
    {
        instances.Add(this);
        gameManager = FindObjectOfType<GameManager>();
        timeScaleAtStart = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != gameManager.GetSpeedMultiplier())
        {
            speed = gameManager.GetSpeedMultiplier();
            speedText.text = (speed / speedMultiplier).ToString("F1") + "x";
        }

        if (speedSlider.isActiveAndEnabled && speedTimer < 0f && speedSlider.value != gameManager.GetSpeedMultiplier())
        {
            speedSlider.value = gameManager.GetSpeedMultiplier();
            speedTimer = 0.1f;
        }
        else
        {
            if (speedTimer > 0) speedTimer -= Time.deltaTime;
        }

        if (volume != AudioListener.volume)
        {
            volume = AudioListener.volume;
            if (volume > 0.5f)
            {
                volumeImage.sprite = volumeHi;
            }
            else if (volume > 0f)
            {
                volumeImage.sprite = volumeLo;
            }
            else
            {
                volumeImage.sprite = volumeMute;
            }
        }

        if (volumeSlider.isActiveAndEnabled && volumeTimer < 0f && volumeSlider.value != AudioListener.volume)
        {
            volumeSlider.value = AudioListener.volume;
            volumeTimer = 0.1f;
        }
        else
        {
            if (volumeTimer > 0)
            {
                volumeTimer -= Time.deltaTime;
            }

        }

        bool controlOpen = GlobalTableUI.IsControlOpen();
        bool globalvarOpen = speedSlider.gameObject.activeInHierarchy || volumeSlider.gameObject.activeInHierarchy;
        bool soundbankOpen = soundBankFooter.activeInHierarchy;
        bool showDividers = controlOpen || globalvarOpen || soundbankOpen;

        if (bgDividers.activeSelf != showDividers) bgDividers.SetActive(showDividers);
        if (bgControl.activeSelf != controlOpen) bgControl.SetActive(controlOpen);
        if (bgGlobalVar.activeSelf != globalvarOpen) bgGlobalVar.SetActive(globalvarOpen);
        if (bgSoundBank.activeSelf != soundbankOpen) bgSoundBank.SetActive(soundbankOpen);
    }

    public void UpdateSpeedFromSlider()
    {
        if (gameManager == null) return;
        gameManager.SetSpeedMultiplier(speedSlider.value);
        speedTimer = 1f;
        GlobalTableUI.instance.OnSpeedChange(this);
    }

    public void UpdateVolumeFromSlider()
    {
        AudioListener.volume = volumeSlider.value;
        volumeTimer = 1f;
    }

    public void SetPauseSprites(bool paused)
    {
        paused = !paused;
        
        foreach (TableUIController instance in instances)
        {
            instance.pauseImage.sprite = paused ? pausedSprite : playSprite;
        }
        Time.timeScale = paused ? 0f : timeScaleAtStart;
    }

    public void RequestPause()
    {
        GlobalTableUI.instance.PauseRequest(this);
    }

    public void RequestReset()
    {
        GlobalTableUI.instance.ResetRequest(this);
    }

    public void RequestClear()
    {
        GlobalTableUI.instance.ClearRequest(this);
    }

    public void RequestSoundBank()
    {
        GlobalTableUI.instance.OpenSoundBankRequest(this);
    }
}
