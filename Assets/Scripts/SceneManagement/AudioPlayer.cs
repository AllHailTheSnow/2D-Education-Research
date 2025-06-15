using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public int musicToPlay;
    private bool musicStarted;

    private void Update()
    {
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.Instance.PlayBGM(musicToPlay);
        }
    }
}
