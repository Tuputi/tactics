using UnityEngine;
using System.Collections.Generic;

public class NameCreator : MonoBehaviour {


    public List<string> names;
    public List<string> animalNames;

    public List<string> usedNames;

    public static NameCreator instance;


    void Awake()
    {
        instance = this;
        usedNames = new List<string>();
    }

    public bool IsNameAlreadyUsed(string name)
    {
        return usedNames.Contains(name);
    }


    public string GetAName(NameType nameType)
    {
        List<string> nameList = new List<string>(names);
        switch (nameType)
        {
            case NameType.Rat:
                nameList = new List<string>(names);
                break;
            case NameType.Animal:
                nameList = new List<string>(animalNames);
                break;
            default:
                break;
        }
        int index = Random.Range(0, nameList.Count);
        string name = nameList[index];

        switch (nameType)
        {
            case NameType.Rat:
                names.Remove(name);
                break;
            case NameType.Animal:
                animalNames.Remove(name);
                break;
            default:
                break;
        }
        usedNames.Add(name);
        return name;
    }

    public void AddANameToUsed(string name)
    {
        usedNames.Add(name);
    }

}
