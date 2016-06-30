using UnityEngine;
using System.Collections;

public class AnimatorSoldier : CustomAnimator
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
                        currentAnim = "Hurt_Air";
                    else
                        currentAnim = "Defeated";
                }
                
            }
            else if (hurt)
            {
                if (currentAnim != "Hurt_Air" && currentAnim != "Hurt_Ground")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_Ground";
                    else
                        currentAnim = "Hurt_Air";
                }
            }
            else if (attacking)
            {
                if (currentAnim != "Attack_Air" && currentAnim != "Attack_Ground")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_Air";
                    else
                        currentAnim = "Attack_Ground";
                }
            }
            else if (specialFire)
            {
                //currentAnim = "Special_Fire";
            }
            else if (special)
            {
                //currentAnim = "Special";
            }
            else if (!physics.collideBottom)
            {
                if (physics.speed.y > 2)
                    currentAnim = "Jump_Up";
                else if (physics.speed.y < -2)
                    currentAnim = "Jump_Down";
                else
                    currentAnim = "Jump_Neutral";
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
