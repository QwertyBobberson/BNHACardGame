using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    /*
    * Holds cards and determines some of their traits
    */

    //Whether or not this cardslot currently contains a card
    public bool filled;
    //Displacement between the cardslot and the card it is attatched too
    [SerializeField] Vector3 offset;
    //The orientation of the card in the cardslot
    [SerializeField] Vector3 rotation;
    //Whether this cardslot is on the field or in a player's team
    [SerializeField] bool isInHand;
    //The team this card belongs to (computer or human)
    public Enums.Teams team;
    //The card attatched to this cardslot
    public Card card;

    public void Insert(Card card)
    {
        //if the slot is empty and the card and cardslot belong to the same player

        if (!filled && team == card.team)
        {
            if (!isInHand)
            {
                //Check if the player has enough energy to play a card
                if (GameMaster.GM.players[(int)team].energy >= card.energyCost)
                {
                    //Tell the card it is on the field
                    card.isInPlay = true;
                    card.OnPlay();
                    GameMaster.GM.players[(int)team].energy -= card.energyCost;
                }
                else
                {
                    return;
                }
            }

            //if the card is already in a card slot
            if (card.cardSlot != null)
            {
                //Tell that cardslot it is no longer filled
                card.cardSlot.filled = false;
                card.cardSlot.card = null;
            }
            //Update the card currently in the cardslot
            this.card = card;
            //Update whether or not this cardslot has a card
            filled = true;
            //Rotate the card to the correct orientation
            card.transform.eulerAngles = rotation;
            //Move the card to the cardslot
            //TODO: add lerp
            card.transform.position = transform.position + offset;
            //If this cardslot is not in a players hand
            
            //Tell the card it is now in this cardslot
            card.cardSlot = this;
        }
        
    }
}