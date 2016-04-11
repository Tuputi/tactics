using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class MageInventory : UIInventory {

    public int inventorySlotAmount;
    public float RotationAmount;
    public InventorySlot invSlotTemplate;
    public GameObject rotationPoint;
    private GameObject rotationHelper;

    public SpellForm spellForm;


    void Awake()
    {
        CreateInventory();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateBy(20f);
        }
    }

    public new void CreateInventory()
    {
        InventorySlots = new List<InventorySlot>();
        PlaceEvenly();
        rotationHelper = new GameObject();
        rotationHelper.name = "RotationHelperMage";
    }

    private void PlaceEvenly()
    {
        float angleBetweenObjects = 360 / inventorySlotAmount + 1;
        RotationAmount = angleBetweenObjects + 3f;
        float distanceBetweenObjects = 450f;
        double tempDouble = System.Math.Cos(angleBetweenObjects * 0.5);
        float tempFloat = (float)tempDouble;
        float distanceFromCenter = (distanceBetweenObjects * 0.5f) / tempFloat;
        float accumulatedAngle = 360.0f;
        //foreach (GameObject go in ButtonObjects )
        for (int i = 0; i < inventorySlotAmount - 1; i++)
        {
            GameObject newG = Instantiate(invSlotTemplate.gameObject);
            newG.transform.SetParent(rotationPoint.transform);
            newG.transform.localScale = new Vector3(1, 1, 1);
            double cosResult = System.Math.Cos(accumulatedAngle) * distanceFromCenter;
            double sinResult = System.Math.Sin(accumulatedAngle) * distanceFromCenter;
            float cosFloat = (float)cosResult;
            float sinFloat = (float)sinResult;
            newG.transform.localPosition = new Vector3(cosFloat, sinFloat, 0);
            accumulatedAngle -= angleBetweenObjects;
            newG.GetComponent<InventorySlot>().Init();
            newG.AddComponent<DraggableObject>();
            newG.GetComponent<DraggableObject>().Init(newG.transform.localPosition);
            InventorySlots.Add(newG.GetComponent<InventorySlot>());
        }
    }

    public List<InventorySlot> GetAllActiveButtons()
    {
        return InventorySlots;
    }

    public void UpdateAllItemSlots()
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase item = TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetItem(slot.MyItem.ItemInstanceID);
                if(item == null)
                {
                    break;
                }
                slot.ClearSlot();
                slot.AddItem(item, item.ItemSprite, item.ItemCount);

                if(slot.MyItem.ItemCount <= 0)
                {
                    slot.GetComponent<DraggableObject>().draggable = false;
                }
                else
                {
                    slot.GetComponent<DraggableObject>().draggable = true;
                }
            }
        }
    }

    public void RotateBy(float amount)
    {
        Debug.Log("Rotate by");
        rotationHelper.transform.eulerAngles = rotationPoint.transform.eulerAngles + new Vector3(0, 0, amount);
        StartCoroutine(RotateDial(rotationHelper.transform));
    }

    public void RotateTo(float degree)
    {
        rotationHelper.transform.eulerAngles = new Vector3(0, 0, degree);
        StartCoroutine(RotateDial(rotationHelper.transform));
    }

    IEnumerator RotateDial(Transform myTarget)
    {
        Vector3 sourceRot = rotationPoint.transform.eulerAngles;
        Vector3 targetRot = myTarget.eulerAngles;
        // Debug.Log(sourceRot);
        // Debug.Log(targetRot);

        float i = 0.0f;
        while (i < 1.0f)
        {
            float previousRotation = rotationPoint.transform.eulerAngles.z;
            rotationPoint.transform.eulerAngles = Vector3.Lerp(sourceRot, targetRot, Mathf.SmoothStep(0, 1f, i));
            float currentRotation = rotationPoint.transform.eulerAngles.z;
            RotateSlots(previousRotation - currentRotation);
            i += Time.deltaTime;
            yield return 0;
        }
    }

    public void RotateSlots(float amount)
    {
        for (int i = 0; i < rotationPoint.transform.childCount; i++)
        {
            rotationPoint.transform.GetChild(i).gameObject.transform.Rotate(new Vector3(0, 0, amount), Space.Self);
        }
    }

    public override void SelectASlot(InventorySlot iS)
    {
        foreach(InventorySlot slot in InventorySlots)
        { 
            slot.GetComponent<DraggableObject>().ReturnToOrigLocation();
            slot.UnselectSlot();
            slot.GetComponent<DraggableObject>().selected = false;
        }

        iS.SelectSlot();
        iS.transform.GetComponent<DraggableObject>().selected = true;
    }

    public void UnselectAllSlots()
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            slot.GetComponent<DraggableObject>().ReturnToOrigLocation();
            slot.UnselectSlot();
            slot.GetComponent<DraggableObject>().selected = false;
        }
    }

    public override void CloseInventory()
    {
        foreach(IncredientSlot inSlot in spellForm.IncredientSlots)
        {
            inSlot.ClearSlot();
        }

        UnselectSlots();
        foreach (InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                slot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }

    public override void CloseInventoryAfterAttack()
    {
        foreach (IncredientSlot inSlot in spellForm.IncredientSlots)
        {
            inSlot.ClearSlotsWithoutReplacing();
        }

        UnselectSlots();
        foreach (InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                slot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }


}
