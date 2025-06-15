using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraController.Instance.SetPlayerFollowCamera();
    }
}
