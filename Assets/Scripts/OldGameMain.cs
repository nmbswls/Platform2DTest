using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class OldGameMain : MonoBehaviour
{

    private static OldGameMain mInstance;

    public ActGameMode gameMode;

    public SceneLoader sceneLoader;

    public void Release()
    {
        mInstance = null;
    }


    //存储scene 的信息

    public static OldGameMain GetInstance()
    {
        if (mInstance == null)
        {
            Type type = typeof(OldGameMain);
            OldGameMain gameMain = (OldGameMain)FindObjectOfType(type);
            mInstance = gameMain;
            mInstance.Init();

        }
        return mInstance;
    }



    public void Init()
    {
        Application.targetFrameRate = 60;

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
        else
        {
            gameMode.ChangeMap();
        }
        targetScene = string.Empty;
    }

}
