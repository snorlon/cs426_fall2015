using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager o { get; private set; }

    public List<PlayerController> players;
    public PlayerData[] playerData;
    public List<Item> items;
    public Stage stage;
    public int numPlayers = 0;
    public bool pause = false;
    private float endTimer = 0f;
    public bool end = false;
	
    public bool fillAI = false;
	
    public AiTrainerManager aiBroodmother = new AiTrainerManager();//do not interfere!

    void Awake ()
    {
        if (GameManager.o != null)
            Destroy(gameObject);
        o = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        playerData = new PlayerData[4];
        for (int i = 0; i < 4; i++)
            playerData[i] = new PlayerData();
        players = new List<PlayerController>();
        items = new List<Item>();
		
        aiBroodmother.gameManager = this;
        aiBroodmother.Start();
    }
	
    public void OnDestroy ()
    {
        //destroy our aiBroodmother
        aiBroodmother.OnDestroy();
    }
	
    void Update ()
    {
        if (end)
        {
            endTimer -= Time.deltaTime;
            if (endTimer <= 0)
            {
                end = false;
                ChangeScene(0);
            }
        }
        else if (stage != null)
        {
            int i = 0;
            foreach (PlayerController p in players)
            {
                if (p.currentHealth > 0)
                    i += 1;
            }
            if (i <= 1)
            {
                end = true;
                endTimer = 5;
				
				GameObject temp_display = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Interface/AllFightersDefeated"));
            }
        }
		
        //tick the ai broodmother
        aiBroodmother.Update();
    }

    public int AddPlayer (string control, int team)
    {
        int i;
        for (i=0; i<4 && playerData[i].active == true; i++)
            ;
        if (i < 4)
            playerData[i].Activate(control, team);
        numPlayers++;
        return i;
    }

    public void RemovePlayer (int num)
    {

        playerData[num].Deactivate();
        numPlayers--;
    }

    public int FindPlayer (string control)
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].active && playerData[i].control == control)
                return i;
        }
        return -1;
    }

    public void ChangeScene (int scene)
    {
        stage = null;
        players.Clear();
        items.Clear();
        if (scene == 1)
            Application.LoadLevel("Hud" + Mathf.Floor(2).ToString());
        else if (scene == 0)
            Application.LoadLevel("Menu");
    }

    public void LoadStage ()
    {
        stage = FindObjectOfType<Stage>();
        PlayerBattleInterfaceScript[] hud = FindObjectsOfType<PlayerBattleInterfaceScript>();
		
        if (fillAI)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerData == null)
                    continue;
                if (playerData[i] == null)
                    continue;
                if (!playerData[i].active)
                {
                    //if inactive, make them active and set their input to AI
                    AddPlayer("AI", i + 6);
                }
            }
        }
		
        for (int i = 0; i < 4; i++)
        {
            /*if (playerData == null)
                continue;
            if (playerData[i] == null)
                continue;*/
            if (!playerData[i].active)
                continue;
            Debug.Log(playerData[i].character.ToString());
            string c;
			
            if (playerData[i].character == 1)
                c = "Hero";
            else if (playerData[i].character == 2)
                c = "Princess";
            else if (playerData[i].character == 3)
                c = "Soldier";
            else
                continue;
			
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Characters/Player " + c);
            PlayerController p = Instantiate(prefab).GetComponent<PlayerController>();
            players.Add(p);
			
			//given overhead displays their number
			players[i].GetComponent<PlayerOverhead>().setSlot(i+1);
			
            //set the appropriate links for the broodmother
            if (playerData[i].control.Equals("AI"))
            {				
                //set the appropriate links for the broodmother
                aiBroodmother.linkAI(players[i].GetComponent<AiBase>(), i + 1);
            }
            else
            {
                aiBroodmother.linkAI(null, i + 1);//this will reset the ai if not in use
            }

            p.transform.position = stage.playerSpawns[i].transform.position;
            p.team = playerData[i].team;
            p.inputType = playerData[i].control;
            foreach (PlayerBattleInterfaceScript h in hud)
            {
                if (h.name == "Player " + (i + 1).ToString() + " Battle UI")
                {
                    h.connectedPlayer = p;
                    break;
                }
            }
        }

        Item[] itemarr = Resources.LoadAll<Item>("Prefabs/Items");
        for (int i = 0; i < stage.chestSpawns.Length; i++)
        {
            Item itemtemp = Instantiate(itemarr[(int)Mathf.Floor(Random.Range(0, itemarr.Length))]).GetComponent<Item>();
            items.Add(itemtemp);
            itemtemp.transform.position = stage.chestSpawns[i].transform.position;
            Chest chest = Instantiate(Resources.Load<Chest>("Prefabs/Chest")).GetComponent<Chest>();
            chest.transform.position = stage.chestSpawns[i].transform.position;
            chest.item = itemtemp;
            itemtemp.inChest = true;
        }
		
        //load AI, does not work if none are selected
        aiBroodmother.loadAI();
    }
}

public class PlayerData
{
    public bool active = false;
    public string control;
    public int team;
    public int character;

    public PlayerData ()
    {

    }

    public void Activate (string c, int t)
    {
        active = true;
        control = c;
        team = t;
        character = 1;
    }

    public void Deactivate ()
    {
        active = false;
    }
}