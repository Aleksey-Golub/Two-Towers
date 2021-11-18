using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _backToMainMenu;

    internal void SetWinner(Castle winner)
    {
        _text.text = $"Player {winner.Spawner.TeamIndex} \n Win!";
    }
}
