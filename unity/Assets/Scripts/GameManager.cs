using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Result _result;

    private Player _playerComponent;
    private EnemyBase[] _enemies;
    private CinemachineTargetGroup _targetGroup;

    private void Awake()
    {
        if (_player != null)
        {
            _playerComponent = _player.GetComponent<Player>();
        }
        
        _enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        
        _targetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        if (_targetGroup != null)
        {
            if (_playerComponent != null)
            {
                _targetGroup.AddMember(_playerComponent.transform, 1f, 1f);
            }
            
            foreach (EnemyBase enemy in _enemies)
            {
                if (enemy != null)
                {
                    _targetGroup.AddMember(enemy.transform, 1f, 1f);
                }
            }
        }
    }

    private void Update()
    {
        bool allEnemiesDead = true;
        bool playerDead = false;
        
        // Check enemies first
        foreach (EnemyBase enemy in _enemies)
        {
            if (enemy != null && enemy.CheckDead())
            {
                Destroy(enemy.gameObject);
            }
            else if (enemy != null)
            {
                allEnemiesDead = false;
            }
        }
        
        // Check player after enemies
        if (_playerComponent != null && _playerComponent.CheckDead())
        {
            playerDead = true;
            Destroy(_playerComponent.gameObject);
        }

        // Determine result based on death order
        if (playerDead && allEnemiesDead)
        {
            _result.ShowResult("Draw");
        }
        else if (playerDead)
        {
            _result.ShowResult("Game Over");
        }
        else if (allEnemiesDead)
        {
            _result.ShowResult("Player Wins");
        }
        
        _enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
    }
}