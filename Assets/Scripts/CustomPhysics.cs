using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CustomPhysics : MonoBehaviour
{
    //public LayerMask collisionMask = 8;
    protected BoxCollider2D box;
    protected Vector2 siz;
    protected Vector2 cen;
    //NEVER CHANGE BELOW LINE OR UNITY COMPILE FUNNY
    protected float skin = 0.005f;
    public Vector2 speed;
    public float gravity = 15;
    public bool gravEnabled = true;
    public float friction = 5;
    public bool lockX;
    public bool lockY;
    protected float lockXTime;
    protected float lockYTime;
    public bool collideRight, collideLeft, collideTop, collideBottom, collide;
    public string collLayer1 = "Terrain";
    public string collLayer2;
    public string collLayer3;
    [HideInInspector]
    protected Ray2D
        ray;
    [HideInInspector]
    protected RaycastHit2D[]
        hits;
    [HideInInspector]
    protected Vector2
        o, d, sp;
    [HideInInspector]
    protected int
        i, i2, originalLayer;

    protected virtual void Start ()
    {
        box = GetComponent<BoxCollider2D>();
        siz = box.size * transform.localScale.x;
        cen = box.offset * transform.localScale.x;
        Vector3 temp = transform.position;
        temp.z = 0;
        transform.position = temp;
    }

    protected virtual void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (lockX)
        {
            lockXTime -= Time.deltaTime;
            if (lockXTime <= 0)
                lockX = false;
        }
        if (lockY)
        {
            lockYTime -= Time.deltaTime;
            if (lockYTime <= 0)
                lockY = false;
        }
    }

    public virtual void Move (Vector2 target)
    {
        collide = false;
        collideRight = false;
        collideLeft = false;
        collideTop = false;
        collideBottom = false;

        if (!lockX)
        {
            if (collideBottom && ((target.x == 0 && Mathf.Abs(speed.x) > 1) || (target.x != 0 && Mathf.Sign(speed.x) != Mathf.Sign(target.x))))
                speed.x = Accelerate(speed.x, target.x - friction * Mathf.Sign(speed.x));
            else
                speed.x = Accelerate(speed.x, target.x);
        }
        if (!lockY)
        {
            if (gravEnabled)
                speed.y = Accelerate(speed.y, target.y - gravity);
            else
                speed.y = Accelerate(speed.y, target.y);
        }
        sp = speed * Time.deltaTime;

        originalLayer = gameObject.layer;
        gameObject.layer = 2;
        #region Vertical Collisions
        //check for vertical collision
        if (sp.y != 0)
        {
            for (i=0; i<3; i++)
            {
                d.Set(0, Mathf.Sign(sp.y));
                o.Set((transform.position.x + cen.x + siz.x / 2) - siz.x / 2 * i, transform.position.y + cen.y + siz.y / 2 * d.y);
                ray = new Ray2D(o, d);
                //Debug.DrawRay(ray.origin, ray.direction);
                hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Abs(sp.y) + skin, LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
                if (hits.Length != 0)
                {
                    if (CollideV())
                        break;
                }
            }
        }
        #endregion

        #region Horizontal Collisions
        //check for horizontal collision
        if (sp.x != 0)
        {
            for (i=0; i<3; i++)
            {
                d.Set(Mathf.Sign(sp.x), 0);
                o.Set(transform.position.x + cen.x + siz.x / 2 * d.x, (transform.position.y + cen.y + siz.y / 2) - siz.y / 2 * i);
                ray = new Ray2D(o, d);
                //Debug.DrawRay(ray.origin, ray.direction);
                hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Abs(sp.x) + skin, LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
                if (hits.Length != 0)
                {
                    if (CollideH())
                        break;
                }
            }
        }
        #endregion

        #region Digonal Collision
        //check for diagonal collisions
        if (!collide && sp.x != 0 && sp.y != 0)
        {
            o.Set(transform.position.x + cen.x + siz.x / 2 * Mathf.Sign(sp.x), transform.position.y + cen.y + siz.y / 2 * Mathf.Sign(sp.y));
            d.Set(sp.normalized.x, sp.normalized.y);
            ray = new Ray2D(o, d);
            //Debug.DrawRay(ray.origin, ray.direction);
            hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Sqrt(Mathf.Pow(sp.x, 2) + Mathf.Pow(sp.y, 2)), LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
            if (hits.Length != 0)
            {
                CollideD();
            }
        }
        #endregion
        gameObject.layer = originalLayer;

        transform.Translate(sp, Space.World);

    }

    protected float Accelerate (float current, float target)
    {
        float accel = Mathf.Max(10, 5 * Mathf.Abs(target - current));
        if (current == target)
        {
            return current;
        }
        else
        {
            float dir = Mathf.Sign(target - current);
            current += accel * Time.deltaTime * dir;
            
            if (dir == Mathf.Sign(target - current))
            {
                return current;
            }
            else
            {
                return target;
            }
        }
    }

    protected virtual bool CollideH ()
    {
        for (i2=0; i2<hits.Length; i2++)
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
        }
        return true;
    }

    protected virtual bool CollideV ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            collide = true;
            if (ray.direction.y == 1)
                collideTop = true;
            else
                collideBottom = true;
            lockY = false;
        
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
        }
        return true;
    }

    protected virtual bool CollideD ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            collide = true;
            collideBottom = true;
            speed.y = 0;
            sp.y = 0;
        }
        return true;
    }
    
    public void SetSpeedY (float target, float time)
    {
        speed.y = target;
        lockY = true;
        lockYTime = time;
    }

    public void SetSpeedX (float target, float time)
    {
        speed.x = target;
        lockX = true;
        lockXTime = time;
    }
}
