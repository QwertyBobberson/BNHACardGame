using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryGirl : Card
{
    public override void Attack(Card target)
    {
    }

    public override void TakeDamage(Card opponent)
    {
    }

    public override void Support(Card teammate)
    {
        teammate.Health = teammate.baseHealth;
        teammate.Damage = (int)(teammate.Damage * .75);
        teammate.updateStats();

        Die();
    }
}
