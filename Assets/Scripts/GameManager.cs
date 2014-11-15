using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameStage {
	
	waiting, distributeToAll, hostSelectQuestion, othersSelectFake, vote, resultRelease, showScore
}

public class GameManager : MonoBehaviour {
	PhotonView myView;
	
	public static GameManager SP;
	
	public GameStage currentStage;
	public bool isRoundHost;
	public int currentHostIndex;
	public string currentRoundHost;
	
	
	public List<string> PlayerList;
	
	public GameObject roomView;
	public GameObject playerDeckView;
	public GameObject cardDeckView;
	public GameObject hudView;
	public GameObject result;
	public GameObject hudSelectButton;
	public GameObject hudVoteButton;
	
	public PlayerDeckViewController MyDeckController;
	public CardDeckViewController CardDeckController;
	
	
	public bool hostSelected;
	public bool allPlayerSelected;
	public bool allPlayerVoted;
	// Use this for initialization
	void Start () {
		SP = this;
		currentStage = GameStage.waiting;
		AllPlayerStageUpdate (currentStage);
		myView = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		//master client check game status
		if (PhotonNetwork.connected && PhotonNetwork.isMasterClient) {
			
			
			switch(currentStage)
			{
				
			case GameStage.hostSelectQuestion:
				
				if(hostSelected)
				{
					print ("host finally selected.");
					myView.RPC ("updateStage", PhotonTargets.AllBuffered, (int)GameStage.othersSelectFake);
				}
				break;
				
			case GameStage.othersSelectFake:
				
				if(allPlayerSelected)
				{
					print ("finally all selected.");
					myView.RPC ("updateStage",PhotonTargets.AllBuffered, (int)GameStage.vote);
					
				}
				
				break;
			case GameStage.vote:
				if(allPlayerVoted)
				{
					print ("all voted.");
					myView.RPC ("updateStage",PhotonTargets.AllBuffered, (int)GameStage.resultRelease);
				}
				break;
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
			}
			
			
			
			
			
			
			
			
			
			
			
			
		}
		
		
		
		
	}
	
	// only for master client
	public void StartNewRound()
	{
		bool validName = true;
		for (int i =0; i<PlayerList.Count; i++) {
			if (PlayerList [i].Trim() == "")
				validName = false;
		}
		if (validName) {
			
			CardManager.SP.InitBothCardsList();
			
			//			Debug.LogError("All names are valid");
			myView.RPC ("updateStage", PhotonTargets.All, (int)GameStage.distributeToAll);
		}
		
		
	}
	
	
	public void updatePlayerList()
	{
		// keep all player in list
		PlayerList = new List<string> ();
		for (int i=0; i<PhotonNetwork.playerList.Length; i++) {
			PlayerList.Add(PhotonNetwork.playerList[i].name);
		}
	}
	
	void AllPlayerStageUpdate(GameStage Stage)  
	{
		
		currentStage = Stage;
		
		switch (Stage) 
		{
		case GameStage.waiting:
			
			// get full players list
			
			break;
		case GameStage.distributeToAll:
			
			// decide game round host, start from server host
			if(PhotonNetwork.isMasterClient)
			{
				
				getHostAndTellOthers();
				
				
				// generate player card deck 
				string cardsInfo = CardManager.SP.getPlayerCardString();
				// pass the decks to all players
				myView.RPC ("updatePlayerCards",PhotonTargets.AllBuffered,cardsInfo);
			}
			
			
			break;
		case GameStage.hostSelectQuestion:
			
			
			break;
		case GameStage.othersSelectFake:
			
			break;
		case GameStage.vote:
			if(PhotonNetwork.isMasterClient)
			{
				myView.RPC ("updateDeckCards",PhotonTargets.AllBuffered,CardManager.SP.getCardDeckString());
			}


			break;
			
		default:
			break;
		}
		
		
		UpdateUI ();
	}
	
	
	public void UpdateUI()  // game UI changes
	{
		switch (currentStage) 
		{
		case GameStage.waiting:
			NGUITools.SetActive(roomView,true);
			NGUITools.SetActive(playerDeckView,false);
			NGUITools.SetActive(cardDeckView,false);
			NGUITools.SetActive(hudView,false);
			NGUITools.SetActive(result,false);
			
			// get full players list
			
			break;
		case GameStage.distributeToAll:
			NGUITools.SetActive(roomView,false);
			NGUITools.SetActive(playerDeckView,true);
			NGUITools.SetActive(cardDeckView,false);
			NGUITools.SetActive(hudView,false);
			NGUITools.SetActive(result,false);
			
			break;
		case GameStage.hostSelectQuestion:
			NGUITools.SetActive(roomView,false);
			NGUITools.SetActive(playerDeckView,true);
			NGUITools.SetActive(cardDeckView,false);
			NGUITools.SetActive(hudView,true);
			if(isRoundHost)
				NGUITools.SetActive(hudSelectButton,true);
			else
				NGUITools.SetActive(hudSelectButton,false);
			NGUITools.SetActive(hudVoteButton,false);
			NGUITools.SetActive(result,false);
			
			
			break;
		case GameStage.othersSelectFake:
			NGUITools.SetActive(roomView,false);
			NGUITools.SetActive(playerDeckView,true);
			NGUITools.SetActive(cardDeckView,false);
			NGUITools.SetActive(hudView,true);
			if(isRoundHost)
				NGUITools.SetActive(hudSelectButton,false);
			else
				NGUITools.SetActive(hudSelectButton,true);
			NGUITools.SetActive(hudVoteButton,false);
			NGUITools.SetActive(result,false);
			
			break;
		case GameStage.vote:
			NGUITools.SetActive(roomView,false);
			NGUITools.SetActive(playerDeckView,false);
			NGUITools.SetActive(cardDeckView,true);
			NGUITools.SetActive(hudView,true);
			if(isRoundHost)
				NGUITools.SetActive(hudVoteButton,false);
			else
				NGUITools.SetActive(hudVoteButton,true);
			NGUITools.SetActive(hudSelectButton,false);
			NGUITools.SetActive(result,false);
			



			break;
		case GameStage.resultRelease:
			NGUITools.SetActive(roomView,false);
			NGUITools.SetActive(playerDeckView,false);
			NGUITools.SetActive(cardDeckView,false);
			NGUITools.SetActive(hudView,false);
			NGUITools.SetActive(hudVoteButton,false);
			NGUITools.SetActive(hudSelectButton,false);

			NGUITools.SetActive(result,true);
			
		

			
			
			
			break;
		case GameStage.showScore:
			break;
			
		default:
			break;
		}
		
	}
	
