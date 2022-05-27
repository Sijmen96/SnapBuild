using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject singleplayer;
    public GameObject multiplayer;
    public GameObject settings;
    public GameObject exit;
    [Space]
    public GameObject multiplayerPanel;
    public GameObject host;
    public GameObject join;
    [Space]
    public GameObject hostPanel;
    public GameObject hostVariable;
    public GameObject startServer;
    [Space]
    public GameObject joinPanel;
    public GameObject hostCodeInput;
    public GameObject joinServer;

    private Allocation allocation;
    private JoinAllocation joinAllocation;
    void Start()
    {
        UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            AuthenticationService.Instance.SignInAnonymouslyAsync();
        }


        this.mainPanel.SetActive(true);
        this.multiplayerPanel.SetActive(false);
        this.hostPanel.SetActive(false);
        this.joinPanel.SetActive(false);


        Button singleplayerButton = singleplayer.GetComponent<Button>();
        Button multiplayerButton = multiplayer.GetComponent<Button>();
        Button settingsButton = settings.GetComponent<Button>();
        Button exitButton = exit.GetComponent<Button>();
        Button joinButton = join.GetComponent<Button>();
        Button hostButton = host.GetComponent<Button>();
        Button startServerButton = startServer.GetComponent<Button>();
        Button joinServerButton = joinServer.GetComponent<Button>();

        singleplayerButton.onClick.AddListener(onSingleplayerButtonClick);
        multiplayerButton.onClick.AddListener(onMultiplayerButtonClick);
        settingsButton.onClick.AddListener(onSettingsButtonClick);
        exitButton.onClick.AddListener(onExitButtonClick);
        joinButton.onClick.AddListener(onJoinButtonClick);
        hostButton.onClick.AddListener(onHostButtonClick);
        startServerButton.onClick.AddListener(onStartServerButtonClick);
        startServerButton.enabled = false;
        joinServerButton.onClick.AddListener(onJoinServerButtonClick);

    }

    private async void getHostAllocation()
    {
        this.allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(7);
        string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        //set relay data in static class
        RelayData.isHost = true;
        RelayData.IPv4Address = this.allocation.RelayServer.IpV4;
        RelayData.Port = (ushort) this.allocation.RelayServer.Port;

        RelayData.AllocationID = this.allocation.AllocationId;
        RelayData.AllocationIDBytes = this.allocation.AllocationIdBytes;
        RelayData.ConnectionData = this.allocation.ConnectionData;
        RelayData.Key = this.allocation.Key;


        //set code and activate button
        this.hostVariable.GetComponent<TextMeshProUGUI>().text = joinCode;
        startServer.GetComponent<Button>().enabled = true;
    }

    private void onMultiplayerButtonClick()
    {
        this.mainPanel.SetActive(false);
        this.multiplayerPanel.SetActive(true);
    }

    private void onHostButtonClick()
    {
        this.multiplayerPanel.SetActive(false);
        this.hostPanel.SetActive(true);
        getHostAllocation();
    }

    private void onJoinButtonClick()
    {
        this.joinServer.GetComponent<Button>().enabled = false;
        this.multiplayerPanel.SetActive(false);
        this.joinPanel.SetActive(true);
        this.hostCodeInput.GetComponent<TMP_InputField>().onValueChanged.AddListener((textbox) =>
        {
            if (textbox.Length > 0)
            {
                this.joinServer.GetComponent<Button>().enabled = true;
            }
        });
    }

    private void onStartServerButtonClick()
    {
        SceneManager.LoadScene("Multiplayer");
    }

    private async void onJoinServerButtonClick()
    {
        string joinCode = this.hostCodeInput.GetComponent<TMP_InputField>().text;
        joinAllocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

        //set relay data in static class
        RelayData.isHost = false;
        RelayData.IPv4Address = this.joinAllocation.RelayServer.IpV4;
        RelayData.Port = (ushort)this.joinAllocation.RelayServer.Port;

        RelayData.AllocationID = this.joinAllocation.AllocationId;
        RelayData.AllocationIDBytes = this.joinAllocation.AllocationIdBytes;
        RelayData.ConnectionData = this.joinAllocation.ConnectionData;
        RelayData.HostConnectionData = this.joinAllocation.HostConnectionData;
        RelayData.Key = this.joinAllocation.Key;


        //set code and activate button
        this.hostVariable.GetComponent<TextMeshProUGUI>().text = joinCode;
        startServer.GetComponent<Button>().enabled = true;
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