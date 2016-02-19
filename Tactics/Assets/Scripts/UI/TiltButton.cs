using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TiltButton : ButtonScript {

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
    }

    public override void UpdateButton()
    {

    }
}
