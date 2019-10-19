using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.UIElements;

public class SceneLoader : MonoBehaviour
{

    public void StartLoading(string targetScene)
    {


        StartCoroutine(LoadTargetScene(targetScene));

    }


    //进度条的数值
    private float progressValue;
    private Slider slider;

    float minWaitTime = 0.3f;
    float timer = 0;

    IEnumerator LoadTargetScene(string targetScene)
    {

        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene,LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {


            if (async.progress >= 0.9f)
            {
                //progress.text = "按任意键继续";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

}
