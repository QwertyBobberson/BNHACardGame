using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eri : Card
{
    //Cannot attack or take damage
    //Support restores character to initial stats
    //Ends all stun and quirk stuns
    //Transforms standard Izuku into 100% form

    public override void Attack(Card target)
    {
    }

    public override void TakeDamage(Card opponent)
    {
    }

    public override void Support(Card teammate)
    {
        teammate.Health = teammate.baseHealth;
        teammate.Damage = teammate.baseDamage;
        teammate.updateStats();


        if(teammate.GetComponent<Card>().title == "Izuku Midoriya")
        {
            teammate.replaceWith(GameMaster.GM.getCardByName("Izuku Midoriya: 100% Full Cowl"));
        }

        Die();
    }
}
