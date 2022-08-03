using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] levels;

    #region Generic Properties
    private bool _soundOn;
    private bool _vibrationOn;

    [HideInInspector]
    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt(PlayerPrefStrings.LEVEL, 0);
        }
        set
        {

            PlayerPrefs.SetInt(PlayerPrefStrings.LEVEL, value);
            PlayerPrefs.Save();
        }
    }

    [HideInInspector]
    public int GemCount
    {
        get
        {
            return PlayerPrefs.GetInt(PlayerPrefStrings.GEM, 0);
        }
        set
        {

            PlayerPrefs.SetInt(PlayerPrefStrings.GEM, value);
            PlayerPrefs.Save();
            UIManager.Instance.gemCount.text = Utility.FormatBigNumbers(value);
        }
    }

    public bool SoundOn
    {
        get
        {
            if (PlayerPrefs.HasKey(PlayerPrefStrings.SOUND))
                return PlayerPrefs.GetInt(PlayerPrefStrings.SOUND) != 0;
            else
            {
                SoundOn = true;
                return true;
            }
        }
        set
        {
            PlayerPrefs.SetInt(PlayerPrefStrings.SOUND, (value ? 1 : 0));
            PlayerPrefs.Save();
            _soundOn = value;
            AudioListener.pause = !_soundOn;
        }
    }

    public bool VibrationOn
    {
        get
        {
            if (PlayerPrefs.HasKey(PlayerPrefStrings.VIBRATION))
                return PlayerPrefs.GetInt(PlayerPrefStrings.VIBRATION) != 0;
            else
            {
                VibrationOn = true;
                return true;
            }

        }
        set
        {
            PlayerPrefs.SetInt(PlayerPrefStrings.VIBRATION, (value ? 1 : 0));
            PlayerPrefs.Save();
            _vibrationOn = value;
            Taptic.tapticOn = _vibrationOn;

        }
    }
    #endregion


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

    private void Start()
    {
        Application.targetFrameRate = 60;
        Taptic.tapticOn = VibrationOn;
        AudioListener.pause = !SoundOn;

        LoadLevel();
    }

    private void LoadLevel()
    {
        foreach (var item in levels)
        {
            item.SetActive(false);
        }
        levels[Level % levels.Length].SetActive(true);
        UIManager.Instance.OpenPanel(PanelNames.MainMenu, true);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void AdvanceLevel()
    {
        Level++;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
