using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugView : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.Label("Current Scene: " + SceneManager.GetActiveScene().name);
    }
}