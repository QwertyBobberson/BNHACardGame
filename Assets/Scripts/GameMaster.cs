using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    /*
     * Keeps track of gamestates, turns, and cards for the entire game
     */


    //A lost of all cards
    public GameObject[] spawnableCards;
    public GameObject[] allCards;
    //A list of all players
    public Player[] players;
    //Max cards each player can have in their hand
    [SerializeField] int maxCardsInHand;
    //Reference to the button that ends the human player's turn
    [SerializeField] Button endTurnButton;
    //Refrence to the gamemaster that can be used by all other scripts and static functions
    public static GameMaster GM;

    //Field stats
    int temperature;
    int fieldVolume;
    int lightLevel;

    public int maxEnergy;

    private void Start()
    {
        //If the reference to the gamemaster is already set
        if(GM != null)
        {
            //Send an error report
            Debug.LogError("There is more than one Game Master");
        }
        //If there is no reference to a gamemaster script
        else
        {
            //Set the refrence to this gamemaster
            GM = this;
        }

        FillDecks();
    }

    void FillDecks()
    {
        //If any players have less than the max amount of cards, give them more
        for(int i = 0; i < players.Length; i++)
        {
            while(players[i].cardsInHand < maxCardsInHand)
            {
                players[i].GetCard();
            }
        }
    }

    public void EndTurn()
    {
        //Switch who's turn it is
        for(int i = 0; i < GM.players.Length; i++)
        {
            //Reset player energies, count their cards, tell them who can act, and give them cards
            GM.players[i].canAct = !GM.players[i].canAct;
            GM.players[i].countCards();
            GM.players[i].energy = maxEnergy;

            FillDecks();

            //Reset card attack counts and reduce stun lengths
            for(int j = 0; j < GM.players[i].fieldSlots.Length; j++)
            {
                if(GM.players[i].fieldSlots[j].card != null)
                {
                    GM.players[i].fieldSlots[j].card.hasAttacked = 0;
                    GM.players[i].fieldSlots[j].card.stunLength -= GM.players[i].fieldSlots[j].card.stunLength == 0 ? 0 : 1;
                    GM.players[i].fieldSlots[j].card.QuirkStunLength -= GM.players[i].fieldSlots[j].card.QuirkStunLength == 0 ? 0 : 1;
                }
            }

            //Enable or disable the end turn button, increase max energy, and tell the computer to play
            if (GM.players[i].team == Enums.Teams.Human)
            {
                GM.endTurnButton.enabled = GetComponent<Controls>().enabled = GM.players[i].canAct;
                maxEnergy = GM.endTurnButton.enabled ? maxEnergy : maxEnergy + 1;
            }
            else
            {
                GM.players[i].GetComponent<AIPlayer>().Play();
            }
        }
    }

    public GameObject getCardByName(string name)
    {
        for(int i = 0; i < spawnableCards.Length; i++)
        {
            if(allCards[i].GetComponent<Card>().title == name)
            {
                return allCards[i];
            }
        }

        Debug.LogError($"No card with the name {name} exists.");

        return null;
    }
}
