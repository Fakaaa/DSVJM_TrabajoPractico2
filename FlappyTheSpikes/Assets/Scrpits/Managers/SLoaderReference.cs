using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLoaderReference : MonoBehaviour
{
    private SceneLoader reference;

    void Start()
    {
        reference = SceneLoader.Instance;
    }

    public void LoadSceneAsync(string scene)
    {
        reference.LoadSceneAsyncWithLoadScreen(scene);
    }
}
