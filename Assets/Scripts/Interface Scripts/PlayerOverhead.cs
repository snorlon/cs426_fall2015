using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerOverhead : MonoBehaviour
{
    public PlayerController connectedPlayer = null;
	
    public Sprite[] overheadLabels = new Sprite[4];
    public Sprite[] overheadArrows = new Sprite[4];
	
	int slot = 1;//for our current character positon (P1, P2, P3, P4)
	
	GameObject overhead_display = null;

    // Use this for initialization
    void Start ()
    {		
		overhead_display = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Interface/PlayerOverheadDisplay"));
		
		//load in the images
		overheadLabels[0] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p1");
		overheadLabels[1] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p2");
		overheadLabels[2] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p3");
		overheadLabels[3] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p4");
		overheadArrows[0] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_1");
		overheadArrows[1] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_2");
		overheadArrows[2] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_3");
		overheadArrows[3] = Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_4");
		
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (connectedPlayer != null && overhead_display != null)
        {
			//update our position
			overhead_display.transform.SetParent(connectedPlayer.transform);
			overhead_display.transform.position = new Vector3(connectedPlayer.transform.position.x+0.48f,connectedPlayer.transform.position.y+0.7f,connectedPlayer.transform.position.z);
			
			//atempt to update the sprite
			(overhead_display.transform.Find("InterfaceOverheadLabel").GetComponent<Image>()).sprite = overheadLabels[slot-1];
			(overhead_display.transform.Find("InterfaceArrow").GetComponent<Image>()).sprite = overheadArrows[slot-1];
        }
    }
	
	public void setSlot(int newSlot)
	{
		slot = newSlot;
	}
}
