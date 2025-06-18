using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IHitTarget
{
    [SerializeField] private bool _canMove = true;
    [SerializeField] private int _generatorID;
    [SerializeField] private int _maxHitPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveInterval;
    [SerializeField, SerializeReference, SubclassSelector] private SkillBase[] _skillList = new SkillBase[4];
    
    private int _hitPoint;

    private bool _isMoving;
    private float _actionCooldown;
    private float _swampInhibition;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private StageCreator _stageCreator;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _stageCreator = FindFirstObjectByType<StageCreator>();
        
        // Initialize hitPoint with maxHitPoint
        _hitPoint = _maxHitPoint;
        
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }
    

    private void Update()
    {
        if (_actionCooldown > 0)
        {
            _actionCooldown -= Time.deltaTime;
        }
        
        if (_swampInhibition > 0)
        {
            _swampInhibition -= Time.deltaTime;
        }

        for (int i = 0; i < _skillList.Length; i++)
        {
            if (_skillList[i] != null)
            {
                _skillList[i].UpdateCooldown(Time.deltaTime);
            }
        }
        
        // Handle input using polling instead of callbacks
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (_playerInput == null) return;
        
        // Handle movement input
        Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f && _canMove && !_isMoving && _actionCooldown <= 0 && _swampInhibition <= 0)
        {
            // Right or D key input moves to X+
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            
            // Set transform.forward to the last input direction, snapped to X/Z axes
            if (moveDirection != Vector3.zero)
            {
                transform.forward = SnapToAxisDirection(moveDirection.normalized);
            }
            
            Move(moveDirection.normalized);
        }
        
        // Handle skill inputs
        if (_playerInput.actions["Attack01"].WasPressedThisFrame())
        {
            OnSkill(0);
        }
        if (_playerInput.actions["Attack02"].WasPressedThisFrame())
        {
            OnSkill(1);
        }
        if (_playerInput.actions["Attack03"].WasPressedThisFrame())
        {
            OnSkill(2);
        }
        if (_playerInput.actions["Attack04"].WasPressedThisFrame())
        {
            OnSkill(3);
        }
    }


    private bool IsValidPosition(Vector3 position)
    {
        if (_stageCreator == null) return true;
        
        Vector2Int stageSize = _stageCreator.StageSize;
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);
        
        // Check stage bounds
        if (x < 0 || x >= stageSize.x || z < 0 || z >= stageSize.y)
        {
            return false;
        }
        
        // Check for shield fields that block movement
        Vector3 groundCheckPosition = position + Vector3.down * 0.5f;
        Collider[] groundColliders = Physics.OverlapSphere(groundCheckPosition, 0.1f);
        
        foreach (Collider collider in groundColliders)
        {
            ShieldField shieldField = collider.GetComponent<ShieldField>();
            if (shieldField != null)
            {
                return false; // Cannot move to shield field
            }
        }
        
        return true;
    }
    
    private void Move(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        
        // Check if target position is within stage bounds
        if (!IsValidPosition(targetPosition))
        {
            return; // Ignore movement outside stage bounds
        }
        
        _isMoving = true;
        StartCoroutine(MoveCoroutine(targetPosition, direction));
        _actionCooldown = _moveInterval;
    }

    private System.Collections.IEnumerator MoveCoroutine(Vector3 targetPosition, Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;
        float duration = _moveSpeed;

        // Calculate rotation axis for dice-like rolling motion
        // When moving in transform.forward direction, rotate forward (around right axis)
        Vector3 rotationAxis;
        if (Vector3.Dot(direction, transform.forward) > 0.9f) // Moving forward
        {
            rotationAxis = transform.right; // Roll forward
        }
        else if (Vector3.Dot(direction, -transform.forward) > 0.9f) // Moving backward
        {
            rotationAxis = -transform.right; // Roll backward
        }
        else if (Vector3.Dot(direction, transform.right) > 0.9f) // Moving right
        {
            rotationAxis = -transform.forward; // Roll right
        }
        else if (Vector3.Dot(direction, -transform.right) > 0.9f) // Moving left
        {
            rotationAxis = transform.forward; // Roll left
        }
        else
        {
            // Fallback for diagonal or other directions
            rotationAxis = Vector3.Cross(Vector3.up, direction).normalized;
            if (rotationAxis.magnitude < 0.1f)
            {
                rotationAxis = Vector3.right;
            }
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            // Rotate 90 degrees around the appropriate axis (dice-like rotation)
            float rotationAngle = Mathf.Lerp(0f, 90f, t);
            transform.rotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);
            
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = startRotation * Quaternion.AngleAxis(90f, rotationAxis);
        _isMoving = false;
        
        // Check for ground interactions after movement
        CheckGroundInteraction();
    }
    
    private void CheckGroundInteraction()
    {
        Vector3 groundCheckPosition = transform.position + Vector3.down * 0.5f;
        Collider[] groundColliders = Physics.OverlapSphere(groundCheckPosition, 0.1f);
        
        foreach (Collider collider in groundColliders)
        {
            Swamp swamp = collider.GetComponent<Swamp>();
            if (swamp != null)
            {
                _swampInhibition = swamp.InhibitionTime;
                break;
            }
        }
    }

    private void OnSkill(int skillIndex)
    {
        if (_actionCooldown > 0 || skillIndex >= _skillList.Length || _skillList[skillIndex] == null) return;

        SkillBase skill = _skillList[skillIndex];
        if (skill.CheckExecute())
        {
            skill.Execute(gameObject);
            _actionCooldown = skill.NextActionInterval;
        }
    }

    public bool CheckDead()
    {
        return _hitPoint <= 0;
    }

    public bool Damage(int attackPower)
    {
        if (_isMoving && attackPower > 0) return false; // Only block positive damage when moving
        
        _hitPoint -= attackPower;
        
        // Clamp hitPoint to not exceed maxHitPoint (for healing)
        if (_hitPoint > _maxHitPoint)
        {
            _hitPoint = _maxHitPoint;
        }
        
        return true;
    }

    public int GetGeneratorID()
    {
        return _generatorID;
    }
    
    public float GetHPPercentage()
    {
        return _maxHitPoint > 0 ? (float)_hitPoint / _maxHitPoint : 0f;
    }
    
    public void TriggerGroundInteraction()
    {
        CheckGroundInteraction();
    }
    
    private Vector3 SnapToAxisDirection(Vector3 direction)
    {
        // Snap direction to the nearest cardinal direction (X or Z axis)
        float absX = Mathf.Abs(direction.x);
        float absZ = Mathf.Abs(direction.z);
        
        if (absX > absZ)
        {
            // X axis is dominant
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            // Z axis is dominant
            return direction.z > 0 ? Vector3.forward : Vector3.back;
        }
    }
}