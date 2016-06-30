using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    protected BoxCollider2D box;
    public int team;
    public int direction = 1;
    public float life = 0;
    protected PlayerController i;
    public int damage = 1;
    
    // Use this for initialization
    protected virtual void Start ()
    {
        box = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    protected virtual void Update ()
    {
        if (GameManager.o.pause)
            return;
        life += Time.deltaTime;
    }
    
    protected virtual void LateUpdate ()
    {
        foreach (PlayerController i in GameManager.o.players)
        {
            if (box.bounds.Intersects(i.box.bounds))
            {
                if (i.team != team || team == 0)
                if (i.Hit(this))
                    break;
            }
        }
    }
    
    public virtual void Effect (PlayerController player)
    {
        player.Damage(damage, direction, getSource());
    }
	
    public PlayerController getSource ()
    {
        return i;
    }

}
