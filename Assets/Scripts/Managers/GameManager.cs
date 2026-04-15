using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")] // Move to its own UI manager in future
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;

    public bool _endCondition;
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
        if (_endCondition) return;
        _loseScreen.SetActive(true);
        _endCondition = true;
    }

    public void PlayerWin()
    {
        if (_endCondition) return;
        _winScreen.SetActive(true);
        _endCondition = true;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
