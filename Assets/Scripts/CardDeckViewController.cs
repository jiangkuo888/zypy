using UnityEngine;
using System.Collections;
using System.Collections.Generic;	

public class CardDeckViewController : MonoBehaviour {
	public Transform cardPrefab;
	public UIGrid grid;
	public List<string> CardDeck = new List<string>();
	public List<UITexture> CardTextures = new List<UITexture>();
	
	public UIButton voteButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateDeck(string deckInfoString)
	{

		CardDeck.Clear ();

		//decrypt

		string[] eachCardString = deckInfoString.Split (")" [0]);

		string[] halfNameHalfCard = null;

		for(int i=0;i<eachCardString.Length-1;i++)
		{
			halfNameHalfCard = eachCardString[i].Split("/"[0]);
			CardDeck.Add(halfNameHalfCard[1]);

//			print (halfNameHalfCard[1]);
			
		}


		updateGrid ();
		
		updateCardDeckUI ();
	}


	public void updateGrid()
	{
		for (int i=0; i<CardDeck.Count; i++) {
			Transform newCard = Instantiate(cardPrefab,Vector3.zero,cardPrefab.rotation) as Transform;
			newCard.name = "Card"+CardDeck[i];
			newCard.parent  = grid.transform;
			newCard.localScale = new Vector3(1,1,1);
			CardTextures.Add(newCard.GetComponent<UITexture>());

				}

		grid.Reposition ();
	}

	public void updateCardDeckUI()
	{
		string path = "card/";
		
		if (CardDeck.Count != 0) {
			for(int i=0;i<CardDeck.Count;i++)
			{
				CardTextures[i].mainTexture =  Resources.Load(path+CardDeck[i]) as Texture;
			}
		}
	}

	public void VoteCard(){
		
		UIGrid grid = GetComponentInChildren<UIGrid> ();
		
		string cardNumber = grid.GetComponent<UICenterOnChild> ().mCenteredObject.GetComponent<UITexture>().mainTexture.name;
		
		grid.GetComponent<UICenterOnChild> ().mCenteredObject.GetComponentInChildren<UISprite> ().enabled = true;
		
		voteButton.isEnabled = false;
		
		print (cardNumber +" is voted");
		
		GameManager.SP.tellHostIVoted (PhotonNetwork.playerName, cardNumber);
	}

}
