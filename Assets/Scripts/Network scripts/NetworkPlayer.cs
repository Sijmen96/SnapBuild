using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public GameObject cameraPivot;
    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            this.cameraPivot.SetActive(true);
            this.GetComponentInChildren<PlayerMovementController>().enabled = true;
        }
    }
}
