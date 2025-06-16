using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Result _result;

    private Player _playerComponent;
    private EnemyBase[] _enemies;

    private void Awake()
    {
        if (_player != null)
        {
            _playerComponent = _player.GetComponent<Player>();
        }
        
        _enemies = FindObjectsOfType<EnemyBase>();
    }

    private void Update()
    {
        bool allEnemiesDead = true;
        
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

        if (_playerComponent != null && _playerComponent.CheckDead())
        {
            _result.ShowResult("Game Over");
        }
        else if (allEnemiesDead)
        {
            _result.ShowResult("Player Wins");
        }
        
        _enemies = FindObjectsOfType<EnemyBase>();
    }
}