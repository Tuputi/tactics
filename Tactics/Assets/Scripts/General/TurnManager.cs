using UnityEngine;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour {

    public enum TurnMode {  undecided, start, move, action, facing, end};

    public static TurnManager instance;
    public static OrderedBag<Character> characters;
    private List<Character> inactiveCharacters;
    public float EnergyThreshold = 50f;
    public float EnergyAddedPerTurn = 5f;
    public Character CurrentlyTakingTurn;


    public static TurnMode mode;
    public static GameMode gameMode;
    public bool hasMoved = false;
    public bool hasActed = false;
    

    void Awake()
    {
        instance = this;
        characters = new OrderedBag<Character>();
        if (SceneManager.GetActiveScene().name == "GameMode")
        {
            Debug.Log("GameMode");
            gameMode = GameMode.Game;
        }
        else
        {
            gameMode = GameMode.Editor;
        }

    }

	public void CreateCharacterList()
    {
        GameObject characterHolder = GameObject.Find("Characters");
        for(int i = 0; i < characterHolder.transform.childCount; i++)
        {
            characters.Add(characterHolder.transform.GetChild(i).GetComponent<Character>());
        }

        foreach( Character chara in characters)
        {
            if(chara.CharacterInventory == null)
            {
                CharacterLogic.instance.CreateInventory(chara);
            }

            CharacterLogic.instance.CreateAttackList(chara);
            TurnOrderTimeline.AddATimeCard(chara);
        }
    }

    void AddEnergy()
    {
        OrderedBag<Character> tempList = new OrderedBag<Character>(characters);
        characters.Clear();
        foreach(Character chara in tempList)
        {
                chara.characterEnergy += EnergyAddedPerTurn;
                characters.Add(chara);
        }
        List<Character> sendList = new List<Character>(tempList);
        UIManager.instance.UpdateTurnOrderDisplay(sendList);
    }


    public void FindNextInTurn()
    {

        CheckAliveStatus();
        SelectionScript.ClearSelection();
        Character nextCharacter = null;
        while(nextCharacter == null)
        {
            if(characters.GetFirst().characterEnergy >= (225- characters.GetFirst().speed))
            {
                    nextCharacter = characters.GetFirst();
            }
            else
            {
                AddEnergy();
            }
        }
        //Debug.Log("Next in turn is " + nextCharacter.characterName);
        CurrentlyTakingTurn = nextCharacter;
        UIManager.instance.AssignInTurnMaker(CurrentlyTakingTurn);
        TakeTurn();
    }

    public void TakeTurn()
    {
        mode = TurnMode.start;
        hasActed = false;
        hasMoved = false;
        UIManager.instance.NextTurnButton.GetComponent<UnityEngine.UI.Button>().enabled = true;
        UIManager.instance.UpdateButtons();
        CharacterLogic.instance.CreateAttackList(CurrentlyTakingTurn);
        UIManager.instance.CreateActionButton(CurrentlyTakingTurn.AvailableActions);
        SelectionScript.ClearSelection();
        CurrentlyTakingTurn.characterPosition.SelectThis();
        CameraScript.instance.SetMoveTarget(CurrentlyTakingTurn.gameObject);
        CurrentlyTakingTurn.characterEnergy = 0;
       
        if (CurrentlyTakingTurn.isAi)
        {
            CurrentlyTakingTurn.GetComponent<AiModule>().TakeTurn();
        }
    }

    public void Move()
    {
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasMoved)
        {
            mode = TurnMode.move;
            CharacterLogic.instance.Move(CurrentlyTakingTurn);
        }
        else
        {
            Debug.Log("Has already moved");
        }
    }

    public void Action(ActionType at)
    {
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasActed)
        {
            mode = TurnMode.action;
            CharacterLogic.instance.Action(CurrentlyTakingTurn, CurrentlyTakingTurn.AvailableActionDictionary[at]);
        }
        else
        {
            Debug.Log("Has already acted");
        }
    }

    public void Action(ActionType at, ItemBase ib)
    {
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasActed)
        {
            mode = TurnMode.action;
            CharacterLogic.instance.Action(CurrentlyTakingTurn, CurrentlyTakingTurn.AvailableActionDictionary[at], ib);
        }
        else
        {
            Debug.Log("Has already acted");
        }
    }

    public void Action(int id)
    {
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasActed)
        {
            mode = TurnMode.action;
            CharacterLogic.instance.Action(CurrentlyTakingTurn, ItemList.GetItem(id));
        }
        else
        {
            Debug.Log("Has already acted");
        }
    }

    public bool CheckIfTurnDone()
    {
        return TurnManager.instance.hasMoved && TurnManager.instance.hasActed;
    }

    public void FacingPhase()
    {
        UIManager.instance.CloseInventory();
        SelectionScript.ClearSelection();
        CameraScript.instance.SetMoveTarget(CurrentlyTakingTurn.gameObject);
        if (CurrentlyTakingTurn!=null && !CurrentlyTakingTurn.isAi)
        {
            mode = TurnMode.facing;
            UIManager.instance.StartSelectFacingPhase(CurrentlyTakingTurn);
            //UIManager.instance.UpdateButtons();
        }
    }

    public void CheckAliveStatus()
    {
        List<Character> deadCharacter = new List<Character>();
        foreach(Character chara in characters)
        {
            if(!chara.isAlive)
            {
                deadCharacter.Add(chara);
                chara.GetComponent<Animator>().SetBool("Dead", true);
                chara.Hp = 0;
            }
        }

        foreach(Character cha in deadCharacter)
        {
            characters.Remove(cha);
        }

        CheckIfGameOver();
    }

    public bool CheckIfGameOver()
    {
        bool teamAi = false;
        bool teamB = false;

        foreach (Character chara in characters)
        {
            if (chara.isAi)
            {
                teamAi = true;
            }
            else
            {
                teamB = true;
            }
        }

        if (!(teamAi && teamB))
        { 
            Debug.Log("Game Over");
            UIManager.instance.DisplayGameOver(teamB);
            return true;
        }

        return false;
    }

}
