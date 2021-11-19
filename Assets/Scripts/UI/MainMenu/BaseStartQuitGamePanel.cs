using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseStartQuitGamePanel : MonoBehaviour
{
    public virtual void QuitGame()
    {
        Application.Quit();
    }

    public virtual void StartSingleGame()
    {
        SceneManager.LoadScene(1);
    }
}
