using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(BoxCollider2D))]
public class Chest : MonoBehaviour
{
    
    public Animator animator;
    protected CustomPhysics physics;
    protected BoxCollider2D box;
    public bool open = false;
    public Item item;
    protected float openTimer = 1.5f;

    void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        physics = GetComponent<CustomPhysics>();
        box = GetComponent<BoxCollider2D>();
    }
	
    void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (open)
        {
            if (openTimer > 0)
            {
                animator.Play("Opening");
                openTimer -= Time.deltaTime;
                if (openTimer <= 0)
                {
                    item.inChest = false;
                    item.transform.position = transform.position;
                }
            }
            else
                animator.Play("Open");
        }
        else
            animator.Play("Closed");
        physics.Move(Vector2.zero);
    }

    void LateUpdate ()
    {
        if (!open)
        {
            foreach (PlayerController i in GameManager.o.players)
            {
                if (box.bounds.Intersects(i.box.bounds))
                {
                    open = true;
                    break;
                }
            }
        }
    }
}
