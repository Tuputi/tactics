using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Text ItemCount;

    public bool isEmpty = true;


    public void AddItem(Sprite sprite, int itemCount){
        if(itemCount > 1) {
            ItemCount.text = itemCount.ToString();
        }
        else
        {
            ItemCount.text = "";
        }

        isEmpty = false;
        if(sprite == null)
        {
            Debug.Log("no sprite");
        }
        this.GetComponent<Image>().sprite = sprite;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearSlot()
    {
        ItemCount.text = "";
        isEmpty = true;
        this.GetComponent<Image>().sprite = EmptySlot;
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
