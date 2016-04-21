using UnityEngine;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class TurnOrderTimeline : MonoBehaviour {

    private static OrderedBag<TimeCard> timeline;
    public TimeCard TimeCardTemplate;

    private static TimeCard myTemplate;
    public static TurnOrderTimeline instance;

    void Awake()
    {
        instance = this;
        myTemplate = TimeCardTemplate;
        timeline = new OrderedBag<TimeCard>();
    }

    public static void AddATimeCard(Character chara)
    {
        TimeCard newCard = (TimeCard)Instantiate(myTemplate);
        newCard.Name = chara.characterName;
        newCard.Timing = chara.characterEnergy;
        newCard.transform.SetParent(instance.gameObject.transform);
        newCard.transform.localScale = new Vector3(1, 1, 1);
        newCard.UpdateGraphics();
        timeline.Add(newCard);
    }

    public static void MoveATimeCard(Character chara)
    {
        TimeCard myCard = null;
        foreach (TimeCard card in timeline)
        {
            if (card.Name.Equals(chara.characterName))
            {
                myCard = card;
                break;
            }
        }

        if (myCard != null)
        {
            timeline.Remove(myCard);
            myCard.Timing = chara.characterEnergy;
            timeline.Add(myCard);
            foreach (TimeCard tCard in timeline)
            {
                tCard.transform.SetAsLastSibling();
            }
        }
        else
        {
            Debug.Log("Chara not found for timeline");
        }
    }
	
}
