using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _attackPower;
    private float _speed;
    private int _generatorID;
    private Vector3 _moveDirection;

    public static Bullet Builder(GameObject bulletPrefab, Vector3 position, Vector3 direction, int attackPower, float speed, int generatorID)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        
        bullet._attackPower = attackPower;
        bullet._speed = speed;
        bullet._generatorID = generatorID;
        bullet._moveDirection = direction.normalized;
        
        return bullet;
    }

    private void Update()
    {
        transform.position += _moveDirection * _speed * Time.deltaTime;
        
        if (Vector3.Distance(transform.position, Vector3.zero) > 100f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IHitTarget hitTarget = other.GetComponent<IHitTarget>();
        if (hitTarget != null && hitTarget.GetGeneratorID() != _generatorID)
        {
            if (hitTarget.Damage(_attackPower))
            {
                Destroy(gameObject);
            }
        }
    }
}