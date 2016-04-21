using UnityEngine;
using System.Collections;
using System;

public class TimeCard : MonoBehaviour, System.IComparable
{
    //compartors
    bool Equals(TimeCard other)
    {
        if (other.name.Equals(this.name))
        {
           return true;
        }
        return false;
    }

    public int CompareTo(object other)
    {
        TimeCard otherCard = (TimeCard)other;
        if (otherCard != null)
        {
            if (otherCard.Timing < this.Timing)
            {
                return -1;
            }
            else if (otherCard.Timing > this.Timing)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            throw new System.ArgumentException("Object is not a TimeCard");
        }
    }

    public string Name;
    public Sprite Portrait;
    public float Timing;

    public void UpdateGraphics()
    {
        transform.FindChild("Name").GetComponent<UnityEngine.UI.Text>().text = Name;
    }
}
