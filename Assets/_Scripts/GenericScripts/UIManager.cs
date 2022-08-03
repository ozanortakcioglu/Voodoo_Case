using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;
using System;


[System.Serializable]
public class UIPanels : SerializableDictionaryBase<PanelNames, UIPanelAndSetup> { }

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIPanels UIPanelsDictionary;

    [Header("MAIN MENU CANVAS ITEMS")]
    public GameObject settingsBar;
    public Image vibrationImage;
    public Image soundImage;
    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    [Header("IN GAME PANEL ITEMS")]
    public GameObject gemIcon;
    public TMPro.TMP_Text gemCount;
    public TMPro.TMP_Text levelText;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Custom Events

    public void ToggleSettings()
    {
        if (settingsBar.activeInHierarchy)
            settingsBar.SetActive(false);
        else
            settingsBar.SetActive(true);
    }
    public void ToggleVibration()
    {
        GameManager.Instance.VibrationOn = !GameManager.Instance.VibrationOn;
        vibrationImage.sprite = GameManager.Instance.VibrationOn ? vibrationOn : vibrationOff;
    }
    public void ToggleSound()
    {
        GameManager.Instance.SoundOn = !GameManager.Instance.SoundOn;
        soundImage.sprite = GameManager.Instance.SoundOn ? soundOn : soundOff;
    }

    public void ReloadOnClick()
    {
        GameManager.Instance.ReloadLevel();
    }

    public void NextLevelOnClick()
    {
        GameManager.Instance.AdvanceLevel();
    }
    #endregion


    #region On Panel Opened Actions

    public void OnInGamePanelOpened()
    {
        Debug.Log("Setting Up InGame Panel");
        gemCount.text = Utility.FormatBigNumbers(GameManager.Instance.GemCount);
        levelText.text = "Level" + (GameManager.Instance.Level + 1);
    }

    public void OnMainMenuPanelOpened()
    {
        Debug.Log("Setting Up MainMenu Panel");
        settingsBar.SetActive(false);
        vibrationImage.sprite = GameManager.Instance.VibrationOn ? vibrationOn : vibrationOff;
        soundImage.sprite = GameManager.Instance.SoundOn ? soundOn : soundOff;

    }
    #endregion


    #region Panel Functions

    public void OpenPanel(string panel)
    {
        PanelNames panelName;
        if (Enum.TryParse<PanelNames>(panel, out panelName))
            OpenPanel(panelName);
        else
            Debug.LogWarning("Did not find panel: " + panel);
    }

    public void OpenPanel(PanelNames panelName, bool closeOtherPanels)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {

            if (closeOtherPanels)
            {
                CloseAllPanels();
            }

            OpenPanel(panelName);

        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }

    }


    public void OpenPanel(PanelNames[] names)
    {
        foreach (PanelNames panelName in names)
            OpenPanel(panelName);
    }

    public void OpenPanel(PanelNames name, bool closeOtherPanels, float delay)
    {
        if (closeOtherPanels)
            CloseAllPanels();

        StartCoroutine(AddDelay(delay, () => { OpenPanel(name, closeOtherPanels); }));
    }

    public void OpenPanel(PanelNames panelName)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {
            foreach (var item in UIPanelsDictionary[panelName].UIPanel.GetComponentsInChildren<TweenAnimation>())
            {
                item.Play();
            }

            panelToOpen.UIPanel.SetActive(true);
            panelToOpen.UIPanelSetup?.Invoke();
        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }

    }

    public void ClosePanel(string panel)
    {
        PanelNames panelName;
        if (!Enum.TryParse<PanelNames>(panel, out panelName))
        {
            Debug.LogWarning("No enum for string: " + panel);
            return;
        }

        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }

    public void ClosePanel(PanelNames panelName)
    {
        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }


    void CloseAllPanels()
    {
        foreach (PanelNames panelName in UIPanelsDictionary.Keys)
            ClosePanel(panelName);
    }

    IEnumerator AddDelay(float xSeconds, UnityAction Action)
    {
        yield return new WaitForSecondsRealtime(xSeconds);
        Action();
    }

    #endregion

}

[System.Serializable]
public class UIPanelAndSetup
{
    public GameObject UIPanel;
    public UnityEvent UIPanelSetup;
}

