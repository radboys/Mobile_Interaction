using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading;
using System;

public class MyScenesManager : MonoSingleton<MyScenesManager>
{
    private GameObject currentScene = null;

    public void LoadScene<T>(string name, UnityAction func = null) where T : BaseScene
    {
        currentScene?.GetComponent<BaseScene>().ExitScene();

        SceneManager.LoadScene(name);

        currentScene = new GameObject("CurrentSceneManager");
        currentScene.AddComponent<T>().EnterScene();

        func?.Invoke();
    }

    public void LoadSceneAsync<T>(string name, UnityAction func = null) where T : BaseScene
    {
       StartCoroutine(RootLoadSceneAsync<T>(name, func));
    }

    private IEnumerator RootLoadSceneAsync<T>(string name, UnityAction func) where T : BaseScene
    {
        currentScene?.GetComponent<BaseScene>().ExitScene();

        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            //TODO
            //EventCenter.GetInstance().EventTrigger("", ao.progress);
            yield return null;
        }


        currentScene = new GameObject("CurrentSceneManager");
        currentScene.AddComponent<T>().EnterScene();

        func?.Invoke();
    }
}
