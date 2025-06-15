using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //create a static instance of the class
    private static T instance;

    //create a public static property to get the instance
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        //check if the instance is not null and the game object is not null
        if (instance != null && this.gameObject != null)
        {
            //destroy the game object
            Destroy(this.gameObject);
        }
        else
        {
            //set the instance to this
            instance = (T)this;
        }

        //check if the game object has no parent
        if (!gameObject.transform.parent)
        {
            //dont destroy the game object on load
            DontDestroyOnLoad(gameObject);
        }
    }
}
