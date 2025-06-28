using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _maxHitPoint;
    
    private int _hitPoint;
    private Rigidbody _rigidbody;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        // Initialize hitPoint with maxHitPoint
        _hitPoint = _maxHitPoint;
        
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
    
    public float GetHPPercentage()
    {
        return _maxHitPoint > 0 ? (float)_hitPoint / _maxHitPoint : 0f;
    }
}