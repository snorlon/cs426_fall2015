using UnityEngine;
using System.Collections;

public class ItemMagicArmor : Item
{
    
    protected override void Start ()
    {
        base.Start();
    }
    
    protected override void Update ()
    {
        base.Update();
    }
    
    protected override void LateUpdate ()
    {
        base.LateUpdate();
    }
    
    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.knockbackMult /= 2;
        player.invincibleDur *= 2;
        player.knockbackDur *= 0.7f;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.knockbackMult *= 2;
        player.invincibleDur /= 2;
        player.knockbackDur /= 0.7f;
    }
}
