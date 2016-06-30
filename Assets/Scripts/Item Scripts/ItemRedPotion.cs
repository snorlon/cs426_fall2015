using UnityEngine;
using System.Collections;

public class ItemRedPotion : ActiveItem
{
    
    protected override void Start ()
    {
        base.Start();
        cooldown = 20;
        cost = 0;
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
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
    }
    
    public override void Activate (PlayerController player)
    {
        player.currentHealth += 3;
        if (player.currentHealth > player.maxHealth)
            player.currentHealth = player.maxHealth;
    }
}
