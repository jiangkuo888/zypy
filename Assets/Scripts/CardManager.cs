using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {
	
	public static CardManager SP;
	public int TotalCardNumber;
	
	
	public List<int> UsedCards;
	public List<int> NotUsedCards;
	
	
	public List<string> CardDeck = new List<string>();

	public List<string> VoteList = new List<string>();


	public string CorrectHostCardNumber;
	// Use this for initialization
	void Start () {
		SP = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public void InitBothCardsList()
	{
		NotUsedCards = new List<int> ();
		
		for (int i=0; i<TotalCardNumber; i++)
			NotUsedCards.Add (i + 1);
		
		
		UsedCards = new List<int>();
	}
	
	
	public string getPlayerCardString()
	{
		string cardString = "";
		for (int i=0; i<GameManager.SP.PlayerList.Count; i++) 
		{
			cardString += GameManager.SP.PlayerList[i] +"("+getNumbers(7)+")";
		}
		
		return cardString;
	}
	
	
	public string getNumbers(int num)
	{
		string temp = "";
		int ranIndex = 0;
		for (int i=0; i<num; i++) 
		{
			
			ranIndex = Random.Range(0,NotUsedCards.Count-1);
			
			
			
			temp += NotUsedCards[ranIndex]+"/";
			
			
			UsedCards.Add(NotUsedCards[ranIndex]);
			NotUsedCards.RemoveAt(ranIndex);
			
		}
		
		return temp;
	}
	
	public void addCardToDeck(string playerCardInfo)
	{
		if (CardDeck.Count >= PhotonNetwork.playerList.Length)
						return;
				else if (CardDeck.Count == 0) {
			CorrectHostCardNumber = playerCardInfo;
				}

			
		CardDeck.Add (playerCardInfo);
	}

	public string getCardDeckString()
	{
		string temp = "";
		for (int i=0; i<CardDeck.Count; i++) {
			temp += CardDeck[i]+")";
				}

		return temp;
	}


	public void addToVoteList(string playerAndVote)
	{
		VoteList.Add (playerAndVote);
	}
}
