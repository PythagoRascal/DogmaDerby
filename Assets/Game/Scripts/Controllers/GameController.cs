using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : ScriptBase
{
    [SerializeField]
    private float _timePerRound;

    [SerializeField]
    private AudioClip _finishIndicator; 

    private GameState _currentState = GameState.WaitingForPlayers;

    private DigitDisplayManager _playerCount;
    private DigitDisplayManager _timer;
    private NpcController _npcController;
    private PlayerController _playerController;
    private AudioSource _themeAudioSource;
    private float _remainingTime;

    private void Awake()
    {
        _playerCount = FindObject("PlayerCount").GetComponent<DigitDisplayManager>();

        Debug.Log(FindObject("PlayerCount").tag);

        _timer = FindObject("TimerScreen").GetComponent<DigitDisplayManager>();
        _npcController = FindObject("NpcController").GetComponent<NpcController>();
        _playerController = FindObject("PlayerController").GetComponent<PlayerController>();
        _themeAudioSource = FindObject("ThemeAudioSource").GetComponent<AudioSource>();

        _playerController.TeamCount = PlayerPrefs.GetInt("TeamCount", 2);
        PlayerPrefs.DeleteAll();

        AirConsole.instance.onConnect += OnConnect;

        _remainingTime = _timePerRound;
    }

    private void Start()
    {
        _playerCount.displayNumber = _playerController.TeamCount;
    }

    private void Update()
    {
        if (_currentState == GameState.Playing)
        {
            _remainingTime -= Time.deltaTime;

            _timer.displayNumber = ((int)_remainingTime);

            if (_remainingTime <= 0)
            {
                var stats = _npcController.GetNpcStats();

                foreach (var kv in stats)
                {
                    PlayerPrefs.SetInt(kv.Key.ToString(), kv.Value);
                }

                SceneManager.LoadScene(2);
            }
        }
    }

    private void OnConnect(int device_id)
    {
        if (_currentState == GameState.WaitingForPlayers)
        {
            var connectedPlayers = AirConsole.instance.GetControllerDeviceIds().Count;
            if (connectedPlayers >= _playerController.TeamCount)
            {
                StartGame();
            }

            _playerCount.displayNumber = (_playerController.TeamCount - connectedPlayers);
        }
    }

    public void StartGame()
    {
        _currentState = GameState.Playing;

        AirConsole.instance.onConnect -= OnConnect;

        _npcController.StartGame();
        _playerController.StartGame();

        FindObject("WaitingScreen").SetActive(false);

        _themeAudioSource.Play();
    }
}