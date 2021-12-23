using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    /*
     * Repersentation of a unit
     */


    //Name and description of the card
    //TODO: Add "Funny Names"
    public string title;
    public string description;
    //Array of text boxes to display the stats of the card
    private TextMeshProUGUI[] stats;

    //The amount of damage this card can take or deal
    public int baseHealth;
    public int baseDamage;
    public int quirklessHealth;
    public int quirklessDamage;
    private int health;
    private int damage;

    //Stun and attack limits
    private int quirkStunLength;
    public int QuirkStunLength 
    {
        get
        {
            return quirkStunLength;
        }
        
        set
        {

            if (quirkStunLength == 0 && value > 0)
            {
                disableQuirk();
            }
            if(value == 0)
            {
                enableQuirk();
            }
            quirkStunLength = value;
        }
    }
    public int stunLength;
    public int maxAttacks;
    public int hasAttacked;

    //Descriptors of attack and defense
    public Enums.AttackType attackType;
    public Enums.AttackElement attackElement;
    public Enums.BloodType bloodType;

    //The team this card is on (computer or human)
    public Enums.Teams team;

    //The cards abilities and cost
    public bool playable;
    public bool canSupport;
    public int energyCost = 1;

    //The cardslot this card is attatched to
    public CardSlot cardSlot;

    //Whether or not the card is on the field
    public bool isInPlay;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if(value >= 1)
            {
                health = value;
            }
            else
            {
                Die();
            }
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    [SerializeField] Texture image;

    public virtual void Start()
    {
        if(baseHealth == 0)
        {
            baseHealth = 1;
        }

        Health = baseHealth;
        damage = baseDamage;

        //Gets the textboxes and image slots associated with the card
        stats = transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();
        GetComponent<Renderer>().material.mainTexture = image;
        //Fills text boxes with stats
        stats[0].text = title;
        stats[1].text = health + "";
        stats[2].text = damage + "";
        stats[3].text = description;
    }

    public virtual void Attack(Card target)
    {
        //Hit an opponent, never used, always overwritten
        if(hasAttacked < maxAttacks)
        {
            target.TakeDamage(this);
            hasAttacked++;
        }
    }

    public virtual void Support(Card teammate)
    {

        //Support a teammate, never used, always overwritten
        Debug.Log($"{title} supported {teammate.title}!");
    }

    public virtual void TakeDamage(Card opponent)
    {
        //Take damage from an opponent, never used, always overwritten
        Health -= opponent.Damage;
        stats[1].text = health + "";
    }

    public void updateStats()
    {
        stats[0].text = title;
        stats[1].text = health + "";
        stats[2].text = damage + "";
        stats[3].text = description;
    }

    public void Die()
    {
        //Removes the card from its slot, allowing other cards to be placed there later
        cardSlot.filled = false;
        //Removes the card from the game
        Destroy(gameObject);
    }

    public void replaceWith(GameObject card)
    {
        //Create a card, place it in the same location as the old one, match stats, destroy this card
        Instantiate(card);

        card.transform.position = transform.position;
        card.transform.rotation = transform.rotation;
        card.transform.localScale = transform.localScale;


        Card c = card.GetComponent<Card>();

        cardSlot.Insert(c);
        cardSlot.card = c;
        c.team = team;
        c.cardSlot = cardSlot;
        c.isInPlay = isInPlay;

        Destroy(gameObject);
    }

    public virtual void OnPlay()
    {
        //Prevents cards from attacking when first placed
        hasAttacked = maxAttacks;
    }

    public void enableQuirk()
    {
        //Keeps the ratio of health to max health
        health = (health / quirklessHealth) * baseHealth;
        damage = (damage / quirklessDamage) * baseDamage;
    }

    public void disableQuirk()
    {
        //Keeps the ratio of health to max health
        health = (int)(((float)health / (float)baseHealth) * (float)quirklessHealth);
        damage = (int)(((float)damage / (float)baseDamage) * (float)quirklessDamage);
    }
}