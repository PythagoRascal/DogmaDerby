using UnityEngine;
using System.Collections;
using System;

public class Player : Humanoid
{
    [SerializeField]
    public int TeamIndex;

    [SerializeField]
    private float _borderOffset;

    [SerializeField]
    private float _convictionRadius;

    [SerializeField]
    private float _destructionRadius;

    [SerializeField]
    private AudioClip _convictionSound;

    [SerializeField]
    private AudioClip _destructionSound;

    [SerializeField]
    private float _convictionDuration;

    [SerializeField]
    private GameObject _convictionParticles;

    [SerializeField]
    private Color _playerColor;

    public int DeviceID { get; set; }

    public PlayerState State { get; set; }

    private Vector3 _moveDirection;
    private AudioSource _audioSource;
    private float _convictionStart;
	
    protected override void Awake()
    {
        base.Awake();

        State = PlayerState.Normal;
        _audioSource = GetComponent<AudioSource>();
    }

	private void Update()
    {
        IsMoving = false;

        if (State == PlayerState.Normal)
        {
            ProcessMovement();
        }
        else if (State == PlayerState.Convincing)
        {
            ProcessConviction();
        }
	}

    public void Move(Vector3 direction)
    {
        _moveDirection = direction.normalized;
    }

    public void Convince()
    {
        if (State == PlayerState.Normal)
        {
            State = PlayerState.Convincing;
            _convictionStart = Time.time;

            var particleGeneratorObj = Instantiate(_convictionParticles, transform.position, Quaternion.identity) as GameObject;
            var particleGeneratorComp = particleGeneratorObj.GetComponent<ParticleGenerator>();
            particleGeneratorComp.Generate(_convictionRadius, 4, 16, _playerColor);

            _audioSource.PlayOneShot(_convictionSound);
        }
    }

    public void Destroy()
    {
        if (State == PlayerState.Normal)
        {
            State = PlayerState.Destroying;

            var particleGeneratorObj = Instantiate(_convictionParticles, transform.position, Quaternion.identity) as GameObject;
            var particleGeneratorComp = particleGeneratorObj.GetComponent<ParticleGenerator>();
            particleGeneratorComp.Generate(_destructionRadius, 1, 16, Color.black);

            _audioSource.PlayOneShot(_destructionSound);

            var pos2D = new Vector2(transform.position.x, transform.position.y);
            var colliders = Physics2D.OverlapCircleAll(pos2D, _destructionRadius);

            foreach (var collider in colliders)
            {
                var npc = collider.GetComponent<Npc>();

                if (npc != null)
                {
                    npc.Kill();
                }
                else
                {
                    var player = collider.GetComponent<Player>();

                    if (player != null)
                    {
                        player.Kill();
                    }
                }
            }
        }
    }

    public override void Kill()
    {
        State = PlayerState.Burning;
        _animator.SetTrigger("Burn");
    }

    public void AfterBurning()
    {
        SetVisibility(false);
        State = PlayerState.Dead;
    }

    private void ProcessMovement()
    {
        var newPos = Get2DPosition() + _moveDirection * _speed;
        var worldBox = Util.CalculateWorldBox(_borderOffset);

        newPos.x = Mathf.Clamp(newPos.x, worldBox.LowerCorner.x, worldBox.UpperCorner.x);
        newPos.y = Mathf.Clamp(newPos.y, worldBox.LowerCorner.y, worldBox.UpperCorner.y);

        transform.position = newPos;
        AdjustDepth();

        if (_moveDirection != Vector3.zero)
        {
            UpdateMoveDirection(_moveDirection);
            IsMoving = true;
        }
    }

    private void ProcessConviction()
    {
        if (Time.time - _convictionStart >= _convictionDuration)
        {
            var pos2D = new Vector2(transform.position.x, transform.position.y);
            var colliders = Physics2D.OverlapCircleAll(pos2D, _convictionRadius);

            foreach (var collider in colliders)
            {
                var npc = collider.GetComponent<Npc>();

                if (npc != null)
                {
                    npc.TeamIndex = TeamIndex;
                }
            }

            State = PlayerState.Normal;
        }
    }
}
