using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text _resultText;
    [SerializeField] private GameObject _resultPanel;

    private void Awake()
    {
        if (_resultPanel != null)
        {
            _resultPanel.SetActive(false);
        }
    }

    public void ShowResult(string resultMessage)
    {
        if (_resultText != null)
        {
            _resultText.text = resultMessage;
        }
        
        if (_resultPanel != null)
        {
            _resultPanel.SetActive(true);
        }
    }
}