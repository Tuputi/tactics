using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RotationButton : ButtonScript {

    public List<ButtonScript> toggleGroup;

    public override void SetUp()
    {
        button = this.GetComponent<Button>();
        MyImage = transform.GetComponentInChildren<Image>();
        UnselectButton();
    }

    void Awake()
    {
        SetUp();
    }

    public override void SelectAction()
    {
        if (Selected)
        {
            CameraController.rotationOn = false;
            UnselectButton();
        }
        else {
            CameraController.rotationOn = true;
            SelectButton();
        }
    }

    public override void UnselectButton()
    {
        CameraController.rotationOn = false;
        MyImage.sprite = UnselectedButton;
        Selected = false;
    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
        Selected = true;
        foreach(ButtonScript bs in toggleGroup)
        {
            bs.UnselectButton();
        }
    }

    public override void UpdateButton()
    {
       
    }
}
