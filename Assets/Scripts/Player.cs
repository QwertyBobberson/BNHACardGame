using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * Keeps track of each player's cards and cardslots
     */

    //Determines whether it is this players turn or not
    public bool canAct;
    //Number of cards in the players hand (not including cards on field or in deck)
    public int cardsInHand;
    //Whether the player is a human or computer
    public Enums.Teams team;

    public int energy;

    //List of all cardslots that make up the player's side of the field
    public CardSlot[] fieldSlots;
    //List of all cardslots in the player's hand
    public CardSlot[] handSlots;

    public void Start()
    {
        //Neither player starts with any cards
        cardsInHand = 0;

        for(int i = 0; i < handSlots.Length; i++)
        {
            handSlots[i].team = team;
        }

        countCards();
    }

    public void countCards()
    {
        //Counts the number of cards in the player's hand
        cardsInHand = 0;
        for(int i = 0; i < handSlots.Length; i++)
        {
            CardSlot cardSlot = handSlots[i].GetComponent<CardSlot>();
            if (cardSlot.filled)
            {
                cardsInHand++;
            }
        }
    }

    public virtual void GetCard()
    {
        //Adds a random card to the player's hand
        cardsInHand++;
        Summon(GameMaster.GM.spawnableCards[Random.Range(0, GameMaster.GM.spawnableCards.Length)]);
    }

    public virtual void Summon(GameObject card)
    {
        //Creates the new card for the player
        Card newCard = GameObject.Instantiate(card).GetComponent<Card>();
        //Sets the new card's team to the team of the player
        newCard.team = team;

        //Inserts the card into an empty card slot in the player's hand
        for (int i = 0; i < handSlots.Length; i++)
        {

            CardSlot cardSlot = handSlots[i].GetComponent<CardSlot>();
            if(!cardSlot.filled)
            {
                cardSlot.Insert(newCard);
            }
        }
    }
}
