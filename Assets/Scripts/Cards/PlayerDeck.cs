using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDeck {

	public List<Card> myDeck;

	public PlayerDeck(){
		myDeck = new List<Card>();
	}

	public void clearDeck(){
		myDeck.Clear();
	}

	public void addCard(Card card){
		myDeck.Add(card);
	}
}
