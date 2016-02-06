using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ButtonScript : MonoBehaviour {

    public string ButtonText;
    [HideInInspector]
    public Button button;

    public abstract void SetUp();

    public abstract void UpdateButton();

    public abstract void SelectButton();

}
