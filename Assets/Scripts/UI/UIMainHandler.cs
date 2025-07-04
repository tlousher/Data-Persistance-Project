using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainHandler : MonoBehaviour
{
    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
