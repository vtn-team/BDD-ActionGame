using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.Label("Current Scene: " + SceneManager.GetActiveScene().name);
    }
}