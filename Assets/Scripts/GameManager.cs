using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject networkManager;

    private void Start()
    {
        if (RelayData.Key == null)
            return;

        if (RelayData.isHost)
        {
            UnityTransport transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
            transport.SetRelayServerData(RelayData.IPv4Address, RelayData.Port, RelayData.AllocationIDBytes, RelayData.Key, RelayData.ConnectionData);
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            UnityTransport transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
            transport.SetRelayServerData(RelayData.IPv4Address, RelayData.Port, RelayData.AllocationIDBytes, RelayData.Key, RelayData.ConnectionData, RelayData.HostConnectionData);
            NetworkManager.Singleton.StartClient();
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        StatusLabels();
        GUILayout.EndArea();
    }


    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
             "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}