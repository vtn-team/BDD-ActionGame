using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected virtual void Start()
    {
        // Ensure Box Collider is present
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        
        // Set as trigger for collision detection
        boxCollider.isTrigger = true;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Handle collision - to be implemented by derived classes
        HandleCollision(other);
    }
    
    protected abstract void HandleCollision(Collider other);
}