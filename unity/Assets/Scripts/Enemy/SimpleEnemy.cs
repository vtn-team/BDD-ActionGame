using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [SerializeField] private float _attackInterval;
    [SerializeField, SerializeReference, SubclassSelector] private SkillBase _skill;
    [SerializeField] private int _generatorID;

    private float _attackTimer;

    protected override void Awake()
    {
        base.Awake();
        _attackTimer = _attackInterval;
    }

    private void Update()
    {
        _attackTimer -= Time.deltaTime;
        
        if (_skill != null)
        {
            _skill.UpdateCooldown(Time.deltaTime);
        }

        if (_attackTimer <= 0 && _skill != null && _skill.CheckExecute())
        {
            _skill.Execute(gameObject);
            _attackTimer = _attackInterval;
        }
    }

    public override int GetGeneratorID()
    {
        return _generatorID;
    }
}