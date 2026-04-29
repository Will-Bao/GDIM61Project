using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool PlayerSpotted {  get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }

    public void PlayerLose()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    public void PlayerWin()
    {
        SceneManager.LoadScene("MainMenu"); // TODO: change to win screen once added
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetPlayerSpotted(bool spotted)
    {
        PlayerSpotted = spotted;
    }
}
