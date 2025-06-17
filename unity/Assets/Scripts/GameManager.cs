using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
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
        
        // Generate HitPointGauge for Player and Enemies
        GenerateHitPointGauges();
    }
    
    private void GenerateHitPointGauges()
    {
        // Create HitPointGauge for Player
        if (_playerComponent != null)
        {
            HitPointGauge.Builder(_playerComponent);
        }
        
        // Create HitPointGauge for each Enemy
        foreach (EnemyBase enemy in _enemies)
        {
            if (enemy != null)
            {
                HitPointGauge.Builder(enemy);
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
            if (enemy != null && enemy.gameObject.activeInHierarchy && enemy.CheckDead())
            {
                enemy.gameObject.SetActive(false);
            }
            else if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                allEnemiesDead = false;
            }
        }
        
        // Check player after enemies
        if (_playerComponent != null && _playerComponent.gameObject.activeInHierarchy && _playerComponent.CheckDead())
        {
            playerDead = true;
            _playerComponent.gameObject.SetActive(false);
        }

        // Determine result based on game design spec (general.md)
        // Victory: All enemies eliminated
        // Defeat: Player eliminated
        if (playerDead && allEnemiesDead)
        {
            _result.ShowResult("Draw");
        }
        else if (playerDead)
        {
            _result.ShowResult("Defeat");
        }
        else if (allEnemiesDead)
        {
            _result.ShowResult("Victory");
        }
        
        _enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
    }
}