using UnityEngine;
using System.Collections;

public class PlayerPhysics : CustomPhysics
{
    public float ledge;

    protected override void Start ()
    {
        base.Start();
        collLayer2 = "Characters";
    }
    public override void Move (Vector2 target)
    {
        ledge = 0;
        base.Move(target);
    }
    protected override bool CollideH ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            if (hits[i2].collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                collide = true;
                if (ray.direction.x == 1)
                    collideRight = true;
                else
                    collideLeft = true;
                lockX = false;
            
                float dst = Vector2.Distance(ray.origin, hits[i2].point);
                if (dst > skin)
                {
                    speed.x = 0;
                    sp.x = dst * ray.direction.x - skin * ray.direction.x;
                }
                else
                {
                    speed.x = 0;
                    sp.x = 0;
                }
                if (i > 0)
                    ledge = hits[i2].collider.transform.position.y + hits[i2].collider.transform.localScale.y / 2;
                return true;
            }
            else if (hits[i2].collider.gameObject.layer == LayerMask.NameToLayer("Characters"))
            {
                float temp = Mathf.Min(6, Mathf.Max(3, Mathf.Abs(speed.x - hits[i2].collider.gameObject.GetComponent<PlayerPhysics>().speed.x)));
                GetComponent<PlayerController>().Bounce(hits[i2].collider.gameObject, temp * ray.direction * -1);
                hits[i2].collider.GetComponent<PlayerController>().Bounce(gameObject, temp * ray.direction);

                collide = true;
            
                float dst = Vector2.Distance(ray.origin, hits[i2].point);
                if (dst > skin)
                {
                    sp.x = dst * ray.direction.x - skin * ray.direction.x;
                }
                else
                {
                    sp.x = 0;
                }
                return true;
            }
        }
        return false;
    }
    
    protected override bool CollideV ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            if (hits[i2].collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                collide = true;
                if (ray.direction.y == 1)
                    collideTop = true;
                else
                    collideBottom = true;
                lockX = false;
                
                float dst = Vector3.Distance(ray.origin, hits[i2].point);
                if (dst > skin)
                {
                    speed.y = 0;
                    sp.y = dst * ray.direction.y - skin * ray.direction.y;
                }
                else
                {
                    speed.y = 0;
                    sp.y = 0; 
                }
                return true;
            }
            else if (hits[i2].collider.gameObject.layer == LayerMask.NameToLayer("Characters"))
            {
                float temp = Mathf.Min(10, Mathf.Max(5, Mathf.Abs(speed.y - hits[i2].collider.GetComponent<PlayerPhysics>().speed.y)));
                GetComponent<PlayerController>().Bounce(hits[i2].collider.gameObject, temp * ray.direction * -1);
                hits[i2].collider.GetComponent<PlayerController>().Bounce(gameObject, temp * ray.direction);
                
                collide = true;
                
                float dst = Vector3.Distance(ray.origin, hits[i2].point);
                if (dst > skin)
                {
                    sp.y = dst * ray.direction.y - skin * ray.direction.y;
                }
                else
                {
                    sp.y = 0; 
                }
                return true;
            }
        }
        return false;
    }
    
}
