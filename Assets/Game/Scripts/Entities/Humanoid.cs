using UnityEngine;
using System.Collections;

public abstract class Humanoid : ScriptBase
{
    [SerializeField]
    protected float _speed;

    private bool _isMoving;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        protected set
        {
            _isMoving = value;
            _animator.SetBool("IsMoving", _isMoving);
        }
    }

    protected Animator _animator;

    public abstract void Kill();

    public Vector3 Get2DPosition()
    {
        return Vector3.Scale(transform.position, new Vector3(1, 1, 0));
    }

    public void SetVisibility(bool visible)
    {
        gameObject.GetComponent<Renderer>().enabled = visible;
    }

    protected void AdjustDepth()
    {
        var pos = transform.position;
        pos.z = pos.y / (Camera.main.pixelHeight / 2);
        transform.position = pos;
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        AdjustDepth();
    }

    protected void UpdateMoveDirection(Vector3 directionVector)
    {
        if (directionVector.x >= 0 && directionVector.y >= 0)
        {
            _animator.SetBool("IsFront", false);
            Flip(-1);
        }
        else if (directionVector.x < 0 && directionVector.y >= 0)
        {
            _animator.SetBool("IsFront", false);
            Flip(1);
        }
        else if (directionVector.x < 0 && directionVector.y < 0)
        {
            _animator.SetBool("IsFront", true);
            Flip(1);
        }
        else
        {
            _animator.SetBool("IsFront", true);
            Flip(-1);
        }
    }

    protected void Flip(float direction)
    {
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}
