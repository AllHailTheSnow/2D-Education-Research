using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            shouldLoadAfterFade = true;

            GameManager.Instance.fadingBetweenAreas = true;

            UIFade.Instance.FadeToBlack();

        }
    }
}
