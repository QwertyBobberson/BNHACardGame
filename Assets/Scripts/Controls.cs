using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    /*
     * Allows the human player to interact with the game
     */

    //The two objects currently selected by the human player
    GameObject selectOne, selectTwo;

    private void Start()
    {
        
    }

    private void Update()
    {

        //If the mouse is clicked
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Find what the mouse hit
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //If it hit something
            if (Physics.Raycast(mouse, out hit, 2000))
            {
                //If the player has nothing selected
                if(selectOne == null)
                {
                    //The object is not selected in slot 2
                    selectOne = hit.transform.gameObject;
                    selectOne.transform.localScale *= 1.2f;
                }
                //If the player has one item selected
                else
                {
                    //The object is now selected in slot 2
                    selectTwo = hit.transform.gameObject;
                    selectTwo.transform.localScale *= 1.2f;
                }
            }
        }

        //If the player has an item selected
        if(selectOne != null)
        {
            //If the player reclicks a selected object
            if (selectOne == selectTwo)
            {
                //deselect the object
                selectOne.transform.localScale /= 1.2f;
                selectTwo.transform.localScale /= 1.2f;
            
            
                selectOne = selectTwo = null;
            }
            //If the first selected object is a card
            else if (selectOne.GetComponent<Card>() != null)
            {
                //If the card does not belong to the player
                if (selectOne.GetComponent<Card>().team != Enums.Teams.Human)
                {
                    //deselect the card
                    selectOne.transform.localScale /= 1.2f;

                    selectOne = null;
                }

            }
            //If the first seelected object is a cardslot
            else if (selectOne.GetComponent<CardSlot>() != null)
            {
                //deselect the cardslot
                selectOne.transform.localScale /= 1.2f;

                selectOne = null;
            }
        }
        
        //if two items are selected
        if (selectOne != null && selectTwo != null)
        {
            //Retrieve the card scripts of both items
            Card selectOneCard = selectOne.GetComponent<Card>();
            Card selectTwoCard = selectTwo.GetComponent<Card>();

            //If both items are cards
            if (selectOneCard != null && selectTwoCard != null)
            {
                //If both cards are in playe
                if (selectOneCard.isInPlay && selectTwoCard.isInPlay)
                {
                    //If both cards are on different teams
                    if(selectOneCard.team != selectTwoCard.team)
                    {
                        //Card 1 attacks card 2
                        selectOne.GetComponent<Card>().Attack(selectTwo.GetComponent<Card>());
                    }
                }
                //if both cards are on the same team and cardOne can support
                if(selectOneCard.team == selectTwoCard.team && selectOneCard.canSupport)
                {
                    //Check if the player has enough energy to play this card
                    if (GameMaster.GM.players[(int)selectOneCard.team].energy >= selectOneCard.energyCost)
                    {
                        selectOneCard.Support(selectTwoCard);
                        GameMaster.GM.players[(int)selectOneCard.team].energy -= selectOneCard.energyCost;
                    }
                }
            }
            //If one item is a card and the other is a cardslot, and the card is playable
            else if (selectOneCard != null && selectTwo.GetComponent<CardSlot>() != null && selectOneCard.playable)
            {
                //if the card is not in play
                if(!selectOneCard.isInPlay)
                {
                    //Place the card into the cardslot
                    selectTwo.GetComponent<CardSlot>().Insert(selectOneCard);
                }
            }

            //Deselect both cards
            selectOne.transform.localScale /= 1.2f;
            selectTwo.transform.localScale /= 1.2f;

            selectOne = selectTwo = null;
        }
    }
}
