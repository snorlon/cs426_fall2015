using UnityEngine;
using System.Collections;

public class MeleeProjectile : Projectile
{

    public bool on = false;
    protected float onTime;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (on)
        {
            onTime -= Time.deltaTime;
            if (onTime <= 0)
            {
                on = false;
            }
        }
    }

    protected override void LateUpdate ()
    {
        if (on)
            base.LateUpdate();
    }

    public virtual void Activate (float time)
    {
        on = true;
        onTime = time;
    }

    public virtual void Deactivate ()
    {
        on = false;
    }
}
