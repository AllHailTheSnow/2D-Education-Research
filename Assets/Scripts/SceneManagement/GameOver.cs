using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string mainMenuScene;
    public string loadGameScene;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(3);
    }

    public void QuitToMain()
    {
        Destroy(GameManager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        Destroy(BattleManager.Instance.gameObject);
        Destroy(UIController.Instance.gameObject);
        Destroy(CameraController.Instance.gameObject);
        Destroy(QuestManager.Instance.gameObject);
        Destroy(BaseSingleton.Instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave()
    {
        Destroy(PlayerController.Instance.gameObject);
        Destroy(UIController.Instance.gameObject);

        SceneManager.LoadScene(loadGameScene);
    }
}
