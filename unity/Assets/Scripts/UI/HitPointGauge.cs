using UnityEngine;
using UnityEngine.UI;

public class HitPointGauge : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    
    private IHitTarget _target;
    private Canvas _canvas;
    private Camera _camera;
    
    public static HitPointGauge Builder(IHitTarget target)
    {
        // Load HitPointGauge prefab from Resources
        GameObject gaugePrefab = Resources.Load<GameObject>("HitPointGauge");
        if (gaugePrefab == null)
        {
            Debug.LogError("HitPointGauge prefab not found in Resources folder!");
            return null;
        }
        
        // Find Canvas and instantiate under it
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return null;
        }
        
        GameObject gaugeObject = Instantiate(gaugePrefab, canvas.transform);
        HitPointGauge gauge = gaugeObject.GetComponent<HitPointGauge>();
        
        if (gauge != null)
        {
            gauge._target = target;
            gauge._canvas = canvas;
            gauge._camera = Camera.main;
        }
        
        return gauge;
    }
    
    private void Awake()
    {
        _canvas = FindFirstObjectByType<Canvas>();
        _camera = Camera.main;
    }
    
    private void Update()
    {
        if (_target == null || _camera == null || _canvas == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Get target's head position (assuming MonoBehaviour for position)
        MonoBehaviour targetMono = _target as MonoBehaviour;
        if (targetMono == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 headPos = targetMono.transform.position + Vector3.up;
        
        // Convert 3D position to screen coordinates
        Vector3 screenPos = _camera.WorldToScreenPoint(headPos);
        
        // Check if target is behind camera or off-screen
        if (screenPos.z <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        
        // Convert screen position to UI position
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPos, _canvas.worldCamera, out uiPos);
        
        // Update position
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = uiPos;
        
        // Update HP percentage
        UpdateHPDisplay();
    }
    
    private void UpdateHPDisplay()
    {
        if (_target == null || _fillImage == null) return;
        
        // Get HP percentage from target
        float hpPercentage = GetHPPercentage(_target);
        
        // Update fill amount
        _fillImage.fillAmount = hpPercentage;
    }
    
    private float GetHPPercentage(IHitTarget target)
    {
        // This assumes the target has a way to get max HP and current HP
        // Since IHitTarget doesn't specify HP percentage method, we need to cast to known types
        if (target is Player player)
        {
            return GetPlayerHPPercentage(player);
        }
        else if (target is EnemyBase enemy)
        {
            return GetEnemyHPPercentage(enemy);
        }
        
        return 1f; // Default to full HP if type unknown
    }
    
    private float GetPlayerHPPercentage(Player player)
    {
        // Access private fields through reflection or add public properties
        // For now, assume HP is accessible (this will need to be updated based on actual implementation)
        System.Reflection.FieldInfo hitPointField = typeof(Player).GetField("_hitPoint", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo maxHitPointField = typeof(Player).GetField("_maxHitPoint", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (hitPointField != null && maxHitPointField != null)
        {
            int currentHP = (int)hitPointField.GetValue(player);
            int maxHP = (int)maxHitPointField.GetValue(player);
            return maxHP > 0 ? (float)currentHP / maxHP : 0f;
        }
        
        return 1f;
    }
    
    private float GetEnemyHPPercentage(EnemyBase enemy)
    {
        // Access private fields through reflection or add public properties
        System.Reflection.FieldInfo hitPointField = typeof(EnemyBase).GetField("_hitPoint", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo maxHitPointField = typeof(EnemyBase).GetField("_maxHitPoint", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (hitPointField != null && maxHitPointField != null)
        {
            int currentHP = (int)hitPointField.GetValue(enemy);
            int maxHP = (int)maxHitPointField.GetValue(enemy);
            return maxHP > 0 ? (float)currentHP / maxHP : 0f;
        }
        
        return 1f;
    }
}