	public void getHostAndTellOthers()
	{
		
		if (currentRoundHost == "") 
		{
			currentHostIndex =0;
			currentRoundHost = PlayerList [currentHostIndex];
		}
		else
		{
			currentHostIndex ++;
			currentRoundHost = PlayerList [currentHostIndex];
		}
		
		myView.RPC ("setRoundHost", PhotonTargets.All, currentRoundHost);
		
	}
	
	public void tellHostISelected(string playername, string cardSelected)
	{
		if (playername == currentRoundHost) {
			hostSelected = true;
		}


		
		myView.RPC ("addCardToDeck", PhotonTargets.AllBuffered, playername+"/"+cardSelected);
		
	}

	public void tellHostIVoted(string playername, string cardVoted)
	{
		myView.RPC ("addPlayerVote", PhotonTargets.AllBuffered, playername + "/" + cardVoted);
	}


	[RPC]
	public void updateDeckCards(string cardDeckInfoString)
	{
		CardDeckController.updateDeck(cardDeckInfoString);

	}

	
	[RPC]
	public void addCardToDeck(string playerCardInfo)
	{
		if (PhotonNetwork.isMasterClient) {
			CardManager.SP.addCardToDeck (playerCardInfo);
			
			if(CardManager.SP.CardDeck.Count == PhotonNetwork.playerList.Length)
				allPlayerSelected = true;
		}
		
	}
	[RPC]
	public void addPlayerVote(string playerVoteInfo)
	{

		CardManager.SP.addToVoteList(playerVoteInfo);

		if (PhotonNetwork.isMasterClient) {


			if(CardManager.SP.VoteList.Count == PhotonNetwork.playerList.Length -1)
			{
				allPlayerVoted = true;
			}
		}
	}
	
	[RPC]
	public void setRoundHost(string hostName)
	{
		if (hostName == PhotonNetwork.playerName) { // you are the chosen one
			isRoundHost = true;		
			AllPlayerStageUpdate(GameStage.hostSelectQuestion);
			
			
			
			myView.RPC ("updateStage", PhotonTargets.OthersBuffered, (int)GameStage.hostSelectQuestion);
		}
		else
		{
			isRoundHost = false;
		}
		
		currentRoundHost = hostName;
		
	}
	
	[RPC]
	public void updateStage(int nextStage)
	{
		
		AllPlayerStageUpdate((GameStage)nextStage);
		
	}
	[RPC]
	public void updatePlayerCards(string cardInfo)
	{
		
		string[] cardNumbers = null;
		
		string[] playerWithCardNumbers = cardInfo.Split (")"[0]);
		
		for (int i=0; i<playerWithCardNumbers.Length; i++) {
			if(playerWithCardNumbers[i].Contains(PhotonNetwork.playerName))
			{
				string[] nameAndCard = playerWithCardNumbers[i].Split("("[0]);
				cardNumbers = nameAndCard[1].Split("/"[0]);
				
				
				
			}
		}
		
		
		if (cardNumbers.Length != 0) {
			for(int i=0;i<cardNumbers.Length;i++)
				//				print (cardNumbers[i]);
				
				MyDeckController.updateMyDeck(cardNumbers);
		}
		
		
		
	}
	
	
	
}
