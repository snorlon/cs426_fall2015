using UnityEngine;
using System.Collections;

public class CustomAnimator : MonoBehaviour
{

    public Animator animator;
    protected CustomPhysics physics;
    protected PlayerController player;

    public bool attacking = false;
    public bool special = false;
    public bool item = false;
    public bool hurt = false;
    public bool specialFire = false;

    protected Vector3 t;

    // Use this for initialization
    public virtual void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        physics = GetComponent<CustomPhysics>();
        player = GetComponent<PlayerController>();
    }
	
    // Update is called once per frame
    public virtual void LateUpdate ()
    {
        t = animator.transform.localScale;
        t.x *= player.direction * Mathf.Sign(animator.transform.localScale.x);
        animator.transform.localScale = t;
        if (GameManager.o.pause)
            animator.speed = 0;
        else
            animator.speed = 1;
    }
}
