using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NpcController : ScriptBase
{
    [SerializeField]
    private GameObject _maleNpcPrefab;

    [SerializeField]
    private GameObject _femaleNpcPrefab;

    [SerializeField]
    private int _npcSpawnCount;

    [SerializeField]
    private int _borderOffset;

    [SerializeField]
    private float _updateInterval;

    [SerializeField]
    private float _playerMoveProbability;

    [SerializeField]
    private float _playerMoveBoxWidth;

    [SerializeField]
    private float _playerMoveBoxHeight;

    private GameObject _npcContainer;
    private List<Npc> _npcs = new List<Npc>();
    private float _lastUpdateTime;

    private void Awake()
    {
        _npcContainer = Instantiate(new GameObject("NpcContainer"));
    }

    public void StartGame()
    {
        SpawnRandomNpcs();
    }

    public Dictionary<int, int> GetNpcStats()
    {
        var stats = new Dictionary<int, int>();

        stats.Add(0, _npcs.Count(n => n.TeamIndex == 0));
        stats.Add(1, _npcs.Count(n => n.TeamIndex == 1));
        stats.Add(2, _npcs.Count(n => n.TeamIndex == 2));
        stats.Add(3, _npcs.Count(n => n.TeamIndex == 3));

        return stats;
    }

    private void SpawnRandomNpcs()
    {
        for (var i = 0; i < _npcSpawnCount; i++)
        {
            var randomPos = Util.CalculateWorldBox(_borderOffset).GetRandomVectorInBox();

            GameObject npc = null;
            if (Util.CalculateRandomEvent(0.5f))
            {
                npc = Instantiate(_maleNpcPrefab, randomPos, Quaternion.identity) as GameObject;
            }
            else
            {
                npc = Instantiate(_femaleNpcPrefab, randomPos, Quaternion.identity) as GameObject;
            }

            npc.transform.SetParent(_npcContainer.transform);

            _npcs.Add(npc.GetComponent<Npc>());
        }
    }

    private void Update()
    {
        if (Time.time - _lastUpdateTime > _updateInterval)
        {
            foreach (var npc in _npcs)
            {
                if (npc.State == NpcState.Decayed)
                {
                    npc.State = NpcState.Normal;
                    npc.transform.position = Util.CalculateWorldBox(_borderOffset).GetRandomVectorInBox();
                    npc.SetVisibility(true);
                }

                if (npc.State != NpcState.Killed && !npc.IsMoving && Util.CalculateRandomEvent(_playerMoveProbability))
                {
                    var pos = npc.Get2DPosition();

                    var lowerX = pos.x - _playerMoveBoxWidth / 2;
                    var lowerY = pos.y - _playerMoveBoxHeight / 2;
                    var upperX = pos.x + _playerMoveBoxWidth / 2;
                    var upperY = pos.y + _playerMoveBoxHeight / 2;

                    var moveBox = new Box(lowerX, lowerY, upperX, upperY);
                    moveBox.FitBox(Util.CalculateWorldBox(_borderOffset));

                    var newPos = moveBox.GetRandomVectorInBox();

                    npc.MoveTo(newPos);
                }
            }

            _lastUpdateTime = Time.time;
        }
    }
}
