using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMight : Card
{
    //Has a chance to take 2x damage from attacks

    public override void Attack(Card target)
    {
        target.TakeDamage(this);
    }

    public override void TakeDamage(Card opponent)
    {
        Health -= Random.value < .05 ? opponent.Damage * 2 : opponent.Damage;
    }
}
