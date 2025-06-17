using UnityEngine;

[System.Serializable]
public class ThreeLineShoot : SkillBase
{
    [SerializeField] private int _attackPower = 1;
    [SerializeField] private float _bulletSpeed = 5f;
    
    public ThreeLineShoot()
    {
        // Set default values from game design spec
        _actionInterval = 1.0f;
        _coolTime = 5.0f;
    }

    public override void Execute(GameObject executor)
    {
        if (!CheckExecute()) return;

        Vector3 centerPosition = executor.transform.position;
        Vector3 shootDirection = executor.transform.forward;
        Vector3 upOffset = executor.transform.up;
        
        IHitTarget executorHitTarget = executor.GetComponent<IHitTarget>();
        int generatorID = executorHitTarget?.GetGeneratorID() ?? 0;

        // Shoot 3 bullets: center, one above, one below
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition, shootDirection);
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition + upOffset, shootDirection);
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition - upOffset, shootDirection);
        
        StartCooldown();
    }
}