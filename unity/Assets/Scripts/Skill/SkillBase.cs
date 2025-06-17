using UnityEngine;

[System.Serializable]
public abstract class SkillBase
{
    [SerializeField] protected float _actionInterval;
    [SerializeField] protected float _coolTime;
    
    private float _coolTimeTimer;

    public float NextActionInterval => _actionInterval;
    
    public float CoolTime => _coolTimeTimer > 0 ? _coolTimeTimer : 0;
    
    public bool CheckExecute()
    {
        return _coolTimeTimer <= 0;
    }
    
    public void StartCooldown()
    {
        _coolTimeTimer = _coolTime;
    }
    
    public void UpdateCooldown(float deltaTime)
    {
        if (_coolTimeTimer > 0)
        {
            _coolTimeTimer -= deltaTime;
        }
    }
    
    public abstract void Execute(GameObject executor);
}