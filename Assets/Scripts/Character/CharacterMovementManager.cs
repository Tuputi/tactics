using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovementManager : MonoBehaviour {

    public static GameObject character;
    public static Character chara;
    static int distanceToGo = 0;
    static List<Tile> pathToGo = new List<Tile>();
    
    
    void Start()
    {
       // character = GameObject.Find("Character");
    }


    void FixedUpdate()
    {
        if (distanceToGo > 0)
        {
          
            bool moveOn = MoveBetween(character.transform.position, new Vector3(pathToGo[distanceToGo - 1].positionRow, pathToGo[distanceToGo - 1].height, pathToGo[distanceToGo - 1].positionColumn));
            if (moveOn)
            {
                if (distanceToGo - 2 >= 0)
                {
                    chara.ChangeFacing(pathToGo[distanceToGo - 1], pathToGo[distanceToGo - 2]);
                }
                distanceToGo--;
            }
        }
        else
        {
            if (!(chara == null))
            {
                chara.gameObject.GetComponent<Animator>().SetBool("Moving", false);
            }
        }
    }

    public static void SetPosition(int x, float tileheight, int y, Character myCharacter)
    {
        chara = myCharacter;
        character = myCharacter.gameObject;
        character.transform.position = new Vector3(x, tileheight, y);
    }

    public static void SetPath(List<Tile> path)
    {
        distanceToGo = path.Count;
        pathToGo = path;
        chara.gameObject.GetComponent<Animator>().SetBool("Moving", true);
    }

     public bool MoveBetween(Vector3 startPos, Vector3 endPos)
     {
         character.transform.position = Vector3.MoveTowards(startPos, endPos, 0.04f);
         if(character.transform.position == endPos)
         {
             return true;
         }
         return false;
     }
}
