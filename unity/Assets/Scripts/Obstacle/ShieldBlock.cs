using UnityEngine;

public class ShieldBlock : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _hitPoint = 5;
    [SerializeField] private int _generatorID = -1;

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
}