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
        
        // Ensure bullet only flies in X/Z plane (no Y component)
        shootDirection.y = 0;
        shootDirection = shootDirection.normalized;
        
        // Calculate perpendicular direction for side bullets (in X/Z plane)
        Vector3 rightDirection = Vector3.Cross(Vector3.up, shootDirection).normalized;
        
        IHitTarget executorHitTarget = executor.GetComponent<IHitTarget>();
        int generatorID = executorHitTarget?.GetGeneratorID() ?? 0;

        // Shoot 3 bullets: center, one left, one right
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition, shootDirection);
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition + rightDirection, shootDirection);
        Bullet.Builder(_attackPower, _bulletSpeed, generatorID, centerPosition - rightDirection, shootDirection);
        
        StartCooldown();
    }
}