using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int CurrentLevel { get; private set; }

    [Header("Levels")]
    [SerializeField] private List<LevelParallax> _levels = new();

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
        UpdateLevels();
    }

    public void NextLevel()
    {
        if (CurrentLevel >= _levels.Count - 1) return;

        CurrentLevel++;
        UpdateLevels();
    }

    public void PreviousLevel()
    {
        if (CurrentLevel <= 0) return;

        CurrentLevel--;
        UpdateLevels();
    }

    private void UpdateLevels()
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            int layerOffset = i - CurrentLevel;
            _levels[i].SetParallaxLayer(layerOffset);
        }
    }
}
