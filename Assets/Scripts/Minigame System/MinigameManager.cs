using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;
    [Header("Games")]
    [SerializeField] private BeatGameManager _beatGame;
    [SerializeField] private DodgeGameManager _dodgeGame;

    [Header("Duration")]
    [SerializeField] private float _rhythmGameDuration;
    [SerializeField] private float _dodgeGameDuration;

    public event Action<bool> OnGameStarted;
    public bool GameStarted { get; private set; }
    public float TotalGameAmount { get; private set; }
    public enum GameType { Beat, Dodge, Random }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    public void StartGame(GameType type)
    {
        if (GameStarted) return;
        GameStarted = true;
        TotalGameAmount++;

        if (type == GameType.Random) type = (GameType)Random.Range(0, 2);

        switch (type)
        {
            case GameType.Beat:
                _beatGame.StartGame(_rhythmGameDuration);
                break;
            case GameType.Dodge:
                _dodgeGame.StartGame(_dodgeGameDuration);
                break;
        }
        OnGameStarted.Invoke(true);
    }

    public void EndGame()
    {
        if (!GameStarted) return;
        GameStarted = false;
        OnGameStarted.Invoke(false);
    }
}
