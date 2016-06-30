using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorPrincess))]
public class ControllerPrincess : PlayerController
{
    public MeleeProjectile projBasicLeft, projBasicRight, projChargeLeft, projChargeRight;
    
    public bool basicCharge;
    public float basicChargeTime, basicChargeTimeMax = 0.7f;
    
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        basicCooldown = 0.3f;
        specialCooldown = 2.5f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projChargeLeft.team = team;
        projChargeRight.team = team;
        projBasicLeft.direction = -1;
        projChargeLeft.direction = -1;
    }
    
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        
    }
    
    protected void LateUpdate ()
    {
        if (basicCharge)
        {
            basicChargeTime += Time.deltaTime;
            if (basicChargeTime > basicChargeTimeMax)
                basicChargeTime = basicChargeTimeMax;
            if (!Attack())
            {
                basicCharge = false;
                if (AttackReleased() && basicChargeTime == basicChargeTimeMax && currentMagic >= 1)
                {
                    LockInput(0.3f);
                    currentMagic -= 1;
                    if (direction > 0)
                        projChargeRight.Activate(0.2f);
                    else
                        projChargeLeft.Activate(0.2f);
                }
            }
        }
    }
    
    protected override void AttackEffect ()
    {
        if (basicCooldownCurrent <= 0)
        {
            basicCooldownCurrent = basicCooldown;
            
            if (direction > 0)
                projBasicRight.Activate(0.2f);
            else
                projBasicLeft.Activate(0.2f);
            physics.speed.x += 6 * direction;
            animator.attacking = true;
            LockInput(0.2f);
        }
    }
    
    protected override void SpecialEffect ()
    {
        if (currentMagic >= 1 && specialCooldownCurrent <= 0)
        {
            basicChargeTime = 0;
            basicCharge = true;
            animator.special = true;
            LockInput(0);
        }
    }
    
    public override void Damage (int damage, int dir, PlayerController source)
    {
        base.Damage(damage, dir, source);
        animator.attacking = false;
        animator.special = false;
        projBasicLeft.Deactivate();
        projBasicRight.Deactivate();
        projChargeLeft.Deactivate();
        projChargeRight.Deactivate();
        basicCharge = false;
    }
    
    public override void UnlockInput ()
    {
        base.UnlockInput();
        animator.attacking = false;
        animator.specialFire = false;
    }
}
