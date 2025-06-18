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
        return target.GetHPPercentage();
    }
}