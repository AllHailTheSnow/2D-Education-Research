using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Battle State")]
    public bool battleActive;
    public int currentTurn;
    public int currentActiveBattler;
    public bool turnWaiting;
    public int chanceToFlee = 35;
    public bool cannotFlee;
    public int battleTrack;
    private bool fleeing;

    [Header("Battle Scene")]
    public GameObject battleScene;
    public SpriteRenderer battleBG;

    [Header("Battle Positions")]
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    [Header("Battle Characters")]
    public BattleCharacter[] playerPrefabs;
    public BattleCharacter[] enemyPrefabs;
    public List<BattleCharacter> activeBattlers = new();

    [Header("Battle Moves")]
    public BattleMoves[] movesList;
    public GameObject enemyAttackEffects;
    public DamageController DamageNumber;

    [Header("Item Menu")]
    public GameObject itemMenu;
    public ItemButton[] itemButtons;
    public GameObject itemTargetMenu;
    public TMP_Text[] itemCharChoiceNames;
    public string selectedItem;
    public Item activeItem;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text useButtonText;
    public GameObject healPrefab;

    [Header("UI Elements")]
    public GameObject uiButtonsHolder;
    public GameObject characterStatsHolder;
    public TMP_Text[] playerName;
    public TMP_Text[] playerHP;
    public TMP_Text[] playerMP;
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;
    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    [Header("Rewards")]
    public int rewardEXP;
    public string[] rewardItems;
    public BattleNotification battleNotice;

    public string gameOverScene;

    public void Update()
    {
#if UNITY_EDITOR
        //Editor testing starts battle when pressing M key
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            BattleStart(new string[] { "Centipede", "Baby Spider" }, false);
        }
#endif
        //Check if the battle is active and if the current turn is waiting for input
        if (battleActive)
        {
            if(turnWaiting)
            {
                //Check if the current turn is a player
                if (activeBattlers[currentTurn].isPlayer)
                {
                    //show the UI buttons
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    //if the current turn is an enemy then hide the UI buttons
                    uiButtonsHolder.SetActive(false);

                    //start the enemy move routine
                    StartCoroutine(EnemyMoveRoutine());
                }
            }
#if UNITY_EDITOR
            //Editor testing for skipping turns
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                NextTurn();
            }
