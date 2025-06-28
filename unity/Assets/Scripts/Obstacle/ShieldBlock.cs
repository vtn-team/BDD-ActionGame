using UnityEngine;

public class ShieldBlock : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _hitPoint = 5;
    [SerializeField] private int _generatorID = -1;
    
    private int _maxHitPoint;

    private void Awake()
    {
        _maxHitPoint = _hitPoint;
    }

    public bool Damage(int attackPower)
    {
        _hitPoint -= attackPower;
        
        if (_hitPoint <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        
        return true;
    }

    public int GetGeneratorID()
    {
        return _generatorID;
    }
    
    public bool CheckDead()
    {
        return _hitPoint <= 0;
    }
    
    public float GetHPPercentage()
    {
        return _maxHitPoint > 0 ? (float)_hitPoint / _maxHitPoint : 0f;
    }
}