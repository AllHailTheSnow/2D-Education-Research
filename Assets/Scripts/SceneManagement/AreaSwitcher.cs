using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    public AreaEntrance entrance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    private void Start()
    {
        if(entrance != null)
        {
            entrance.transitionName = sceneTransitionName;
        }
    }

    private void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            shouldLoadAfterFade = true;

            GameManager.Instance.fadingBetweenAreas = true;

            UIFade.Instance.FadeToBlack();

            PlayerController.Instance.areaTransitionName = sceneTransitionName;
        }
    }
}
