using UnityEngine;

[CreateAssetMenu(fileName = "ShootBullet", menuName = "Skills/ShootBullet")]
public class ShootBullet : SkillBase
{
    [SerializeField] private int _attackPower;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private GameObject _bulletPrefab;

    public override void Execute(GameObject executor)
    {
        if (!CheckExecute()) return;

        Vector3 shootPosition = executor.transform.position + executor.transform.forward;
        Vector3 shootDirection = executor.transform.forward;
        
        IHitTarget executorHitTarget = executor.GetComponent<IHitTarget>();
        int generatorID = executorHitTarget?.GetGeneratorID() ?? 0;

        Bullet.Builder(_bulletPrefab, shootPosition, shootDirection, _attackPower, _bulletSpeed, generatorID);
        
        StartCooldown();
    }
}