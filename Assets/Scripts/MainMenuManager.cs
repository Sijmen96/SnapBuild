using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject singleplayer;
    public GameObject multiplayer;
    public GameObject settings;
    public GameObject exit;
    void Start()
    {
        Button singleplayerButton = singleplayer.GetComponent<Button>();
        Button multiplayerButton = multiplayer.GetComponent<Button>();
        Button settingsButton = settings.GetComponent<Button>();
        Button exitButton = exit.GetComponent<Button>();

        singleplayerButton.onClick.AddListener(onSingleplayerButtonClick);
        multiplayerButton.onClick.AddListener(onMultiplayerButtonClick);
        settingsButton.onClick.AddListener(onSettingsButtonClick);
        exitButton.onClick.AddListener(onExitButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onMultiplayerButtonClick()
    {
        SceneManager.LoadScene("Multiplayer");
    }

    private void onExitButtonClick()
    {
        Application.Quit();
    }

    private void onSingleplayerButtonClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void onSettingsButtonClick()
    {

    }
}