using UnityEngine;
using System.Collections.Generic;

public class Container : MonoBehaviour {

    public List<Craftable> items;
    public GameObject layoutgroup;
    public GameObject slottemplate;

    public InventorySlot slot;

    void Start()
    {
        CreateContainer();
    }

    public void CreateContainer()
    {
        foreach(Craftable craft in items)
        {
            GameObject newObj = Instantiate(slottemplate);
            newObj.GetComponent<InventorySlot>().Init();
            newObj.GetComponent<InventorySlot>().AddItem(craft);
            newObj.transform.SetParent(layoutgroup.transform);
            newObj.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
