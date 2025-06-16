using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _hitPoint;

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