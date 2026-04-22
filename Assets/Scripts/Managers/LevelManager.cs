using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int CurrentLevel { get; private set; }

    [Header("Levels")]
    [SerializeField] private List<GameObject> _levels = new();
    [SerializeField] private int _lastLevel;

    [Header("Transition")]
    [SerializeField] private float _cooldown = 0.5f;

    public static Action<int> OnLevelSwitched;
    public bool IsTransitioning { get; private set; }
    private List<LevelParallax> _levelsParallax = new();
    private List<LevelData> _levelsData = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
        foreach (var level in _levels)
        {
            _levelsParallax.Add(level.GetComponent<LevelParallax>());
            _levelsData.Add(level.GetComponent<LevelData>());
        }
    }
    private void Start()
    {
        UpdateLevels();
    }

    public LevelParallax GetLevelParallax(int layerNum)
    {
        return _levelsParallax[layerNum];
    }

    public LevelData GetLevelData(int layerNum)
    {
        return _levelsData[layerNum];
    }

    public void NextLevel()
    {
        if (CurrentLevel >= _levels.Count - 1 || IsTransitioning) return;
        if (CurrentLevel == _lastLevel)
        {
            GameManager.Instance.PlayerWin();
            return;
        }
        StartCoroutine(Cooldown());

        CurrentLevel++;
        UpdateLevels();
        OnLevelSwitched?.Invoke(CurrentLevel);
    }

    public void PreviousLevel()
    {
        if (CurrentLevel <= 0 || IsTransitioning) return;
        StartCoroutine(Cooldown());

        CurrentLevel--;
        UpdateLevels();
        OnLevelSwitched?.Invoke(CurrentLevel);
    }

    private void UpdateLevels()
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            int layerOffset = i - CurrentLevel;
            _levelsParallax[i].SetParallaxLayer(layerOffset);
        }
    }

    private IEnumerator Cooldown()
    {
        IsTransitioning = true;
        yield return new WaitForSeconds(_cooldown);
        IsTransitioning = false;
    }
}