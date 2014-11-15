using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDeckViewController : MonoBehaviour {
	
	public List<string> myCurrentDeck = new List<string>();
	public List<UITexture> CardTextures;
	
	public UIButton selectButton;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void updateMyDeck(string[] cardNumbers)
	{
		myCurrentDeck.Clear ();


		for (int i =0; i<cardNumbers.Length-1; i++) {
			myCurrentDeck.Add (cardNumbers [i]);
//			print(cardNumbers[i]+" added");
		}
		
		
		updateDeckUI ();
	}
	
	public void updateDeckUI()
	{
		string path = "card/";
		
		if (myCurrentDeck.Count != 0) {
			for(int i=0;i<myCurrentDeck.Count;i++)
			{
				CardTextures[i].mainTexture =  Resources.Load(path+myCurrentDeck[i]) as Texture;
			}
		}
		
	}


	public void SelectCard(){

		UIGrid grid = GetComponentInChildren<UIGrid> ();

		string cardNumber = grid.GetComponent<UICenterOnChild> ().mCenteredObject.GetComponent<UITexture> ().mainTexture.name;

		grid.GetComponent<UICenterOnChild> ().mCenteredObject.GetComponentInChildren<UISprite> ().enabled = true;

		selectButton.isEnabled = false;

//		print (cardNumber +" is selected");

		GameManager.SP.tellHostISelected (PhotonNetwork.playerName, cardNumber);
	}
}
