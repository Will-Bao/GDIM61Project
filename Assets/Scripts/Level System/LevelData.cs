using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Key Locations")]
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _exit;

    [Header("Enemy Patrol Points")]
    [SerializeField] private List<Transform> _patrolPoints;

    public Vector2 GetStart()
    {
        return _start.position;
    }
    public Vector2 GetExit()
    {
        return _exit.position;
    }
    public List<Transform> GetPatrolPoints()
    {
        return _patrolPoints;
    }
}
