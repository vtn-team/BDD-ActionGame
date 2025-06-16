using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageCreator : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnData
    {
        public Vector2Int enemyPos;
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
                        Vector3 position = new Vector3(x, 0, y);
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
                GameObject enemy = Instantiate(enemyData.enemyPrefab, position, Quaternion.identity);
                enemy.name = $"Enemy_{enemyData.enemyPos.x}_{enemyData.enemyPos.y}";
            }
        }
    }

    private void MovePlayer()
    {
        if (_playerRef == null)
        {
            _playerRef = FindObjectOfType<Player>();
        }

        if (_playerRef != null)
        {
            Vector3 playerPosition = new Vector3(_playerPos.x, 0, _playerPos.y);
            _playerRef.transform.position = playerPosition;
        }
    }
#endif
}