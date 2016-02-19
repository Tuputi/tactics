using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ButtonScript : MonoBehaviour {

    public string ButtonText;
    [HideInInspector]
    public Button button;
    public Sprite SelectedButton;
    public Sprite UnselectedButton;
    protected Image MyImage;
    protected bool Selected = false;

    public abstract void SetUp();

    public abstract void UpdateButton();

    public abstract void SelectAction();

    public abstract void UnselectButton();

    public abstract void SelectButton();

}
