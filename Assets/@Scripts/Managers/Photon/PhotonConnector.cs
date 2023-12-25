using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;

public class PhotonConnector : PhotonSingleton<PhotonConnector>
{
    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
    [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
    public byte MaxPlayers = 4;

    public int playerTTL = -1;

    public event Action OnComplete;

    public void ConnectNow()
    {
        Debug.Log($"{nameof(PhotonConnector)}.{nameof(ConnectNow)}() will now call: {nameof(PhotonNetwork)}.{nameof(PhotonNetwork.ConnectUsingSettings)}");

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
    }

    // below, we implement some callbacks of the Photon Realtime API.
    // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room.");
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;
        PhotonNetwork.JoinOrCreateRoom("master", roomOptions, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        OnComplete?.Invoke();
    }
}