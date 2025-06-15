using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagment : Singleton<SceneManagment>
{
    public string SceneTransitionName { get; private set; }

    public void SetSceneTransitioName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
}
