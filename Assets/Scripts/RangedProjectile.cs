using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
public class RangedProjectile : Projectile
{
    
    protected CustomPhysics physics;
    protected SpriteRenderer sprite;
    
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        physics = GetComponent<CustomPhysics>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        if (GameManager.o.pause)
            return;
        physics.Move(Vector2.zero);
    }
}
