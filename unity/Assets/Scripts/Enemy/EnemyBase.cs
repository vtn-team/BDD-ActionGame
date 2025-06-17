using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _hitPoint;
    
    private Rigidbody _rigidbody;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }

    public bool CheckDead()
    {
        return _hitPoint <= 0;
    }

    public virtual bool Damage(int attackPower)
    {
        _hitPoint -= attackPower;
        return true;
    }

    public abstract int GetGeneratorID();
}