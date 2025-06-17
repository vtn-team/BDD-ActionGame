using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EnemyDirection
{
    ENEMY_DIR_RANDOM,
    ENEMY_DIR_PLAYER,
    ENEMY_DIR_LEFT,
    ENEMY_DIR_UP,
    ENEMY_DIR_DOWN,
    ENEMY_DIR_RIGHT
}

public class StageCreator : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnData
    {
        public Vector2Int enemyPos;
        public EnemyDirection enemyDir;
        public GameObject enemyPrefab;
    }

    [System.Serializable]
    public struct CellGenerationRule
    {
        public CellList cellID;
        public int needNum;
    }

    [SerializeField] private int _stageWidth;
    [SerializeField] private int _stageHeight;
    [SerializeField] private GameObject[] _cellPrefabList;
    [SerializeField] private Vector2Int _playerPos;
    [SerializeField] private EnemySpawnData[] _enemyList;
    [SerializeField] private CellGenerationRule[] _cellRuleList;

    private Player _playerRef;
    
    public Vector2Int StageSize => new Vector2Int(_stageWidth, _stageHeight);

#if UNITY_EDITOR
    [ContextMenu("Generate Stage")]
    public void GenerateStage()
    {
        ClearExistingStage();
        GenerateCells();
        SpawnEnemies();
        MovePlayer();
    }

    private void ClearExistingStage()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void GenerateCells()
    {
        for (int x = 0; x < _stageWidth; x++)
        {
            for (int y = 0; y < _stageHeight; y++)
            {
                CellList cellType = DetermineCellType(x, y);
                if (cellType != CellList.INVALID_CELL && (int)cellType < _cellPrefabList.Length)
                {
                    GameObject cellPrefab = _cellPrefabList[(int)cellType];
                    if (cellPrefab != null)
                    {
                        Vector3 position = new Vector3(x, -0.5f, y);
                        GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                        cell.name = $"Cell_{x}_{y}_{cellType}";
                    }
                }
            }
        }
    }

    private CellList DetermineCellType(int x, int y)
    {
        return CellList.NORMAL_GROUND;
    }

    private void SpawnEnemies()
    {
        foreach (EnemySpawnData enemyData in _enemyList)
        {
            if (enemyData.enemyPrefab != null)
            {
                Vector3 position = new Vector3(enemyData.enemyPos.x, 0, enemyData.enemyPos.y);
                Quaternion rotation = GetEnemyRotation(enemyData.enemyDir, enemyData.enemyPos);
                GameObject enemy = Instantiate(enemyData.enemyPrefab, position, rotation);
                enemy.name = $"Enemy_{enemyData.enemyPos.x}_{enemyData.enemyPos.y}";
            }
        }
    }

    private Quaternion GetEnemyRotation(EnemyDirection direction, Vector2Int enemyPos)
    {
        switch (direction)
        {
            case EnemyDirection.ENEMY_DIR_LEFT:
                return Quaternion.Euler(0, 270, 0);
            case EnemyDirection.ENEMY_DIR_UP:
                return Quaternion.Euler(0, 0, 0);
            case EnemyDirection.ENEMY_DIR_DOWN:
                return Quaternion.Euler(0, 180, 0);
            case EnemyDirection.ENEMY_DIR_RIGHT:
                return Quaternion.Euler(0, 90, 0);
            case EnemyDirection.ENEMY_DIR_PLAYER:
                if (_playerRef != null)
                {
                    Vector3 dirToPlayer = new Vector3(_playerPos.x - enemyPos.x, 0, _playerPos.y - enemyPos.y);
                    if (dirToPlayer != Vector3.zero)
                    {
                        // Calculate angle and snap to 90-degree intervals (X-axis direction)
                        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
                        float snappedAngle = Mathf.Round(angle / 90f) * 90f;
                        return Quaternion.Euler(0, snappedAngle, 0);
                    }
                }
                return Quaternion.identity;
            case EnemyDirection.ENEMY_DIR_RANDOM:
                return Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            default:
                return Quaternion.identity;
        }
    }
    
    private void MovePlayer()
    {
        if (_playerRef == null)
        {
            _playerRef = FindFirstObjectByType<Player>();
        }

        if (_playerRef != null)
        {
            Vector3 playerPosition = new Vector3(_playerPos.x, 0, _playerPos.y);
            _playerRef.transform.position = playerPosition;
        }
    }
#endif
}