using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public int GemCount
    {
        get
        {
            return PlayerPrefs.GetInt("GemCount", 0);
        }
        set
        {

            PlayerPrefs.SetInt("GemCount", value);
            PlayerPrefs.Save();
            UIManager.Instance.gemCount.text = Utility.FormatBigNumbers(value);
        }
    }

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
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void AdvanceLevel()
    {
        //level++
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
