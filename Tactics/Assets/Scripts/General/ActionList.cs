using UnityEngine;
using System.Collections.Generic;

public class ActionList : MonoBehaviour {

    public List<AttackBase> actions;

    public static Dictionary<ActionType, AttackBase> actionDictionary;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        actionDictionary = new Dictionary<ActionType, AttackBase>();
        foreach (AttackBase action in actions)
        {
            actionDictionary.Add(action.actionType, action);
        }
    }

    public static AttackBase GetAction(ActionType actionType)
    {
        return actionDictionary[actionType];
    }

}
