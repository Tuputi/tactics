using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {

    public static Character currentlytakingTurn = null;
    public List<Character> characters;
    public static TurnState turnState = TurnState.Start;
    public float energyTreshhold = 5f;

    public static bool HasMoved = false;
    public static bool HasAttacked = false;

    private bool skippedTurn = false;

    public static Tile targetTile;

    //GUI



    void Start()
    {
        if (!(characters.Count > 0))
        {
            GameObject charaContainer = GameObject.Find("Characters").gameObject;
            for (int i = 0; i < charaContainer.transform.childCount; i++)
            {
                characters.Add(charaContainer.transform.GetChild(i).GetComponent<Character>());
            }
        }
    }


    public Character NextInTurn()
    {
        CleanDead();
        Character next = characters[0];
        if (currentlytakingTurn != null)
        {
            int index = characters.IndexOf(currentlytakingTurn) +1;
            if (index > characters.Count - 1)
            {
                index = 0;
            }

            next = characters[index];
        }
        while (next.characterEnergy < energyTreshhold)
        {
            foreach (Character c in characters)
            {
                if (c.characterEnergy > next.characterEnergy)
                {
                    if (c == currentlytakingTurn && !skippedTurn)
                    {
                        if (c.isAlive)
                        {
                            next = c;
                        }
                    }
                }
                c.EnergyCalculation();
            }
        }
        return next;
    }

    public void CleanDead()
    {
        List<Character> deadPeople = new List<Character>();
        foreach (Character chara in characters)
        {
            if (!chara.isAlive)
            {
                deadPeople.Add(chara);
            }
        }
        if (deadPeople.Count > 0)
        {
            for (int i = 0; i < deadPeople.Count; i++)
            {
                characters.Remove(deadPeople[i]);
                Destroy(deadPeople[i].gameObject);
            }
        }
    }


    public void TurnButton()
    {
        //should there be a next turn?
        if (GameManager.instance.VictoryConditionReached(characters))
        {
            Debug.Log("Game over");
            Application.LoadLevel(0);
        }
        else
        {
            //CleanDead();
            StartTurn();
        }      
    }

    public void StartTurn()
    {
        MapCreatorManager.instance.ClearButton();
        if (currentlytakingTurn != null)
        {
            currentlytakingTurn.ChangeMarker(false);
        }

        if (!HasAttacked && !HasAttacked && currentlytakingTurn != null)
        {
            skippedTurn = true;
            currentlytakingTurn.characterEnergy -= currentlytakingTurn.characterSpeed*0.5f;
        }

        HasAttacked = false;
        HasMoved = false;



        Character chara = NextInTurn();
        currentlytakingTurn = chara;
        InputManager.CurrentTile = chara.position;

        PrefabHolder.instance.gameCamera.transform.SetParent(chara.transform);
        PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, PrefabHolder.instance.gameCamera.transform.localPosition.y, 0);
        GameUI.UpdateStatUI(chara);
        SelectionManager.selectedCharacter = currentlytakingTurn;
        currentlytakingTurn.ChangeMarker(true);
        //Debug.Log("Next is: " + chara.characterName);
        if (currentlytakingTurn.isAi)
        {
            currentlytakingTurn.TakeTurn();
            //TurnButton();
        }
    }

   

    public static void ClearAttack()
    {
        HasAttacked = false;
        turnState = TurnState.Undecided;
    }

    public static void ClearMove()
    {
        HasMoved = false;
        turnState = TurnState.Undecided;
    }


}
