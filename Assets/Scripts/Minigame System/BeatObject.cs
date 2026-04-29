using UnityEngine;

public class BeatObject : MonoBehaviour
{
    private Beat _beat;
    private RectTransform _rect;

    private float _spawnX;
    private Vector2 _targetPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    public void Initialize(Beat data, Vector2 targetPos, float xOffset)
    {
        _beat = data;
        _targetPos = targetPos;
        _spawnX = targetPos.x + xOffset;
    }

    public Beat GetData()
    {
        return _beat;
    }

    private void Update()
    {
        float timeRemaining = _beat.hitTime - Time.time;

        float xPosition = Mathf.Lerp(_targetPos.x, _spawnX, timeRemaining / _beat.travelTime);

        _rect.anchoredPosition = new Vector2(xPosition, 0);

        if (Mathf.Abs(_rect.anchoredPosition.x - _targetPos.x) < 0.1f)
        {
            BeatGameManager.Instance.NotifyMiss(this);
            Destroy(gameObject);
        }
    }
}

public class Beat
{
    public float hitTime;
    public float travelTime;
    public float direction; // Left: -1 ; Right: 1

    public Beat(float hit, float travel, float dir)
    {
        hitTime = hit;
        travelTime = travel;
        direction = dir;
    }
}
