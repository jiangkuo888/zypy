using UnityEngine;
using System.Collections;

public class Card {
	/* every card will have a unique id for referencing */
	private int card_id;
	/* every card will have a texture source, so that when the card has been used, it can load the texture */
	private string card_src_pic;
	
	public int ID {
		get {
			return card_id;
		}

		set {
			if(value > 0){
				card_id = value;
			} else {
				/* card id should be positive, any invalid value will set to -1 */
				card_id = -1;
			}
		}
	}

	public string url {
		get {
			return url;
		}

		set {
			url = value;
		}
	}

	public Card(int id, string src){
		ID = id;
		url = src;
	}
}