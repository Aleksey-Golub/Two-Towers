using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    private List<Castle> _castles = new List<Castle>();

    public void RegisterCastle(Castle castle)
    {
        _castles.Add(castle);

        castle.Spawner.Enemy_castle.GameOver += OnGameOver;
    }

    private void OnGameOver(Castle winner)
    {
        _gameOverPanel.gameObject.SetActive(true);
        _gameOverPanel.SetWinner(winner);

        foreach (Castle c in _castles)
        {
            c.GameOver -= OnGameOver;
        }
    }
}
