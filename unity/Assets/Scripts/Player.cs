using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IHitTarget
{
    [SerializeField] private bool _canMove = true;
    [SerializeField] private int _generatorID;
    [SerializeField] private int _hitPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveInterval;
    [SerializeField, SerializeReference, SubclassSelector] private SkillBase[] _skillList = new SkillBase[4];

    private bool _isMoving;
    private float _actionCooldown;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private StageCreator _stageCreator;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _stageCreator = FindFirstObjectByType<StageCreator>();
        
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
        if (moveInput.magnitude > 0.1f && _canMove && !_isMoving && _actionCooldown <= 0)
        {
            // Consider Y=90 rotation: right input should move forward
            Vector3 moveDirection = new Vector3(moveInput.y, 0, -moveInput.x);
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
        
        return x >= 0 && x < stageSize.x && z >= 0 && z < stageSize.y;
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

        // Calculate rotation axis based on movement direction (dice-like rotation)
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction).normalized;
        if (rotationAxis.magnitude < 0.1f) // Handle forward/backward movement
        {
            rotationAxis = Vector3.right;
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
        if (_isMoving) return false;
        
        _hitPoint -= attackPower;
        return true;
    }

    public int GetGeneratorID()
    {
        return _generatorID;
    }
}