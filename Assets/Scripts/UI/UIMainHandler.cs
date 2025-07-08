using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text bestHighScoreText;

    private void Start()
    {
        bestHighScoreText.text = DataManager.Data.HasName
            ? $"Best Score : {DataManager.Data.GetHighScoreName} : {DataManager.Data.HighScore}"
            : "No High Score";
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