#endif
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        //Check if the battle is not already active
        if (!battleActive)
        {
            //Set the battle scene to active and cannot flee to whats passed in
            cannotFlee = setCannotFlee;

            battleActive = true;

            GameManager.Instance.battleActive = true;

            //Set the battle background to the one set in the GameManager and the camera position
            StartCoroutine(WaitForBattle());

            //loop through the player positions
            for (int i = 0; i < playerPositions.Length; i++)
            {
                //check if the player is active
                if (GameManager.Instance.playerStats[i].gameObject.activeSelf)
                {
                    //loop through the player prefabs
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        //check if the player prefab name matches the player stats name
                        if (playerPrefabs[j].characterName == GameManager.Instance.playerStats[i].characterName)
                        {
                            //Instantiate the player prefab at the player position
                            BattleCharacter newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);

                            //Set the player prefab to the player position and add it to the active battlers list
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            //Set the player prefab stats to the player stats
                            CharacterStats currentPlayer = GameManager.Instance.playerStats[i];
                            activeBattlers[i].currentHP = currentPlayer.currentHP;
                            activeBattlers[i].maxHP = currentPlayer.maxHP;
                            activeBattlers[i].currentMP = currentPlayer.currentMP;
                            activeBattlers[i].maxMP = currentPlayer.maxMP;
                            activeBattlers[i].strength = currentPlayer.strength;
                            activeBattlers[i].defense = currentPlayer.defense;
                            activeBattlers[i].wpnPwr = currentPlayer.wpnPwr;
                            activeBattlers[i].armPwr = currentPlayer.armPwr;

                        }
                    }
                }
            }

            //loop through the enemies to spawn
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                //check if enemies are not empty
                if (enemiesToSpawn[i] != "")
                {
                    //loop through the enemy prefabs
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        //check if the enemy prefab name matches the enemy stats name
                        if (enemyPrefabs[j].characterName == enemiesToSpawn[i])
                        {
                            //Instantiate the enemy prefab at the enemy position and add it to the active battlers list
                            BattleCharacter newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            //set turn waiting to true and set the current turn to a random number
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
            characterStatsHolder.SetActive(true);
            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        //plus one to the current turn
        currentTurn++;

        //check if the current turn is greater than the active battlers count
        if (currentTurn >= activeBattlers.Count)
        {
            //if it is then set the current turn to 0
            currentTurn = 0;
        }

        //set turn waiting to true update the battle and UI stats
        turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        //loop through the active battlers
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //check if the current battler is dead
            if (activeBattlers[i].currentHP < 0)
            {
                //if it is then set the current HP to 0
                activeBattlers[i].currentHP = 0;
            }

            //check if the current battler is dead
            if (activeBattlers[i].currentHP == 0)
            {
                //if it is and is player then set the dead sprite and set the animator trigger to dead
                if (activeBattlers[i].isPlayer)
                {
                    //activeBattlers[i].sprite.sprite = activeBattlers[i].deadSprite;
                    activeBattlers[i].anim.SetTrigger("Dead");
                }
                else
                {
                    //else if enemy then call the enemy fade function
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                //if the current battler is not dead then set the animator trigger to idle
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].anim.SetTrigger("Idle");
                }
                else
                {
                    //set enemy to alive
                    allEnemiesDead = false;
                }
            }
        }

        //check if all enemies are dead or all players are dead
        if (allEnemiesDead || allPlayersDead)
        {
            //if all enemies are dead
            if(allEnemiesDead)
            {
                //start the end battle routine
                StartCoroutine(EndBattleRoutine());
            }
            else
            {
                //if all players are dead then start the game over routine
                StartCoroutine(GameOverRoutine());
            }
        }
        else
        {
            //if the current turn is dead then set the current turn to the next player
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;

                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }



    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        //create a variable to hold the move power
        int movePower = 0;

        //loop through the moves list and check if the move name matches the move name in the list
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                //instantiate the move effects at the target position and rotation and set the move power to the move power in the list
                Instantiate(movesList[i].effects, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        //set the animator trigger to attack and instantiate the enemy attack effects at the current turn position and rotation
        activeBattlers[currentTurn].anim.SetTrigger("Attack");
        Instantiate(enemyAttackEffects, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        //deal damage to the target
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void PlayerMagicAttack(string moveName, int selectedTarget)
    {
        //create a variable to hold the move power
        int movePower = 0;

        //loop through the moves list and check if the move name matches the move name in the list
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                //instantiate the move effects at the target position and rotation and set the move power to the move power in the list
                Instantiate(movesList[i].effects, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        //set the animator trigger to magic and instantiate the enemy attack effects at the current turn position and rotation
        activeBattlers[currentTurn].anim.SetTrigger("Magic");
        Instantiate(enemyAttackEffects, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        //deal damage to the target
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void EnemyAttack()
    {
        //create a new list to hold the players
        List<int> players = new();

        //loop through the active battlers and check if the current battler is a player and is alive
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                //if the current battler is a player and is alive then add it to the players list
                players.Add(i);
            }
        }

        //create a variable to hold the selected target and set it to a random player from the players list
        int selectedTarget = players[Random.Range(0, players.Count)];

        //create a variable to hold the selected attack and set it to a random move from the current battlers moves available
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        //create a variable to hold the move power
        int movePower = 0;

        //loop through the moves list and check if the move name matches the move name in the list
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                //instantiate the move effects at the target position and rotation and set the move power to the move power in the list
                Instantiate(movesList[i].effects, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        //instantiate the enemy attack effects at the current turn position and rotation
        Instantiate(enemyAttackEffects, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        //deal damage to the target
        DealDamage(selectedTarget, movePower);

        //call the update battle function
        UpdateBattle();
    }

    public void DealDamage(int target, int movePower)
    {
        //create a variable to hold the attack power and set it to the current battlers strength and weapon power
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        //create a variable to hold the defense power and set it to the target battlers defense and armor power
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armPwr;

        //calculate the damage to give by dividing the attack power by the defense power and multiplying it by the move power and a random number between 0.9 and 1.1
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(0.9f, 1.1f);
        //create a variable to hold the damage to give and set it to the rounded damage calculation
        int damageToGive = Mathf.RoundToInt(damageCalc);

        //debug log the damage calculation
        Debug.Log(activeBattlers[currentTurn].characterName + " is dealing " + damageCalc + "(" + damageToGive + ")" + " damage to " + activeBattlers[target].characterName);

        //reduce the target battlers current HP by the damage to give
        activeBattlers[target].currentHP -= damageToGive;

        //instantiate the damage number prefab at the target battlers position and rotation and set the damage to give
        Instantiate(DamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        //call the update ui stats function
        UpdateUIStats();

    }

    public void UpdateUIStats()
    {
        //loop through the players
        for (int i = 0; i < playerName.Length; i++)
        {
            //check if count of active battlers is greater than the current index
            if (activeBattlers.Count > i)
            {
                //check if the current battler is a player
                if (activeBattlers[i].isPlayer)
                {
                    //create a variable to hold the player data
                    BattleCharacter playerData = activeBattlers[i];

                    //set player name to active battlers name and set the player HP and MP text
                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.characterName;
                    playerHP[i].text = "HP: " + Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = "MP: " + Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                    
                }
                else
                {
                    //set player name to active
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                //set player name to inactive
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenTargetMenu(string moveName)
    {
        //set the target menu to active
        targetMenu.SetActive(true);

        //create a new list to hold the enemies
        List<int> enemies = new();

        //loop through the active battlers and check if the current battler is an enemy
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                //add the enemy to the enemies list
                enemies.Add(i);
            }
        }

        //loop through the target buttons
        for (int i = 0; i < targetButtons.Length; i++)
        {
            //check if the enemies list count is greater than the current index and if the current battler is alive
            if (enemies.Count > i && activeBattlers[enemies[i]].currentHP > 0)
            {
                //set the target button to active
                targetButtons[i].gameObject.SetActive(true);

                //set the target button to the enemy and set the target name to the enemy name
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattleTarget = enemies[i];
                targetButtons[i].targetName.text = activeBattlers[enemies[i]].characterName;
            }
            else
            {
                //set the target button to inactive
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        //set the magic menu to active
        magicMenu.SetActive(true);

        //loop through the magic buttons
        for (int i = 0; i < magicButtons.Length; i++)
        {
            //check if available moves are greater than the current index
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                //set the magic button to active and set the spell name and cost
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                //loop through the moves list and check if the move name matches the move name in the list
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        //set the spell cost and text
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                //set the magic button to inactive
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenItemMenu()
    {
        GameManager.Instance.SortItems();
        itemMenu.SetActive(true);
        ShowItems();
    }

    public void ShowItems()
    {
        GameManager.Instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++ )
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.Instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.Instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item selectedItem)
    {
        activeItem = selectedItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if(activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    //public void OpenItemTargetMenu()
    //{
    //    itemTargetMenu.SetActive(true);

    //    for (int i = 0; i < itemCharChoiceNames.Length; i++)
    //    {
    //        itemCharChoiceNames[i].text = GameManager.Instance.playerStats[i].characterName;
    //        itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.Instance.playerStats[i].gameObject.activeSelf);
    //    }
    //}

    public void UseItem(int selectedCharacter)
    {
        activeItem.Use(selectedCharacter);
        itemMenu.SetActive(false);
        GameManager.Instance.SortItems();
        UpdateUIStats();
        battleNotice.theText.text = "Used a " + activeItem.itemName + " on " + activeBattlers[currentTurn].characterName;
        battleNotice.Activate();
        StartCoroutine(UseItemRoutine());
    }

    public void CloseItemMenu()
    {
        itemMenu.SetActive(false);
    }

    public void Flee()
    {
        //check if cannot flee is true
        if (cannotFlee)
        {
            //display a message that you cannot flee
            battleNotice.theText.text = "Can't Flee This Battle!";
            battleNotice.Activate();
        }
        else
        {
            //create a variable to hold the flee chance and set it to a random number between 0 and 100
            int fleeSuccess = Random.Range(0, 100);
            //check if the flee chance is less than the chance to flee
            if (fleeSuccess < chanceToFlee)
            {
                //set fleeing to true and start the end battle routine
                fleeing = true;
                StartCoroutine(EndBattleRoutine());
            }
            else
            {
                //else display a message that you could not flee and move to next turn
                NextTurn();
                battleNotice.theText.text = "Could not Escape!";
                battleNotice.Activate();
            }
        }
    }

    /*-------------------------------------------------------------------------Enumerators---------------------------------------------------------------------------*/

    public IEnumerator EnemyMoveRoutine()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public IEnumerator UseItemRoutine()
    {
        Instantiate(healPrefab, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public IEnumerator EndBattleRoutine()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        characterStatsHolder.SetActive(false);

        yield return new WaitForSeconds(.5f);
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.Instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].characterName == GameManager.Instance.playerStats[j].characterName)
                    {
                        GameManager.Instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.Instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.Instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;

        if(fleeing)
        {
            GameManager.Instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleRewards.Instance.OpenRewardsScreen(rewardEXP, rewardItems);
        }

        AudioManager.Instance.PlayBGM(FindObjectOfType<AudioPlayer>().musicToPlay);
    }

    public IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1f);
        battleActive = false;
        characterStatsHolder.SetActive(false);
        GameManager.Instance.battleActive = false;
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }

    //Wait for the battle to start to allow camera to move to player position
    private IEnumerator WaitForBattle()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        battleScene.SetActive(true);
        AudioManager.Instance.PlayBGM(battleTrack);
    }
}
