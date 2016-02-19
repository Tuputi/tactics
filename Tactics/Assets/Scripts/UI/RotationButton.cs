using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotationButton : ButtonScript {

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
        MyImage.sprite = UnselectedButton;
        Selected = false;
    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
        Selected = true;
    }

    public override void UpdateButton()
    {
       
    }
}
