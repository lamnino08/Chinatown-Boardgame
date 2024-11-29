using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainmenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField; 
    [SerializeField] private Button hostButton; 
    [SerializeField] private Button joinButton;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;

        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    private void OnHostClicked()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.LogError("Name is required!");
            return;
        }

        networkManager.StartHost();
        ChangeToLobby();
    }

    private void OnJoinClicked()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.LogError("Name is required!");
            return;
        }

        networkManager.StartClient(); 
        ChangeToLobby();
    }

    private void ChangeToLobby()
    {
        string name = nameInputField.text;
        PlayerPrefs.SetString("PlayerName", name);
    }
}
