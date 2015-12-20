using UnityEngine;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class TurnManager : MonoBehaviour {

    public static OrderedBag<Character> characters;
    public float EnergyThreshold = 50f;
    public Character CurrentlyTakingTurn;

    void Awake()
    {
        characters = new OrderedBag<Character>();
    }

	public void CreateCharacterList()
    {
        GameObject characterHolder = GameObject.Find("Characters");
        for(int i = 0; i < characterHolder.transform.childCount; i++)
        {
            characters.Add(characterHolder.transform.GetChild(i).GetComponent<Character>());
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
        CurrentlyTakingTurn.characterPosition.SelectThis();
        CameraScript.instance.SetMoveTarget(CurrentlyTakingTurn.gameObject);
        //CameraScript.instance.MoveToTargetFunc(CurrentlyTakingTurn.gameObject.transform);
        CurrentlyTakingTurn.characterEnergy = 0;
    }
}
