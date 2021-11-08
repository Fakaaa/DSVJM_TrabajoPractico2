using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MonoBehaviourSingletonScript;

public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
{
    void Start()
    {
        GameManager.Instance.OnResetGameplay += ReloadGameplay;
        GameManager.Instance.OnGoMainMenu += LoadSceneAsync;
        GameManager.Instance.OnQuitGame += QuitApplication;
    }

    private void LoadSceneAsync(string value)
    {
        IEnumerator AsyncLoad()
        {
            AsyncOperation val = SceneManager.LoadSceneAsync(value, LoadSceneMode.Additive);

            while (!val.isDone)
                yield return new WaitForEndOfFrame();

            if (val.allowSceneActivation)
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
        StartCoroutine(AsyncLoad());
    }
    private void ReloadGameplay(int secondsToWait)
    {
        IEnumerator WaitFewSecondsUtilLoadScene()
        {
            float t = 0;

            while(t < secondsToWait)
            {
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene("Gameplay");
            yield break;
        }

        StartCoroutine(WaitFewSecondsUtilLoadScene());
    }
    private void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
