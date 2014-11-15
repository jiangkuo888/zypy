using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkHandler : MonoBehaviour {
	public string roomName = "JK's Room";
	public UIInput PlayerInput;
	public GameObject StartButton;
	public UILabel WaitingString;
	public UILabel PlayerNum;
	
	public bool AutoConnect = true;

	public PhotonView myView;

	public int PlayerNumber = 0;
	
	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;
	
	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
		NGUITools.SetActive (WaitingString.gameObject, false);


		myView = GetComponent<PhotonView> ();
	}
	
	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			//Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings("2."+Application.loadedLevel);
		}

		if (PlayerNumber != PhotonNetwork.playerList.Length) {
			UpdatePlayerList();

				}
		
	}
	
	// to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!
	
	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		//Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}
	
	public virtual void OnPhotonRandomJoinFailed()
	{
		//		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		
		Debug.Log ("Room created.");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 8 }, null);
		
		if(PhotonNetwork.isMasterClient)
			NGUITools.SetActive(StartButton,true);
	}
	
	// the following methods are implemented to give you some context. re-implement them as needed.
	
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}
	public void OnCreatedRoom()
	{
		UpdatePlayerList ();
	}


	public void OnJoinedRoom()
	{
		if(!PhotonNetwork.isMasterClient)
			NGUITools.SetActive(StartButton,false);
		
		Debug.Log ("Room joined.");

		GameManager.SP.updatePlayerList ();
		//Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}
	
	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
	}

	[RPC]
	public void UpdatePlayerList()
	{
		if (PhotonNetwork.connected) {
			PlayerNum.text = "Players: ";
			
			for (int i=0; i<PhotonNetwork.playerList.Length; i++) {
				PlayerNum.text += "\n" + PhotonNetwork.playerList [i].name;
			}
		}

		PlayerNumber = PhotonNetwork.playerList.Length;


		GameManager.SP.updatePlayerList ();
	}
	
	// submit local player name
	public void SetPlayerName()
	{
		if (PhotonNetwork.connected) {
			PhotonNetwork.playerName = PlayerInput.value;
//			print (PhotonNetwork.playerName);
		}
		
		NGUITools.SetActive (PlayerInput.gameObject, false);
		NGUITools.SetActive (WaitingString.gameObject, true);

		myView.RPC ("UpdatePlayerList", PhotonTargets.All);

	}

	
	public void OnApplicationQuit()
	{
		PhotonNetwork.LeaveRoom ();
	}
}
