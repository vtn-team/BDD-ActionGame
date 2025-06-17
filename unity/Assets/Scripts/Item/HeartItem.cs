using UnityEngine;

public class HeartItem : ItemBase
{
    [SerializeField] private int _healAmount = 30; // From game design spec: heart.md
    
    protected override void HandleCollision(Collider other)
    {
        // Check if collided object has Player tag and IHitTarget component
        if (other.CompareTag("Player"))
        {
            IHitTarget hitTarget = other.GetComponent<IHitTarget>();
            if (hitTarget != null)
            {
                // Apply healing through negative damage
                Heal(hitTarget);
                
                // Destroy the item after use
                Destroy(gameObject);
            }
        }
    }
    
    private void Heal(IHitTarget hitTarget)
    {
        // Use negative damage to represent healing
        // The IHitTarget's Damage method should handle this appropriately
        hitTarget.Damage(-_healAmount);
    }
}