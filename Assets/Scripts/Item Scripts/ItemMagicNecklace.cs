using UnityEngine;
using System.Collections;

public class ItemMagicNecklace : Item {
    
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
        player.maxMagic += 2;
        player.currentMagic += 2;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.maxMagic -= 2;
        if (player.currentMagic > player.maxMagic)
            player.currentMagic = player.maxMagic;
    }
}
