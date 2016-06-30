using UnityEngine;
using System.Collections;

public class AnimatorHero : CustomAnimator
{
    private string currentAnim;

    public override void LateUpdate ()
    {
        base.LateUpdate();
        if (animator != null)
        {
            if (player.defeated)
            {
                if (currentAnim != "Defeated")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_2";
                    else
                        currentAnim = "Defeated";
                }
                
            }
            else if (hurt)
            {
                if (currentAnim != "Hurt_2" && currentAnim != "Hurt_1")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_1";
                    else
                        currentAnim = "Hurt_2";
                }
            }
            else if (attacking)
            {
                if (currentAnim != "Attack_2" && currentAnim != "Attack_1")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_2";
                    else
                        currentAnim = "Attack_1";
                }
            }
            else if (specialFire)
            {
                currentAnim = "Special_Fire";
            }
            else if (special)
            {
                currentAnim = "Special";
            }
            else if (!physics.collideBottom)
            {
                if (physics.speed.y > 2)
                    currentAnim = "Jump_up";
                else if (physics.speed.y < -2)
                    currentAnim = "Jump_down";
                else
                    currentAnim = "Jump_neutral";
            }
            else if (physics.speed.x != 0)
            {
                currentAnim = "Running";
            }
            else
            {
                currentAnim = "Idle";
            }
            animator.Play(currentAnim);
        }
    }
}
