using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    /*
     * Subclass of the Human class that allows the computer to control a player
     * TODO: Completely rewrite this
     */

    [SerializeField] CardSlot[] opponentCardSlots;

    public void Play()
    {
        //If it is the computer's turn
        if(canAct)
        {
            //Fill each empty card slot on the computer's side of the field with a card from the computer's hand
            for(int i = 0; i < fieldSlots.Length; i++)
            {
                if(!fieldSlots[i].filled && cardsInHand > 0)
                {
                    fieldSlots[i].Insert(handSlots[i].card);
                    countCards();
                }
            }

            //Attack a random card on the opposing player's side of the field with each card on the computer's side of the field

           
            for(int i = 0; i < fieldSlots.Length; i++)
            {
                if(!fieldSlots[i].filled)
                {
                    continue;
                }

                for(int j = 0; j < fieldSlots[i].card.maxAttacks; j++)
                {
                    int enemyCards = 0;

                    for (int k = 0; k < opponentCardSlots.Length; k++)
                    {
                        if (opponentCardSlots[k].filled)
                        {
                            enemyCards++;
                        }
                    }

                    if (enemyCards == 0)
                    {
                        GameMaster.GM.EndTurn();
                        return;
                    }

                    if (fieldSlots[i].filled && enemyCards > 0)
                    {
                        int opponentSlot = 0;
                        do
                        {
                            opponentSlot = Random.Range(0, opponentCardSlots.Length);
                        } while (!opponentCardSlots[opponentSlot].filled);

                        if (opponentCardSlots[opponentSlot].filled)
                        {
                            fieldSlots[i].card.Attack(opponentCardSlots[opponentSlot].card);
                        }

                    }
                }
            }

            GameMaster.GM.EndTurn();
        }
    }
    
    public override void GetCard()
    {
       //Adds a random card to the player's hand
        cardsInHand++;
        int cardNum;
        do
        {
            cardNum = Random.Range(0, GameMaster.GM.spawnableCards.Length);
        }
        while (!GameMaster.GM.spawnableCards[cardNum].GetComponent<Card>().playable);
        Summon(GameMaster.GM.spawnableCards[cardNum]);
    }
}

