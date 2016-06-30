using UnityEngine;
using System.Collections;

public class PlayerExternalAI : MonoBehaviour
{
	public PlayerController connectedPlayer = null;
	
	//this is used to denote what stage of the mode is active, such as character select or arena select
	//depending on this, the AI will act appropriately
	public string externalStage = "Character Select";



	// Use this for initialization
	void Start ()
	{
		if(connectedPlayer == null)
		{
			
		}
		else
		{
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(connectedPlayer != null)
		{
			
		}
	}
}
