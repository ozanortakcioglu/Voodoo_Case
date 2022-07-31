using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


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
            UIManager.Instance.gemCount.text = value.ToString();
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
