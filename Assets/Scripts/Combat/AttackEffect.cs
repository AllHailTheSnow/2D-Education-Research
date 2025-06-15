using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float effectLength;
    public int soundEffect;

    private void Start()
    {
        // Play the sound effect
        AudioManager.Instance.PlaySFX(soundEffect);
    }

    private void Update()
    {
        //destroy the object after the effect length
        Destroy(gameObject, effectLength);
    }
}
