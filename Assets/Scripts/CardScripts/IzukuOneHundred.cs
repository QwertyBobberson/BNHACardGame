using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IzukuOneHundred : Card
{
    //Can attack and take damage, cannot support
    //Chance to stun enemies on attack
    //Chance to take half damage when attacked

    public float blackWhipChance;
    public float dangerSenseChance;

    public override void Attack(Card target)
    {
        if (QuirkStunLength == 0 && stunLength == 0)
        {
            target.stunLength += Random.value > blackWhipChance ? 0 : 1;
            target.TakeDamage(this);
        }
        else if (stunLength == 0)
        {
            Damage = quirklessDamage;
            target.TakeDamage(this);
        }
    }

    public override void TakeDamage(Card opponent)
    {
        Health -= Random.value > (dangerSenseChance) ? opponent.Damage : opponent.Damage / 2;
    }
}
