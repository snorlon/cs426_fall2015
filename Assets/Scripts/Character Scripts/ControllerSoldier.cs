using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorSoldier))]
public class ControllerSoldier : PlayerController
{
    public MeleeProjectile projBasicLeft, projBasicRight, projSpecialLeft, projSpecialRight;
    
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        basicCooldown = 0.3f;
        specialCooldown = 2.5f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projSpecialLeft.team = team;
        projSpecialRight.team = team;
        projBasicLeft.direction = -1;
        projSpecialLeft.direction = -1;
    }
    
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        
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
        projSpecialLeft.Deactivate();
        projSpecialRight.Deactivate();
    }
    
    public override void UnlockInput ()
    {
        base.UnlockInput();
        animator.attacking = false;
        animator.specialFire = false;
    }
}
