using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button findGameButton;
    [SerializeField] private TMP_InputField inputCode;
    [SerializeField] private Multiplayer mp;

    private void Awake()
    {
        createGameButton.onClick.AddListener(StartGame);
        findGameButton.onClick.AddListener(FindGame);
    }

    private async void StartGame()
    {
        int answer = await mp.StartGame();
        if (answer == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }

    private async void FindGame()
    {
        int answer = await mp.FindGame(inputCode.text);
        if (answer == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
