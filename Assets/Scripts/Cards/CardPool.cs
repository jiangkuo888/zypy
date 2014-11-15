using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPool {

	Dictionary<int, Card> cardPool = new Dictionary<int, Card>();

	public void addCard(Card card){
		cardPool.Add(card.ID, card);
	}

	public static Dictionary<int, Card> generateGenericCardPool(int totalCardsNumber){
		Dictionary<int, Card> myCardPool = new Dictionary<int, Card>();
		for (int i = 1; i <= totalCardsNumber; i++){
			Card card = new Card(i, "no src");
			myCardPool.Add(i, card);
		}
		return myCardPool;
	}

}

