using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [Header("Basics")]
    public bool isPlayer;
    public string[] movesAvailable;

    [Header("Character Info")]
    public string characterName;
    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int strength;
    public int defense;
    public int wpnPwr;
    public int armPwr;
    public bool hasDied;

    [Header("Sprites")]
    public SpriteRenderer sprite;
    public Sprite deadSprite;
    public Sprite aliveSprite;
    public Animator anim;

    [Header("Fade Effects")]
    public bool shouldFade;
    public float fadeSpeed = 1f;

    private void Update()
    {
        if(shouldFade)
        {
            sprite.color = new Color(Mathf.MoveTowards(sprite.color.r, 1f, fadeSpeed * Time.deltaTime),
                Mathf.MoveTowards(sprite.color.g, 0f, fadeSpeed * Time.deltaTime),
                Mathf.MoveTowards(sprite.color.b, 0f, fadeSpeed * Time.deltaTime),
                Mathf.MoveTowards(sprite.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(sprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
