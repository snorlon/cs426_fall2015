using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorHero))]
public class ControllerHero : PlayerController
{
    public ProjectileHeroArrow projArrow;
    public MeleeProjectile projBasicLeft, projBasicRight;

    public bool specialCharge;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        maxMagic = 8;
        currentMagic = 8;
        basicCooldown = 0.5f;
        specialCooldown = 1.8f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projBasicLeft.direction = -1;
		specialChargeTimeMax = 0.7f;
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

    }

    protected void LateUpdate ()
    {
        if (specialCharge)
        {
            specialChargeTime += Time.deltaTime;
            if (specialChargeTime > specialChargeTimeMax)
                specialChargeTime = specialChargeTimeMax;
            if (Special())
            {
                LockInput(0);
            }
            else
            {
                specialCharge = false;
                animator.special = false;
                if (SpecialReleased() && currentMagic >= 1)
                {
                    specialCooldownCurrent = specialCooldown;
                    animator.specialFire = true;
                    LockInput(0.3f);
                    
                    ProjectileHeroArrow t = (ProjectileHeroArrow)Instantiate(projArrow);
                    t.transform.position = transform.position + (Vector3)box.offset;
                    t.direction = direction;
                    t.team = team;

                    if (specialChargeTime == specialChargeTimeMax && currentMagic >= 2)
                    {
                        currentMagic -= 2;
                        t.speed = t.speed * 2;
                        t.dropTime = 5;
                    }
                    else
                    {
                        currentMagic -= 1;
                        t.speed = t.speed * (1 + (0.1f * Mathf.Floor(6 * (specialChargeTime / specialChargeTimeMax))));
                        t.dropTime = t.dropTime * (1 + (0.1f * Mathf.Floor(6 * (specialChargeTime / specialChargeTimeMax))));
                    }
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
            specialChargeTime = 0;
            specialCharge = true;
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
        specialCharge = false;
    }

    public override void UnlockInput ()
    {
        base.UnlockInput();
        animator.attacking = false;
        animator.specialFire = false;
    }
}
