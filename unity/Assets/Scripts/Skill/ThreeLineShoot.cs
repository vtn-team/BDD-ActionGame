using UnityEngine;

[CreateAssetMenu(fileName = "ThreeLineShoot", menuName = "Skills/ThreeLineShoot")]
public class ThreeLineShoot : SkillBase
{
    [SerializeField] private int _attackPower;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private GameObject _bulletPrefab;

    public override void Execute(GameObject executor)
    {
        if (!CheckExecute()) return;

        Vector3 shootPosition = executor.transform.position + executor.transform.forward;
        Vector3 forward = executor.transform.forward;
        Vector3 right = executor.transform.right;
        
        IHitTarget executorHitTarget = executor.GetComponent<IHitTarget>();
        int generatorID = executorHitTarget?.GetGeneratorID() ?? 0;

        Bullet.Builder(_bulletPrefab, shootPosition, forward, _attackPower, _bulletSpeed, generatorID);
        Bullet.Builder(_bulletPrefab, shootPosition, (forward + right * 0.3f).normalized, _attackPower, _bulletSpeed, generatorID);
        Bullet.Builder(_bulletPrefab, shootPosition, (forward - right * 0.3f).normalized, _attackPower, _bulletSpeed, generatorID);
        
        StartCooldown();
    }
}