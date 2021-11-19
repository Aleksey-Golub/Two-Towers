using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : BaseStartQuitGamePanel
{
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        GameManager.Instance.RegisterGameOverPanel(this);
        gameObject.SetActive(false);
    }
    
    public void SetWinner(Castle winner)
    {
        _text.text = $"Player {winner.Spawner.TeamIndex} \n Win!";
    }

    public virtual void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
