using UnityEngine;

[System.Serializable]
public class ShootBullet : SkillBase
{
    [SerializeField] private int _attackPower = 1;
    [SerializeField] private float _bulletSpeed = 5f;
    
    public ShootBullet()
    {
        // Set default values from game design spec
        _actionInterval = 0.5f;
        _coolTime = 1.0f;
    }

    public override void Execute(GameObject executor)
    {
        if (!CheckExecute()) return;

        Vector3 shootPosition = executor.transform.position;
        Vector3 shootDirection = executor.transform.forward;
        
        IHitTarget executorHitTarget = executor.GetComponent<IHitTarget>();
        int generatorID = executorHitTarget?.GetGeneratorID() ?? 0;

        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, shootPosition, shootDirection);
        
        StartCooldown();
    }
}