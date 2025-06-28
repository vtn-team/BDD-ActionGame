#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(StageCreator))]
public class StageCreatorEditor : Editor
{
    private void ClearStage(StageCreator stageCreator)
    {
        for (int i = stageCreator.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(stageCreator.transform.GetChild(i).gameObject);
        }
        
        // Also clear enemies that might not be children of StageCreator
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies)
        {
            if (enemy != null)
            {
                DestroyImmediate(enemy.gameObject);
            }
        }
    }
    
    private bool ValidateStageSettings(StageCreator stageCreator)
    {
        SerializedObject serializedObject = new SerializedObject(stageCreator);
        
        // Get stage size
        Vector2Int stageSize = stageCreator.StageSize;
        
        // Validate player position
        SerializedProperty playerPos = serializedObject.FindProperty("_playerPos");
        if (playerPos != null)
        {
            Vector2Int pos = playerPos.vector2IntValue;
            if (pos.x < 0 || pos.x >= stageSize.x || pos.y < 0 || pos.y >= stageSize.y)
            {
                EditorUtility.DisplayDialog("Validation Error", 
                    $"Player position ({pos.x}, {pos.y}) is outside stage bounds (0-{stageSize.x-1}, 0-{stageSize.y-1}).", 
                    "OK");
                return false;
            }
        }
        
        // Get enemy list
        SerializedProperty enemyListProp = serializedObject.FindProperty("_enemyList");
        if (enemyListProp != null && enemyListProp.arraySize > 0)
        {
            HashSet<Vector2Int> enemyPositions = new HashSet<Vector2Int>();
            
            for (int i = 0; i < enemyListProp.arraySize; i++)
            {
                SerializedProperty enemyData = enemyListProp.GetArrayElementAtIndex(i);
                SerializedProperty enemyPos = enemyData.FindPropertyRelative("enemyPos");
                SerializedProperty enemyDir = enemyData.FindPropertyRelative("enemyDir");
                
                Vector2Int pos = enemyPos.vector2IntValue;
                
                // Check bounds
                if (pos.x < 0 || pos.x >= stageSize.x || pos.y < 0 || pos.y >= stageSize.y)
                {
                    EditorUtility.DisplayDialog("Validation Error", 
                        $"Enemy {i+1} position ({pos.x}, {pos.y}) is outside stage bounds (0-{stageSize.x-1}, 0-{stageSize.y-1}).", 
                        "OK");
                    return false;
                }
                
                // Check duplicates
                if (enemyPositions.Contains(pos))
                {
                    EditorUtility.DisplayDialog("Validation Error", 
                        $"Duplicate enemy position found at ({pos.x}, {pos.y}). Please remove duplicates.", 
                        "OK");
                    return false;
                }
                
                enemyPositions.Add(pos);
            }
            
            // Check if player and enemy positions overlap
            Vector2Int playerPosition = playerPos.vector2IntValue;
            if (enemyPositions.Contains(playerPosition))
            {
                EditorUtility.DisplayDialog("Validation Error", 
                    $"Player position ({playerPosition.x}, {playerPosition.y}) overlaps with an enemy position.", 
                    "OK");
                return false;
            }
        }
        
        return true;
    }
    
    private void GenerateStageWithRetry(StageCreator stageCreator, int maxRetries = 3)
    {
        if (!ValidateStageSettings(stageCreator))
        {
            return;
        }
        
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                if (attempt > 0)
                {
                    Debug.Log($"Stage generation attempt {attempt + 1}/{maxRetries}");
                }
                
                stageCreator.GenerateStage();
                
                Debug.Log("Stage generated successfully!");
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Stage generation failed on attempt {attempt + 1}: {e.Message}");
                
                if (attempt < maxRetries - 1)
                {
                    // Clear partial stage before retry
                    ClearStage(stageCreator);
                }
                else
                {
                    EditorUtility.DisplayDialog("Stage Generation Failed", 
                        $"Failed to generate stage after {maxRetries} attempts. Last error: {e.Message}", 
                        "OK");
                }
            }
        }
    }
    
    private void RegenerateFieldsWithRetry(StageCreator stageCreator, int maxRetries = 3)
    {
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                if (attempt > 0)
                {
                    Debug.Log($"Field regeneration attempt {attempt + 1}/{maxRetries}");
                }
                
                stageCreator.RegenerateFieldsOnly();
                
                Debug.Log("Fields regenerated successfully!");
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Field regeneration failed on attempt {attempt + 1}: {e.Message}");
                
                if (attempt < maxRetries - 1)
                {
                    // Clear partial fields before retry
                    ClearFields(stageCreator);
                }
                else
                {
                    EditorUtility.DisplayDialog("Field Regeneration Failed", 
                        $"Failed to regenerate fields after {maxRetries} attempts. Last error: {e.Message}", 
                        "OK");
                }
            }
        }
    }
    
    private void ClearFields(StageCreator stageCreator)
    {
        for (int i = stageCreator.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = stageCreator.transform.GetChild(i).gameObject;
            if (child.GetComponent<GroundBase>() != null)
            {
                DestroyImmediate(child);
            }
        }
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        StageCreator stageCreator = (StageCreator)target;
        
        GUILayout.Space(10);
        
        // Stage generation section
        EditorGUILayout.LabelField("Stage Generation", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Generate Stage", GUILayout.Height(40)))
        {
            if (EditorUtility.DisplayDialog("Generate Stage", 
                "This will delete all existing stage objects and generate a new stage. Continue?", 
                "Yes", "Cancel"))
            {
                GenerateStageWithRetry(stageCreator);
                EditorUtility.SetDirty(stageCreator);
                SceneView.RepaintAll();
            }
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Regenerate Fields Only", GUILayout.Height(35)))
        {
            if (EditorUtility.DisplayDialog("Regenerate Fields", 
                "This will delete existing ground fields and generate new ones. Enemies and player will remain. Continue?", 
                "Yes", "Cancel"))
            {
                RegenerateFieldsWithRetry(stageCreator);
                EditorUtility.SetDirty(stageCreator);
                SceneView.RepaintAll();
            }
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Clear Stage", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Clear Stage", 
                "This will delete all stage objects. Continue?", 
                "Yes", "Cancel"))
            {
                ClearStage(stageCreator);
                EditorUtility.SetDirty(stageCreator);
                SceneView.RepaintAll();
            }
        }
        
        GUILayout.Space(10);
        
        // Validation section
        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Validate Settings", GUILayout.Height(25)))
        {
            if (ValidateStageSettings(stageCreator))
            {
                EditorUtility.DisplayDialog("Validation Result", "All settings are valid!", "OK");
            }
        }
        
        GUILayout.Space(10);
        
        // Help section
        EditorGUILayout.HelpBox(
            "Generate Stage: Creates new stage with validation and retry logic (max 3 attempts)\n" +
            "Regenerate Fields Only: Regenerates ground fields only, keeping enemies and player\n" +
            "Clear Stage: Removes all stage objects\n" +
            "Validate Settings: Checks for position bounds, duplicates, and overlaps\n\n" +
            "Enemy Direction Options:\n" +
            "• Random: Random facing direction\n" +
            "• Player: Face towards player position\n" +
            "• Left/Up/Down/Right: Face specific direction",
            MessageType.Info);
    }
}
#endif