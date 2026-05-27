using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class ElevatorController : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerUI;

    [Header("Cutscene Videos")]
    [SerializeField] private GameObject _videoVisuals;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private string[] _videoClips;

    [Header("Dialogue")]
    [SerializeField] private DialogueSystems _dialogueSystem;
    [SerializeField] private DialogueData _encounterDialogue;

    [Header("Audio")]
    [SerializeField] private AudioClip _elevatorDingSFX;
    [SerializeField] private AudioClip _elevatorRumbleSFX;
    [SerializeField] private float _sfxVolume = 1f;

    [Header("AnteaterSpawn")]
    [SerializeField] private GameObject _anteater;
    [SerializeField] private Vector3 _anteaterOffset = new Vector3(1.5f, 0f, 0f);


    private bool _isPlaying;

    public void StartEncounter()
    {
        Debug.Log("Encounter Started");
        if (_isPlaying) return;
        StartCoroutine(EncounterRoutine());
    }
    private void Awake()
    {
        if (_dialogueSystem == null)
        {
            _dialogueSystem = FindFirstObjectByType<DialogueSystems>();
        }
    }
    private IEnumerator EncounterRoutine()
    {
            SoundFXManager.instance.PlaySoundFXClip(
            _elevatorDingSFX,
            transform,
            _sfxVolume,
            false,
            false
        );
        _isPlaying = true;

        _player.SetActive(false);
        _playerUI.SetActive(false);
        _videoVisuals.SetActive(true);

        if (_encounterDialogue != null && _dialogueSystem != null)
        {
            _dialogueSystem.StartDialogue(_encounterDialogue);
        }
        else
        {
            Debug.Log("Missing dialoguesystem or encounter");
        }
        for (int i = 0; i < _videoClips.Length; i++)
        {
            Debug.Log("Playing: " + _videoClips[i]);
            yield return PlayVideo(_videoClips[i]);

            if (i == 0 && _elevatorRumbleSFX != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(
                    _elevatorRumbleSFX,
                    transform,
                    _sfxVolume,
                    false,
                    false
                );
            }
        }
        // if (_encounterDialogue != null)
        // {
        //     yield return new WaitUntil(() => !_dialogueSystem.IsTalking);
        // }

        _videoPlayer.Stop();
        _videoPlayer.clip = null;
        _videoVisuals.SetActive(false);

        _player.SetActive(true);
        _playerUI.SetActive(true);
        if (_anteater != null)
        {
            _anteater.transform.position = _player.transform.position + _anteaterOffset;

            Enemy enemy = _anteater.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.ForceToCurrentPlayerLayer();

                NoiseData scriptedNoise = new NoiseData(
                    _player.transform.position,
                    5,
                    LevelManager.Instance.CurrentLevel,
                    NoiseType.Player
                );

                enemy.AlertEnemy(scriptedNoise);
            }
        }
        _isPlaying = false;
    }
    private IEnumerator PlayVideo(string clip)
    {
        if (clip == null || _videoPlayer == null) yield break;

        _videoPlayer.Stop();
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, clip);
        _videoPlayer.isLooping = false;
        _videoPlayer.Prepare();

        yield return new WaitUntil(() => _videoPlayer.isPrepared);

        _videoPlayer.Play();

        while (_videoPlayer.isPlaying)
        {
            yield return null;
        }

        _videoPlayer.Stop();
    }
}