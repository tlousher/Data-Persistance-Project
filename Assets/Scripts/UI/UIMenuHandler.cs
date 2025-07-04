
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIMenuHandler : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private void Start()
    {
        nameText.text = DataManager.Data.Name;
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void OnEndEdit()
    {
        DataManager.Data.Name = GetComponent<InputField>().text;
    }
}
