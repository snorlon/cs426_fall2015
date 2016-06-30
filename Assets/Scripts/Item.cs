using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    protected CustomPhysics physics;
    protected BoxCollider2D box;
    public bool held = false;
    protected PlayerController i;
    public bool inChest = false;
    
    //unique number to distinguish which item is in use, set by the prefabs
    //0 is none, and if on an existing item should be treated as an error
    //1-50 is passive items, 51-100 is active items
    //1 = ; 2 = ; 3 = ; 4 = ;
    //51 = red potion; 52 = blue potion; 53 = green potion; 
    public int id = 0;

    // Use this for initialization
    protected virtual void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        box = GetComponent<BoxCollider2D>();
    }
	
    // Update is called once per frame
    protected virtual void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (!held && !inChest)
            physics.Move(Vector2.zero);
    }

    protected virtual void LateUpdate ()
    {
        if (!held && !inChest)
        {
            GetComponent<Renderer>().enabled = true;
            foreach (PlayerController i in GameManager.o.players)
            {
                if (box.bounds.Intersects(i.box.bounds))
                {
                    if (i.Pickup(this))
                        break;
                }
            }
        }
        else
            GetComponent<Renderer>().enabled = false;
    }

    public virtual void OnPickup (PlayerController player)
    {
        held = true;
        //GetComponent<SpriteRenderer>().enabled = false;
    }

    public virtual void OnDrop (PlayerController player)
    {
        held = false;
        //GetComponent<Renderer>().enabled = true;
        transform.position = player.transform.position;
    }

    public virtual void Tick (PlayerController player)
    {
        //Debug.Log("hi there, I'm an item.");
    }
}
