using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStarter : MonoBehaviour
{
    [Header("Potential Battle types")]
    public BattleType[] potentialBattles;
    public SpriteRenderer battleBG;
    public int battleTrack;

    [Header("Activation requirements")]
    public bool activateOnEnter;
    public bool activateOnStay;
    public bool activateOnExit;
    public bool deactivateAfterStarting;
    public bool cannotFlee;
    private bool inArea;
    public float timeBetweenBattles = 10f;
    private float betweenBattleCounter;

    [Header("Quest")]
    public bool shouldCompleteQuest;
    public string questToComplete;

    private void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    private void Update()
    {
        if(inArea && PlayerController.Instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);

                StartCoroutine(StartBattleRoutine());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if(activateOnEnter)
            {
                StartCoroutine(StartBattleRoutine());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattleRoutine());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleRoutine()
    {
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.Instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.Instance.rewardEXP = potentialBattles[selectedBattle].rewardExp;
        BattleManager.Instance.battleBG.sprite = battleBG.sprite;
        BattleManager.Instance.battleTrack = battleTrack;

        yield return new WaitForSeconds(1.5f);

        BattleManager.Instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);
        yield return new WaitForSeconds(0.5f);
        UIFade.Instance.FadeFromBlack();

        if(deactivateAfterStarting)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

        BattleRewards.Instance.markQuestComplete = shouldCompleteQuest;
        BattleRewards.Instance.questToMark = questToComplete;
    }
}
