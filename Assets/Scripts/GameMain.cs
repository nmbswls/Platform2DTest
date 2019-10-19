using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameMain : MonoBehaviour
{

    private static GameMain mInstance;

    public ActGameMode gameMode;

    public SceneLoader sceneLoader;

    public void Release()
    {
        mInstance = null;
    }


    public static GameMain GetInstance()
    {
        if (mInstance == null)
        {
            Type type = typeof(GameMain);
            GameMain gameMain = (GameMain)FindObjectOfType(type);
            mInstance = gameMain;
            mInstance.Init();

        }
        return mInstance;
    }



    public void Init()
    {
        gameMode = new ActGameMode();
        gameMode.Init();
        sceneLoader = GetComponent<SceneLoader>();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SceneLoaded;
    }


    public string targetScene = string.Empty;

    public void SwitchScene(string target)
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
        targetScene = target;
     }

    public void SceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.name == "Loading")
        {
            sceneLoader.StartLoading(targetScene);
        }
        targetScene = string.Empty;
    }

}
