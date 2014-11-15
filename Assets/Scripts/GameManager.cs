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

	public List<Transform> pages;
	// Use this for initialization
	void Start () {
		SP = this;
		currentStage = GameStage.waiting;
		LateUpdateStage (currentStage);
		myView = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdateStage(GameStage Stage)
	{

		currentStage = Stage;

		switch (Stage) 
		{
		case GameStage.waiting:
			NGUITools.SetActive(pages[0].gameObject,true);
			NGUITools.SetActive(pages[1].gameObject,false);
			break;
		case GameStage.distributeToAll:
			NGUITools.SetActive(pages[0].gameObject,false);
			NGUITools.SetActive(pages[1].gameObject,true);
			break;
		default:
			break;
		}
	}


	[RPC]
	public void updateStage(int nextStage)
	{

		LateUpdateStage((GameStage)nextStage);

	}

	public void StartNewRound()
	{

		myView.RPC ("updateStage", PhotonTargets.All, (int)GameStage.distributeToAll);

	}




	public void OnApplicationQuit()
	{
		PhotonNetwork.LeaveRoom ();
	}
}
