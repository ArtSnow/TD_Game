using System;
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
        createGameButton.interactable = false;
        findGameButton.interactable = false;
        inputCode.interactable = false;
        int answer = await mp.StartGame();
        if (answer == 1)
        {
            SceneManager.LoadScene("Game");
        }
        inputCode.interactable = true;
        createGameButton.interactable = true;
        findGameButton.interactable = true;
    }

    private async void FindGame()
    {
        createGameButton.interactable = false;
        findGameButton.interactable = false;
        inputCode.interactable = false;
        int answer = await mp.FindGame(inputCode.text.ToUpper());
        if (answer == 1)
        {
            SceneManager.LoadScene("Game");
        }
        inputCode.interactable = true;
        createGameButton.interactable = true;
        findGameButton.interactable = true;
    }
}
