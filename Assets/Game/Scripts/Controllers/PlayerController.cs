using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

public class PlayerController : ScriptBase
{

    public int TeamCount { get; set; }

    [SerializeField]
    private GameObject[] _team0PlayerPrefabs;

    [SerializeField]
    private GameObject[] _team1PlayerPrefabs;

    [SerializeField]
    private GameObject[] _team2PlayerPrefabs;

    [SerializeField]
    private GameObject[] _team3PlayerPrefabs;

    private Dictionary<GameObject, int> AvailablePrefabs = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, GameObject> PrefabsInUse = new Dictionary<GameObject, GameObject>();

    private int _maxPlayers;
    private GameObject _playerContainer;
    private List<Transform> _teamSpawns = new List<Transform>();
    private List<Player> _players = new List<Player>();
    private int _currentTeamIndex;

    private void Awake()
    {
        _playerContainer = Instantiate(new GameObject("PlayerContainer")); // FindObject("PlayerContainer");

        _teamSpawns.Add(FindObject("SpawnTeam0").transform);
        _teamSpawns.Add(FindObject("SpawnTeam1").transform);
        _teamSpawns.Add(FindObject("SpawnTeam2").transform);
        _teamSpawns.Add(FindObject("SpawnTeam3").transform);

        InitPrefabStructure();
    }

    private void InitPrefabStructure()
    {
        if (TeamCount >= 1)
        {
            foreach (var prefab in _team0PlayerPrefabs)
            {
                AvailablePrefabs.Add(prefab, 0);
            }

            _maxPlayers += _team0PlayerPrefabs.Count();
        }

        if (TeamCount >= 2)
        {
            foreach (var prefab in _team1PlayerPrefabs)
            {
                AvailablePrefabs.Add(prefab, 1);
            }

            _maxPlayers += _team1PlayerPrefabs.Count();
        }

        if (TeamCount >= 3)
        {
            foreach (var prefab in _team2PlayerPrefabs)
            {
                AvailablePrefabs.Add(prefab, 2);
            }

            _maxPlayers += _team2PlayerPrefabs.Count();
        }

        if (TeamCount >= 4)
        {
            foreach (var prefab in _team3PlayerPrefabs)
            {
                AvailablePrefabs.Add(prefab, 3);
            }

            _maxPlayers += _team3PlayerPrefabs.Count();
        }
    }

    public void StartGame()
    {
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        AirConsole.instance.onMessage += OnMessage;

        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        foreach (var deviceID in AirConsole.instance.GetControllerDeviceIds())
        {
            SpawnPlayer(deviceID);
        }
    }

    private void SpawnPlayer(int deviceID)
    {
        var prefabForPlayer = GetNextPrefab();
        var teamIndex = AvailablePrefabs[prefabForPlayer];

        var playerObj = Instantiate(prefabForPlayer, _teamSpawns[teamIndex].position, Quaternion.identity) as GameObject;
        playerObj.transform.SetParent(_playerContainer.transform);

        PrefabsInUse.Add(playerObj, prefabForPlayer);
        AvailablePrefabs.Remove(prefabForPlayer);

        var playerComp = playerObj.GetComponent<Player>();
        playerComp.DeviceID = deviceID;
        _players.Add(playerComp);

        _currentTeamIndex = (_currentTeamIndex + 1) % TeamCount;
    }

    private void DespawPlayer(int deviceID)
    {
        var player = _players.FirstOrDefault(p => p.DeviceID == deviceID);

        if (player != null)
        {
            var playerObj = player.transform.gameObject;
            var playerPrefab = PrefabsInUse[playerObj];
            AvailablePrefabs.Add(playerPrefab, player.TeamIndex);

            _players.Remove(player);
            Object.DestroyObject(player.gameObject);
        }
    }

    private GameObject GetNextPrefab()
    {
        var group = AvailablePrefabs.GroupBy(p => p.Value).OrderByDescending(p => p.Count()).First();
        return AvailablePrefabs.First(e => e.Value == group.Key).Key;
    }

    private void OnConnect(int deviceID)
    {
        if (!_players.Any(p => p.DeviceID == deviceID) &&
            _players.Count < _maxPlayers)
        {
            SpawnPlayer(deviceID);
        }
    }

    private void OnDisconnect(int deviceID)
    {
        DespawPlayer(deviceID);
    }

    private void OnMessage(int deviceID, JToken data)
    {
        var targetPlayer = _players.FirstOrDefault(p => p.DeviceID == deviceID);

        if (targetPlayer != null)
        {
            var message = new ControllerMessage(data);

            if (message.ActionName == "convince")
            {
                targetPlayer.Convince();
            }
            else if (message.ActionName == "destroy")
            {
                targetPlayer.Destroy();
            }
            else if (message.ActionName == "joystick-left")
            {
                targetPlayer.Move(message.JoystickPosition);
            }
        }
    }

    private void Update()
    {
        foreach (var player in _players)
        {
            if (player.State == PlayerState.Dead)
            {
                player.transform.position = _teamSpawns[player.TeamIndex].position;
                player.SetVisibility(true);
                player.State = PlayerState.Normal;
            }
        }
    }
}
