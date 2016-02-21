using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TiltButton : ButtonScript {

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
            CameraController.tiltOn = false;
            UnselectButton();
        }
        else {
            CameraController.tiltOn = true;
            SelectButton();
        }
    }

    public override void UnselectButton()
    {
        CameraController.tiltOn = false;
        MyImage.sprite = UnselectedButton;
        Selected = false;
    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
        Selected = true;
        foreach (ButtonScript bs in toggleGroup)
        {
            bs.UnselectButton();
        }
    }

    public override void UpdateButton()
    {

    }
}
