using UnityEngine;
using System.Collections;

public class Npc : Humanoid
{
    private float _startTime;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private float _moveDistance;
    

    public NpcState State { get; set; }
    
    private int _teamIndex;
    public int TeamIndex
    {
        get
        {
            return _teamIndex;
        }
        set
        {
            _teamIndex = value;
            _animator.SetInteger("Team", _teamIndex);
        }
    }

    public void MoveTo(Vector3 endPoint)
    {
        _endPoint = endPoint;
        _startTime = Time.time;
        _startPoint = Get2DPosition();
        _moveDistance = Vector3.Distance(_startPoint, _endPoint);

        UpdateMoveDirection(_endPoint - _startPoint);
        IsMoving = true;
    }

    public override void Kill()
    {
        State = NpcState.Killed;
        IsMoving = false;
        TeamIndex = -1;
        _animator.SetTrigger("Decay");
    }

    public void AfterDecay()
    {
        SetVisibility(false);
        State = NpcState.Decayed;
    }

    protected override void Awake()
    {
        base.Awake();
        TeamIndex = -1;
        State = NpcState.Normal;
    }
	
	private void Update()
    {
        if (IsMoving)
        {
            float movedDistance = (Time.time - _startTime) * _speed;
            float fracJourney = movedDistance / _moveDistance;
            transform.position = Vector3.Lerp(_startPoint, _endPoint, fracJourney);

            AdjustDepth();

            if ((Get2DPosition() - _endPoint).magnitude < Constants.Precision)
            {
                IsMoving = false;
            }
        }
    } 
}
