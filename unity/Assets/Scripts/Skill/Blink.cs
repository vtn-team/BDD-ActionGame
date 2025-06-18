using UnityEngine;

[System.Serializable]
public class Blink : SkillBase
{
    [SerializeField] private int _blinkDistance = 3;
    
    public Blink()
    {
        _actionInterval = 0.3f;
        _coolTime = 6f;
    }
    
    public override void Execute(GameObject executor)
    {
        if (executor == null) return;
        
        StartCooldown();
        
        // Get forward direction and current position
        Transform executorTransform = executor.transform;
        Vector3 currentPosition = executorTransform.position;
        Vector3 forwardDirection = executorTransform.forward;
        
        // Calculate target position
        Vector3 targetPosition = currentPosition + forwardDirection * _blinkDistance;
        
        // Find optimal blink destination
        Vector3 blinkDestination = FindBlinkDestination(currentPosition, forwardDirection, targetPosition, executor);
        
        // Execute blink (instant movement)
        executorTransform.position = blinkDestination;
        
        // Apply ground effects after movement
        Player player = executor.GetComponent<Player>();
        if (player != null)
        {
            player.TriggerGroundInteraction();
        }
    }
    
    private Vector3 FindBlinkDestination(Vector3 startPos, Vector3 direction, Vector3 targetPos, GameObject executor)
    {
        // Check stage boundaries first
        StageCreator stageCreator = Object.FindFirstObjectByType<StageCreator>();
        if (stageCreator != null)
        {
            Vector2Int stageSize = stageCreator.StageSize;
            
            // Clamp target position to stage bounds
            targetPos.x = Mathf.Clamp(targetPos.x, 0, stageSize.x - 1);
            targetPos.z = Mathf.Clamp(targetPos.z, 0, stageSize.y - 1);
        }
        
        // Check each step along the path for obstacles
        Vector3 bestPosition = startPos;
        
        for (int step = 1; step <= _blinkDistance; step++)
        {
            Vector3 checkPosition = startPos + direction * step;
            
            // Ensure position is within stage bounds
            if (stageCreator != null)
            {
                Vector2Int stageSize = stageCreator.StageSize;
                if (checkPosition.x < 0 || checkPosition.x >= stageSize.x || 
                    checkPosition.z < 0 || checkPosition.z >= stageSize.y)
                {
                    break; // Stop at stage boundary
                }
            }
            
            // Check for obstacles at this position
            if (IsPositionBlocked(checkPosition, executor))
            {
                break; // Stop before obstacle
            }
            
            // Position is clear, update best position
            bestPosition = checkPosition;
        }
        
        return bestPosition;
    }
    
    private bool IsPositionBlocked(Vector3 position, GameObject executor)
    {
        // Check for any colliders at this position (excluding the executor itself)
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f);
        
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            
            // Skip the executor itself
            if (obj == executor) continue;
            
            // Check for blocking objects
            if (obj.GetComponent<Player>() != null ||
                obj.GetComponent<EnemyBase>() != null ||
                obj.GetComponent<ShieldBlock>() != null)
            {
                return true;
            }
            
            // Check for ShieldField ground (blocks movement)
            if (obj.GetComponent<ShieldField>() != null)
            {
                return true;
            }
        }
        
        return false;
    }
}