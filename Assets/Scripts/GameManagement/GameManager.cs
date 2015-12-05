using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    void Start()
    {
        instance = this;
    }


   public bool VictoryConditionReached(List<Character> charaList)
    {
        bool teamRed = false;
        bool teamBlue = false;
        foreach(Character chara in charaList)
        {
            if (chara.isAlive)
            {
                if (!teamRed)
                {
                    if (chara.team == Team.Red)
                    {
                        teamRed = true;
                    }
                }
                if (!teamBlue)
                {
                    if (chara.team == Team.Blue)
                    {
                        teamBlue = true;
                    }
                }
            }
        }
        if (teamBlue == teamRed)
        {
            Debug.Log("Both teams alive");
            return false;
        }
        if(teamBlue && !teamRed)
        {
            Debug.Log("Team blue won");
            return true;

        }
       if(teamRed && !teamBlue)
        {
            Debug.Log("Team red won");
            return true;
        }
        return false;
    }


}
