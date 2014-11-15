using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDeck {

	List<Card> myCardDeck = new List<Card>();
	List<Card> usedDeck = new List<Card>();

	public void initializeCardDeck(Dictionary<int, Card> cardPool){
		myCardDeck.Clear();
		usedDeck.Clear();

		int cardsNum = cardPool.Count;

		List<int> indexArray = new List<int>();
		for (int i = 0; i < cardsNum; i++){
			indexArray.Add(i);
		}

		List<int> anotherArray = new List<int>();

		while(indexArray.Count > 0){
			int id = Random.Range(0, indexArray.Count);
			anotherArray.Add(indexArray[id]);
			indexArray.Remove(id);
		}

		for(int i = 0; i < indexArray.Count; i++){
			myCardDeck.Add(cardPool[indexArray[i]]);
		}
	}

	public void generateCardDeck(){
		List<Card> remainingDeck = new List<Card>();
		for (int i = 0; i < myCardDeck.Count; i++){
			remainingDeck.Add(myCardDeck[i]);
		}

		for (int j = 0; j < usedDeck.Count; j++){
			remainingDeck.Add(usedDeck[j]);
		}

		myCardDeck.Clear();
		usedDeck.Clear();

		int cardsNum = remainingDeck.Count;
		
		List<int> indexArray = new List<int>();
		for (int i = 0; i < cardsNum; i++){
			indexArray.Add(i);
		}
		
		List<int> anotherArray = new List<int>();
		
		while(indexArray.Count > 0){
			int id = Random.Range(0, indexArray.Count);
			anotherArray.Add(indexArray[id]);
			indexArray.Remove(id);
		}

		for(int i = 0; i < indexArray.Count; i++){
			myCardDeck.Add(remainingDeck[indexArray[i]]);
		}

		remainingDeck.Clear();
	}
}
