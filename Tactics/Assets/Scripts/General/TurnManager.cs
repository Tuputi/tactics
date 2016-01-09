using UnityEngine;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class TurnManager : MonoBehaviour {

    public enum TurnMode {  start, move, action, end};
    public enum GameMode { Editor, Game};

    public static TurnManager instance;
    public static OrderedBag<Character> characters;
    public float EnergyThreshold = 50f;
    public Character CurrentlyTakingTurn;


    public static TurnMode mode;
    public static GameMode gameMode;
    public bool hasMoved = false;
    public bool hasActed = false;
    

    void Awake()
    {
        instance = this;
        characters = new OrderedBag<Character>();
        gameMode = GameMode.Game;
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
                chara.CreateInventory();
            }
        }
    }

    void AddEnergy()
    {
        OrderedBag<Character> tempList = new OrderedBag<Character>(characters);
        characters.Clear();
        foreach(Character chara in tempList)
        {
            chara.characterEnergy += chara.speed;
            characters.Add(chara);
        }
    }

    public void NextInTurn()
    {
        if(!(characters.Count > 0))
        {
            CreateCharacterList();
        }

        if(CurrentlyTakingTurn != null)
        {
            foreach (Tile t in CurrentlyTakingTurn.possibleRange)
            {
                t.SetOverlayType(OverlayType.None);
            }
        }

        Character nextCharacter = null;
        while(nextCharacter == null)
        {
            if(characters.GetFirst().characterEnergy >= EnergyThreshold)
            {
                nextCharacter = characters.GetFirst();
            }
            else
            {
                AddEnergy();
            }
        }
        Debug.Log("Next in turn is " + nextCharacter.characterName);
        CurrentlyTakingTurn = nextCharacter;
        TakeTurn();
    }

    public void TakeTurn()
    {
        hasActed = false;
        hasMoved = false;
        UIManager.instance.UpdateButtons();
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
        foreach(Tile t in CurrentlyTakingTurn.possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasMoved)
        {
            mode = TurnMode.move;
            CurrentlyTakingTurn.Move();
        }
        else
        {
            Debug.Log("Has already moved");
        }
    }

    public void Action(ActionType at)
    {
        foreach (Tile t in CurrentlyTakingTurn.possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasActed)
        {
            mode = TurnMode.action;
            CurrentlyTakingTurn.Action(PrefabHolder.instance.actionDictionary[at]);
        }
        else
        {
            Debug.Log("Has already acted");
        }
    }

    public void Action(ItemType it)
    {
        foreach (Tile t in CurrentlyTakingTurn.possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        CurrentlyTakingTurn.possibleRange.Clear();
        SelectionScript.ClearSelection();
        if (!hasActed)
        {
            mode = TurnMode.action;
            CurrentlyTakingTurn.Action(PrefabHolder.instance.itemDictionary[it]);
        }
        else
        {
            Debug.Log("Has already acted");
        }
    }
}
