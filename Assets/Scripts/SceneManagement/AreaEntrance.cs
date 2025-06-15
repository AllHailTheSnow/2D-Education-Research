using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;

    private void Start()
    {
        if(transitionName == PlayerController.Instance.areaTransitionName)
        {
            PlayerController.Instance.transform.position = transform.position;
            CameraController.Instance.SetPlayerFollowCamera();
        }

        UIFade.Instance.FadeFromBlack();
        GameManager.Instance.fadingBetweenAreas = false;
    }
}
