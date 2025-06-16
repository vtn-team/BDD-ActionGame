using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IHitTarget
{
    [SerializeField] private int _generatorID;
    [SerializeField] private int _hitPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveInterval;
    [SerializeField] private SkillBase[] _skillList = new SkillBase[4];

    private bool _isMoving;
    private float _actionCooldown;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Attack01"].performed += ctx => OnSkill(0);
        _playerInput.actions["Attack02"].performed += ctx => OnSkill(1);
        _playerInput.actions["Attack03"].performed += ctx => OnSkill(2);
        _playerInput.actions["Attack04"].performed += ctx => OnSkill(3);
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
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (_isMoving || _actionCooldown > 0) return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        
        if (moveDirection.magnitude > 0.1f)
        {
            Move(moveDirection.normalized);
        }
    }

    private void Move(Vector3 direction)
    {
        _isMoving = true;
        Vector3 targetPosition = transform.position + direction;
        StartCoroutine(MoveCoroutine(targetPosition, direction));
        _actionCooldown = _moveInterval;
    }

    private System.Collections.IEnumerator MoveCoroutine(Vector3 targetPosition, Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = 1f / _moveSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.Rotate(Vector3.up, 360f * Time.deltaTime * _moveSpeed);
            
            yield return null;
        }

        transform.position = targetPosition;
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