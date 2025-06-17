using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _attackPower;
    private float _speed;
    private int _generatorID;
    private Vector3 _moveDirection;

    public static Bullet Builder(int attackPower, float speed, int generatorID, Vector3 position, Vector3 direction)
    {
        // Load bullet prefab from Resources
        GameObject bulletPrefab = Resources.Load<GameObject>("Bullet");
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not found in Resources folder!");
            return null;
        }
        
        // Spawn 1 cell ahead in movement direction
        Vector3 spawnPosition = position + direction.normalized;
        
        // Set rotation to face movement direction
        Quaternion rotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : Quaternion.identity;
        
        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        
        bullet._attackPower = attackPower;
        bullet._speed = speed;
        bullet._generatorID = generatorID;
        bullet._moveDirection = direction.normalized;
        
        return bullet;
    }

    private void Update()
    {
        // Move at speed units per second
        transform.position += _moveDirection * _speed * Time.deltaTime;
        
        // Check if outside stage bounds and destroy
        StageCreator stageCreator = FindFirstObjectByType<StageCreator>();
        if (stageCreator != null)
        {
            Vector2Int stageSize = stageCreator.StageSize;
            Vector3 pos = transform.position;
            
            if (pos.x < -1 || pos.x > stageSize.x || pos.z < -1 || pos.z > stageSize.y)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Fallback: destroy if too far from origin
            if (Vector3.Distance(transform.position, Vector3.zero) > 100f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        OnHit(other.gameObject);
    }
    
    private void OnHit(GameObject hitObject)
    {
        IHitTarget hitTarget = hitObject.GetComponent<IHitTarget>();
        if (hitTarget != null && hitTarget.GetGeneratorID() != _generatorID)
        {
            if (hitTarget.Damage(_attackPower))
            {
                Destroy(gameObject);
            }
        }
    }
}