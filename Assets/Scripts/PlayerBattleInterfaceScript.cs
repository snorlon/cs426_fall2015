using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBattleInterfaceScript : MonoBehaviour
{
    public PlayerController connectedPlayer = null;
	
    public Sprite[] healthSpriteArray;
    public Sprite[] manaSpriteArray;

    protected Component[] images;
	
    CanvasGroup ourCanvas = null;

    Vector3 v;

    // Use this for initialization
    void Start ()
    {
        ourCanvas = this.GetComponent<CanvasGroup>();
        
        images = GetComponentsInChildren<Image>();

        if (connectedPlayer == null)
        {
            //hide ourself
            ourCanvas.alpha = 0;
			
            //abort if we don't have player data
            return;
        }
        else
        {
            //healthSpriteArray = Resources.LoadAll<Sprite> ("health");
            //manaSpriteArray = Resources.LoadAll<Sprite> ("mana");
			
            //Component[] texts = GetComponentsInChildren<Text>();
            //nameField = Text.Find("NameField");

            //foreach (Text e in texts)
            {
                //if (e.name == "HealthField")
                //e.text = ""+connectedPlayer.currentHealth;
            }
        }
		
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (connectedPlayer != null)
        {
            ourCanvas.alpha = 1;
			
            //nameField = Text.Find("NameField");

            foreach (Image e in images)
            {
                if (e.name == "HealthHeart1")
                {
                    if (connectedPlayer.maxHealth < 2)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 2)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 1)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "HealthHeart2")
                {
                    if (connectedPlayer.maxHealth < 4)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 4)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 3)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "HealthHeart3")
                {
                    if (connectedPlayer.maxHealth < 6)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 6)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 5)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "HealthHeart4")
                {
                    if (connectedPlayer.maxHealth < 8)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 8)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 7)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "HealthHeart5")
                {
                    if (connectedPlayer.maxHealth < 10)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 10)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 9)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "HealthHeart6")
                {
                    if (connectedPlayer.maxHealth < 12)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentHealth >= 12)
                        e.sprite = healthSpriteArray[0];
                    else if ((double)connectedPlayer.currentHealth == 11)
                        e.sprite = healthSpriteArray[1];
                    else
                        e.sprite = healthSpriteArray[2];
                }
                else if (e.name == "ManaBottle1")
                {
                    if (connectedPlayer.maxMagic < 2)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 2)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 1)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "ManaBottle2")
                {
                    if (connectedPlayer.maxMagic < 4)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 4)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 3)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "ManaBottle3")
                {
                    if (connectedPlayer.maxMagic < 6)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 6)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 5)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "ManaBottle4")
                {
                    if (connectedPlayer.maxMagic < 8)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 8)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 7)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "ManaBottle5")
                {
                    if (connectedPlayer.maxMagic < 10)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 10)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 9)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "ManaBottle6")
                {
                    if (connectedPlayer.maxMagic < 12)
                        e.GetComponent<Image>().color = Color.clear;
                    else
                        e.GetComponent<Image>().color = Color.white;
                    //check if health is above a threshold for current health
                    if ((double)connectedPlayer.currentMagic >= 12)
                        e.sprite = manaSpriteArray[0];
                    else if ((double)connectedPlayer.currentMagic == 11)
                        e.sprite = manaSpriteArray[1];
                    else
                        e.sprite = manaSpriteArray[2];
                }
                else if (e.name == "Basic Cooldown")
                {
                    v = e.rectTransform.localScale;
                    if (connectedPlayer.basicCooldownCurrent <= 0)
                        v.y = 0;
                    else
                        v.y = 1.2f*connectedPlayer.basicCooldownCurrent/connectedPlayer.basicCooldown;
                    e.rectTransform.localScale = v;
                    v = e.rectTransform.localPosition;
                    v.y=-60+60*connectedPlayer.basicCooldownCurrent/connectedPlayer.basicCooldown;
                    e.rectTransform.localPosition = v;
                }
                else if (e.name == "Special Cooldown")
                {
                    v = e.rectTransform.localScale;
                    if (connectedPlayer.specialCooldownCurrent <= 0)
                        v.y = 0;
                    else
                        v.y = 1.2f*connectedPlayer.specialCooldownCurrent/connectedPlayer.specialCooldown;
                    e.rectTransform.localScale = v;
                    v = e.rectTransform.localPosition;
                    v.y=-60+60*connectedPlayer.specialCooldownCurrent/connectedPlayer.specialCooldown;
                    e.rectTransform.localPosition = v;
                }
                else if (e.name == "Active 1 Cooldown")
                {
                    v = e.rectTransform.localScale;
                    if (connectedPlayer.active1CooldownCurrent <= 0)
                        v.y = 0;
                    else
                        v.y = 1.2f*connectedPlayer.active1CooldownCurrent/connectedPlayer.active1.cooldown;
                    e.rectTransform.localScale = v;
                    if(connectedPlayer.active1 == null) continue;
                    v = e.rectTransform.localPosition;
                    v.y=-60+60*connectedPlayer.active1CooldownCurrent/connectedPlayer.active1.cooldown;
                    e.rectTransform.localPosition = v;
                }
                else if (e.name == "Active 2 Cooldown")
                {
                    v = e.rectTransform.localScale;
                    if (connectedPlayer.active2CooldownCurrent <= 0)
                        v.y = 0;
                    else
                        v.y = 1.2f*connectedPlayer.active2CooldownCurrent/connectedPlayer.active2.cooldown;
                    e.rectTransform.localScale = v;
                    if(connectedPlayer.active2 == null) continue;
                    v = e.rectTransform.localPosition;
                    v.y=-60+60*connectedPlayer.active2CooldownCurrent/connectedPlayer.active2.cooldown;
                    e.rectTransform.localPosition = v;
                }
                else if (e.name == "Active 1")
                {
                    if (connectedPlayer.active1 != null)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.active1.GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Active 2")
                {
                    if (connectedPlayer.active2 != null)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.active2.GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Passive 1")
                {
                    if (connectedPlayer.passives.Count >= 1)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.passives[0].GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Passive 2")
                {
                    if (connectedPlayer.passives.Count >= 2)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.passives[1].GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Passive 3")
                {
                    if (connectedPlayer.passives.Count >= 3)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.passives[2].GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Passive 4")
                {
                    if (connectedPlayer.passives.Count >= 4)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.passives[3].GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
                else if (e.name == "Passive 5")
                {
                    if (connectedPlayer.passives.Count >= 5)
                    {
                        e.GetComponent<Image>().color = Color.white;
                        e.sprite = connectedPlayer.passives[4].GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        e.GetComponent<Image>().color = Color.clear;
                    }
                }
            }

        }
        else
        {
            ourCanvas.alpha = 0;
        }
    }
}
