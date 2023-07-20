using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GlobalTableUI : MonoBehaviour
{
    public static GlobalTableUI instance;
    GameManager gameManager;
    [Header("Pausing")]
    public bool paused;
    public CanvasGroup playPauseGroup;
    public GameObject pauseNotif;
    public TMP_Text pauseText;
    public GameObject playNotif;
    public TMP_Text playText;
    float fadeT = 0f;
    public float fadeoutTime = 5f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    float timeScaleAtStart;
    [Header("Speed")]
    public TMP_Text speedText;
    float speed;
    public float speedMultiplier = 5f;
    [Header("Reset & Clear")]
    public GameObject resetDialog;
    public GameObject clearDialog;
    [Header("Sound Bank")]
    public StyleHUD soundBank;
    public CanvasGroup overlay;
    public Volume volume;
    bool initIcons;
    private void Awake()
    {
        instance = this;
        initIcons = false;
    }

    private void Start()
    {
        timeScaleAtStart = Time.timeScale;
        gameManager = FindObjectOfType<GameManager>();
    }
    void FadeOutNotifs()
    {
        fadeT = Mathf.MoveTowards(fadeT, 0f, Time.deltaTime / fadeoutTime);
        playPauseGroup.alpha = fadeCurve.Evaluate(fadeT);
        pauseText.alpha = fadeCurve.Evaluate(fadeT);
        playText.alpha = fadeCurve.Evaluate(fadeT);
    }

    private void Update()
    {
        if (fadeT > 0f)
        {
            FadeOutNotifs();
        }
        if (!initIcons)
        {
            initIcons = true;
            //soundBank.SetStyle(0);
            UpdateSoundBankUIInstances();
        }
        if (speed != gameManager.GetSpeedMultiplier())
        {
            speed = gameManager.GetSpeedMultiplier();
            speedText.text = (speed / speedMultiplier).ToString("F1") + "x";
        }
    }
    public void PauseRequest(TableUIController table)
    {
        RotateToRequester(table);
        fadeT = 1f;

        paused = !paused;

        foreach (TableUIController instance in TableUIController.instances)
        {
            instance.pauseImage.sprite = paused ? instance.pausedSprite : instance.playSprite;
        }
        pauseNotif.SetActive(paused);
        playNotif.SetActive(!paused);
        speedText.gameObject.SetActive(false);
        Time.timeScale = paused ? 0f : timeScaleAtStart;

        if (paused)
        {
            StartCoroutine("StartPauseDarkening");
            StopCoroutine("StopPauseDarkening");
        }
        else
        {
            StopCoroutine("StartPauseDarkening");
            StartCoroutine("StopPauseDarkening");
        }
    }


    void RotateToRequester(TableUIController table)
    {
        this.transform.rotation = Quaternion.identity;
        this.transform.rotation = Quaternion.AngleAxis(table.rotation, Vector3.forward);
    }

    public void ResetRequest(TableUIController table)
    {
        RotateToRequester(table);
        resetDialog.SetActive(true);
        clearDialog.SetActive(false);
    }

    public void ClearRequest(TableUIController table)
    {
        RotateToRequester(table);
        resetDialog.SetActive(false);
        clearDialog.SetActive(true);
    }

    public void Restart()
    {
        resetDialog.SetActive(false);
        clearDialog.SetActive(false);
        gameManager.ResetBalls();
    }

    public void Clear()
    {
        resetDialog.SetActive(false);
        clearDialog.SetActive(false);
        gameManager.ResetAllButEmitters();
    }

    public void OpenSoundBankRequest(TableUIController table)
    {
        RotateToRequester(table);
        soundBank.footer.SetActive(true);
    }

    public void UpdateSoundBankUIInstances()
    {
        foreach (TableUIController instance in TableUIController.instances)
        {
            instance.soundIcons.bank = soundBank.GetCurrentStyle().styleName;
            instance.soundIcons.UpdateIcons();
        }
    }

    public static bool IsControlOpen()
    {
        return instance.resetDialog.activeInHierarchy || instance.clearDialog.activeInHierarchy;
    }

    public static bool IsSoundBankOpen()
    {
        return instance.soundBank.footer.activeInHierarchy;
    }

    public void OnSpeedChange(TableUIController table)
    {
        RotateToRequester(table);
        pauseNotif.SetActive(false);
        playNotif.SetActive(false);
        speedText.gameObject.SetActive(true);
        fadeT = 1f;
    }

    IEnumerator StartPauseDarkening()
    {
        float t = 0f;
        float clock = 0f;
        float timeToDarken = 0.25f;
        while (clock < timeToDarken)
        {

            clock += 0.05f;
            t = Mathf.Clamp01(clock / timeToDarken);
            volume.weight = Mathf.Lerp(0f, 1f, t);
            overlay.alpha = Mathf.Lerp(0f, 0.5f, t);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
    }

    IEnumerator StopPauseDarkening()
    {
        float t = 0f;
        float clock = 0f;
        float timeToDarken = 0.25f;
        while (clock < timeToDarken)
        {

            clock += 0.05f;
            t = Mathf.Clamp01(clock / timeToDarken);
            volume.weight = Mathf.Lerp(0f, 1f, 1f - t);
            overlay.alpha = Mathf.Lerp(0f, 0.5f, 1f - t);
            yield return new WaitForSecondsRealtime(0.05f);
        }

    }
}
