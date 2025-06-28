using UnityEngine;

public class ShieldField : GroundBase
{
    private void Start()
    {
        Vector3 shieldPosition = transform.position + Vector3.up * 0.5f;
        GameObject shieldBlockPrefab = Resources.Load<GameObject>("ShieldBlock");
        
        if (shieldBlockPrefab != null)
        {
            Instantiate(shieldBlockPrefab, shieldPosition, Quaternion.identity);
        }
    }
}