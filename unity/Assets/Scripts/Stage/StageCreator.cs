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
    public struct ItemSpawnData
    {
        public GameObject itemPrefab;
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
    [SerializeField] private ItemSpawnData[] _itemList;
    [SerializeField] private CellGenerationRule[] _cellRuleList;

    private Player _playerRef;
    private CellList[,] _cellLayout;
    
    public Vector2Int StageSize => new Vector2Int(_stageWidth, _stageHeight);
    
    private void Start()
    {
        // Generate items if item list is set up
        if (_itemList != null && _itemList.Length > 0)
        {
            SpawnItems();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Stage")]
    public void GenerateStage()
    {
        ClearExistingStage();
        GenerateCells();
        SpawnEnemies();
        MovePlayer();
    }
    
    [ContextMenu("Regenerate Fields Only")]
    public void RegenerateFieldsOnly()
    {
        ClearExistingFields();
        GenerateCells();
    }

    private void ClearExistingStage()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    
    private void ClearExistingFields()
    {
        // Clear only ground field objects (GroundBase derived objects)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.GetComponent<GroundBase>() != null)
            {
                DestroyImmediate(child);
            }
        }
    }

    private void GenerateCells()
    {
        // First, determine the layout for all cells
        GenerateCellLayout();
        
        // Then create the actual cell objects
        for (int x = 0; x < _stageWidth; x++)
        {
            for (int y = 0; y < _stageHeight; y++)
            {
                CellList cellType = _cellLayout[x, y];
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
    
    private void GenerateCellLayout()
    {
        _cellLayout = new CellList[_stageWidth, _stageHeight];
        
        // Calculate total available cells
        int totalCells = _stageWidth * _stageHeight;
        
        // Count how many cells are already assigned by rules
        int assignedCells = 0;
        foreach (CellGenerationRule rule in _cellRuleList)
        {
            assignedCells += rule.needNum;
        }
        
        // Calculate remaining cells for normal ground
        int normalGroundCells = totalCells - assignedCells;
        
        // Create weighted list based on cell generation rules
        System.Collections.Generic.List<CellList> cellPool = new System.Collections.Generic.List<CellList>();
        
        // Add cells based on rules
        foreach (CellGenerationRule rule in _cellRuleList)
        {
            for (int i = 0; i < rule.needNum; i++)
            {
                cellPool.Add(rule.cellID);
            }
        }
        
        // Fill remaining with normal ground
        for (int i = 0; i < normalGroundCells; i++)
        {
            cellPool.Add(CellList.NORMAL_GROUND);
        }
        
        // Shuffle the pool for random distribution
        for (int i = 0; i < cellPool.Count; i++)
        {
            CellList temp = cellPool[i];
            int randomIndex = Random.Range(i, cellPool.Count);
            cellPool[i] = cellPool[randomIndex];
            cellPool[randomIndex] = temp;
        }
        
        // Assign cells to the 2D layout
        int poolIndex = 0;
        for (int y = 0; y < _stageHeight; y++)
        {
            for (int x = 0; x < _stageWidth; x++)
            {
                if (poolIndex < cellPool.Count)
                {
                    _cellLayout[x, y] = cellPool[poolIndex];
                    poolIndex++;
                }
                else
                {
                    _cellLayout[x, y] = CellList.NORMAL_GROUND;
                }
            }
        }
    }

    private CellList DetermineCellType(int x, int y)
    {
        // Use the pre-generated layout if available
        if (_cellLayout != null && x >= 0 && x < _stageWidth && y >= 0 && y < _stageHeight)
        {
            return _cellLayout[x, y];
        }
        
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

    private void SpawnItems()
    {
        if (_itemList == null || _itemList.Length == 0) return;
        
        // Get list of valid positions (normal cells without enemies)
        System.Collections.Generic.List<Vector2Int> validPositions = GetValidItemPositions();
        
        // Spawn items randomly on valid positions
        foreach (ItemSpawnData itemData in _itemList)
        {
            if (itemData.itemPrefab != null && validPositions.Count > 0)
            {
                // Choose random position from valid positions
                int randomIndex = Random.Range(0, validPositions.Count);
                Vector2Int itemPos = validPositions[randomIndex];
                validPositions.RemoveAt(randomIndex); // Remove to avoid duplicate placement
                
                Vector3 position = new Vector3(itemPos.x, 0, itemPos.y);
                GameObject item = Instantiate(itemData.itemPrefab, position, Quaternion.identity);
                item.name = $"Item_{itemPos.x}_{itemPos.y}";
            }
        }
    }
    
    private System.Collections.Generic.List<Vector2Int> GetValidItemPositions()
    {
        System.Collections.Generic.List<Vector2Int> validPositions = new System.Collections.Generic.List<Vector2Int>();
        System.Collections.Generic.HashSet<Vector2Int> enemyPositions = new System.Collections.Generic.HashSet<Vector2Int>();
        
        // Collect enemy positions
        foreach (EnemySpawnData enemyData in _enemyList)
        {
            enemyPositions.Add(enemyData.enemyPos);
        }
        
        // Find all normal ground cells without enemies
        for (int x = 0; x < _stageWidth; x++)
        {
            for (int y = 0; y < _stageHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                
                // Check if position is normal ground and has no enemy
                CellList cellType = (_cellLayout != null) ? _cellLayout[x, y] : CellList.NORMAL_GROUND;
                if (cellType == CellList.NORMAL_GROUND && !enemyPositions.Contains(pos) && pos != _playerPos)
                {
                    validPositions.Add(pos);
                }
            }
        }
        
        return validPositions;
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