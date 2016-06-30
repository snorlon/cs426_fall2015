using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuCharSelect : MonoBehaviour
{
    public GameObject[] players;
    public MenuCharSelectWindow[] windows;

    protected bool fillWithBots = true;//default to true for now to test
	
	const int playableCount = 3;
	Image[] characterIcons = new Image[playableCount];
	
	Dictionary<string, int> charID = new Dictionary<string, int>();

    private int i;

    void Awake ()
    {
        if (GameManager.o == null)
        {
            GameObject g = new GameObject("Game Manager");
            g.AddComponent<GameManager>();
        }
    }
    void Start ()
    {
		//load character data
		charID.Add("Hero",0);
		charID.Add("Princess",1);
		charID.Add("Soldier",2);
		
        windows = new MenuCharSelectWindow[4];
        for (i = 0; i<4; i++)
        {
            windows[i] = new MenuCharSelectWindow(i+1);
            windows[i].Set(this, players[i].gameObject, "Player " + (i + 1).ToString());
        }
        if (GameManager.o.numPlayers > 0)
        {
            for (i = 0; i<GameManager.o.playerData.Length; i++)
            {
                if (!GameManager.o.playerData[i].active)
                    continue;
                if (GameManager.o.playerData[i].control == "Keyboard")
				{
                    windows[i].On("Keyboard", "Press X/Escape to quit");
					switchCharacter("The Destined Hero", i);
				}
                else
				{
                    windows[i].On(GameManager.o.playerData[i].control, "Press B to quit");
					switchCharacter("The Destined Hero", i);
				}
            }
        }
		
		//get all of the selectable character icons
		foreach (Image img in FindObjectsOfType(typeof(Image)) as Image[])
		{
			int id = 0;
			
			if(img.name == "Hero Panel")
			{
				charID.TryGetValue("Hero", out id);
				id++;
				
				characterIcons[id-1] = img;
			}
			if(img.name == "Princess Panel")
			{
				charID.TryGetValue("Princess", out id);
				id++;
				
				characterIcons[id-1] = img;
			}
			if(img.name == "Soldier Panel")
			{
				charID.TryGetValue("Soldier", out id);
				id++;
				
				characterIcons[id-1] = img;
			}
		}
    }
	
	void switchCharacter(string charName, int slot)
	{
		int index = slot + 1;
			
		int offset = 0;
		
		if(charName == "The Destined Hero")
		{
            windows[slot].textCharacter.text = "The Destined Hero";
			windows[slot].bigIcon.sprite = (Resources.Load<Sprite>("Graphics/Character Select/characters/standalone/hero_"+index+"_single"));
			
			//update cursor image
			if(windows[slot].characterCursor != null)
			{
				//set its image
				windows[slot].characterCursor.sprite = (Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p"+index));
			}
		}
		
		else if(charName == "Adventuring Princess")
		{
            windows[slot].textCharacter.text = "The Destined Hero";
			windows[slot].bigIcon.sprite = (Resources.Load<Sprite>("Graphics/Character Select/characters/standalone/princess_"+index+"_single"));
			
			//update cursor image
			if(windows[slot].characterCursor != null)
			{
				//set its image
				windows[slot].characterCursor.sprite = (Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p"+index));
			}
			
			offset = 1;
		}
		
		else if(charName == "Wandering Soldier")
		{
            windows[slot].textCharacter.text = "The Destined Hero";
			windows[slot].bigIcon.sprite = (Resources.Load<Sprite>("Graphics/Character Select/characters/standalone/soldier_"+index+"_single"));
			
			//update cursor image
			if(windows[slot].characterCursor != null)
			{
				//set its image
				windows[slot].characterCursor.sprite = (Resources.Load<Sprite>("Graphics/In-Game GUI/overhead_arrow_p"+index));
			}
			
			offset = 2;
		}
		
		//load the transform
		if(windows[slot].characterCursor != null)
		{			
			Vector2 p1 = new Vector2(-20,20);
			Vector2 p2 = new Vector2(20,20);
			Vector2 p3 = new Vector2(-20,-20);
			Vector2 p4 = new Vector2(20,-20);
			
			float xDiff = 68*offset;
			
			if(slot==0)
				windows[slot].characterCursor.transform.localPosition = new Vector3( p1.x + xDiff, p1.y, 0);
			else if(slot==1)
				windows[slot].characterCursor.transform.localPosition = new Vector3( p2.x + xDiff, p2.y, 0);
			else if(slot==2)
				windows[slot].characterCursor.transform.localPosition = new Vector3( p3.x + xDiff, p3.y, 0);
			else if(slot==3)
				windows[slot].characterCursor.transform.localPosition = new Vector3( p4.x + xDiff, p4.y, 0);
		}
	}
	
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("Menu Confirm"))
        {
            if (Input.GetButtonDown("Joy1 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy1") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy1", 1);
                    windows[i].On("Joy1", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("Joy2 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy2") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy2", 2);
                    windows[i].On("Joy2", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("Joy3 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy3") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy3", 3);
                    windows[i].On("Joy3", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("Joy4 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy4") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy4", 4);
                    windows[i].On("Joy4", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("KB Confirm"))
            {
                if (GameManager.o.FindPlayer("Keyboard") == -1)
                {
                    i = GameManager.o.AddPlayer("Keyboard", 5);
                    windows[i].On("Keyboard", "Press X/Escape to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("Xcade1 Confirm"))
            {
                if (GameManager.o.FindPlayer("Xcade1") == -1)
                {
                    i = GameManager.o.AddPlayer("Xcade1", 6);
                    windows[i].On("Xcade1", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
            if (Input.GetButtonDown("Xcade2 Confirm"))
            {
                if (GameManager.o.FindPlayer("Xcade2") == -1)
                {
                    i = GameManager.o.AddPlayer("Xcade2", 7);
                    windows[i].On("Xcade 2", "Press B to quit");
					switchCharacter("The Destined Hero", i);
                }
            }
        }
        else if (Input.GetButtonDown("Menu Cancel"))
        {
            if (Input.GetButtonDown("Joy1 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy1");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy2 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy2");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy3 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy3");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy4 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy4");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("KB Cancel"))
            {
                i = GameManager.o.FindPlayer("Keyboard");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Xcade1 Cancel"))
            {
                i = GameManager.o.FindPlayer("Xcade1");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Xcade2 Cancel"))
            {
                i = GameManager.o.FindPlayer("Xcade2");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }

        }
        
        if (Input.GetButtonDown("Joy1 Grab1"))
        {
            i = GameManager.o.FindPlayer("Joy1");
            if (i >= 0)
            {
                Debug.Log(i.ToString());
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("Joy2 Grab1"))
        {
            i = GameManager.o.FindPlayer("Joy2");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("Joy3 Grab1"))
        {
            i = GameManager.o.FindPlayer("Joy3");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("Joy4 Grab1"))
        {
            i = GameManager.o.FindPlayer("Joy4");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("KB Grab1"))
        {
            i = GameManager.o.FindPlayer("Keyboard");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("Xcade1 Grab1"))
        {
            i = GameManager.o.FindPlayer("Xcade1");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        if (Input.GetButtonDown("Xcade2 Grab1"))
        {
            i = GameManager.o.FindPlayer("Xcade2");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 2;
				switchCharacter("Adventuring Princess", i);
            }
        }
        
        if (Input.GetButtonDown("Joy1 Grab2"))
        {
            i = GameManager.o.FindPlayer("Joy1");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("Joy2 Grab2"))
        {
            i = GameManager.o.FindPlayer("Joy2");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("Joy3 Grab2"))
        {
            i = GameManager.o.FindPlayer("Joy3");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("Joy4 Grab2"))
        {
            i = GameManager.o.FindPlayer("Joy4");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("KB Grab2"))
        {
            i = GameManager.o.FindPlayer("Keyboard");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("Xcade1 Grab2"))
        {
            i = GameManager.o.FindPlayer("Xcade1");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }
        if (Input.GetButtonDown("Xcade2 Grab1"))
        {
            i = GameManager.o.FindPlayer("Xcade2");
            if (i >= 0)
            {
                GameManager.o.playerData[i].character = 3;
				switchCharacter("Wandering Soldier", i);
            }
        }

        if (Input.GetButtonDown("KB Pause") && GameManager.o.FindPlayer("Keyboard") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Joy1 Pause") && GameManager.o.FindPlayer("Joy1") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Joy2 Pause") && GameManager.o.FindPlayer("Joy2") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Joy3 Pause") && GameManager.o.FindPlayer("Joy3") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Joy4 Pause") && GameManager.o.FindPlayer("Joy4") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Xcade1 Pause") && GameManager.o.FindPlayer("Xcade1") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Xcade2 Pause") && GameManager.o.FindPlayer("Xcade2") >= 0 && (GameManager.o.numPlayers >= 2 || fillWithBots))
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
		//option for if there are no players whatsoever
        else if (Input.GetButtonDown("KB Pause") && fillWithBots)
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (Input.GetButtonDown("Joy1 Pause") && fillWithBots)
        {
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
        else if (GameManager.o.aiBroodmother.trainingMode)
        {
            //skip everything if the AI is in training mode, so it can loop around infinitely
            GameManager.o.ChangeScene(1);
            GameManager.o.fillAI = fillWithBots;
        }
    }
}

public class MenuCharSelectWindow
{
	int slot = 1;
    public GameObject window;
    public Text textControl;
    public Text textCharacter;
    public Text textQuit;
    public Text textJoin;
    public Image bigIcon;
	
	public Image characterCursor = null;
	
    public MenuCharSelectWindow (int newSlot)
    {
		slot = newSlot;
    }

    public void Set (MenuCharSelect charSelect, GameObject o, string player)
    {
		//grab our selection icon
		
		//get all of the selectable character icons
		foreach (Image img in MenuCharSelect.FindObjectsOfType(typeof(Image)) as Image[])
		{
			if(img.name == "Cursor_P"+slot)
			{
				characterCursor = img;
			}
		}
		
        window = o;
        Text[] ta = o.GetComponentsInChildren<Text>();
        foreach (Text t in ta)
        {
            if (t.name == "Control")
                textControl = t;
            else if (t.name == "Character")
                textCharacter = t;
            else if (t.name == "Quit")
                textQuit = t;
            else if (t.name == "Join")
                textJoin = t;
        }
        Image[] tb = o.GetComponentsInChildren<Image>();
        foreach (Image t in tb)
        {
            if (t.name == "big image")
                bigIcon = t;
        }

        Off();
    }

    public void On (string control, string quit)
    {
        textControl.text = control;
        textQuit.text = quit;
		textJoin.text = "";
    }

    public void Off ()
    {
        textControl.text = "";
        textCharacter.text = "Open Slot";
        textQuit.text = "";
		textJoin.text = "Press Confirm";
		
		if(characterCursor != null)
			characterCursor.sprite = (Resources.Load<Sprite>("Graphics/Character Select/characters/standalone/null"));
		
		if(bigIcon != null)
			bigIcon.sprite = (Resources.Load<Sprite>("Graphics/Character Select/characters/standalone/null"));
    }
}