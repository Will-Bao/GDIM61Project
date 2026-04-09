using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")] // Move to its own UI manager in future
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    private void Start()
    {
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
    }

    public void PlayerLose()
    {
        _loseScreen.SetActive(true);
    }

    public void PlayerWin()
    {
        _winScreen.SetActive(true);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
