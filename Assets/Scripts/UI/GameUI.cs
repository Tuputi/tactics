using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public static Slider healthBar;
    private static GameObject statObject;
    private static Text currentHP;
    private static Text maxHP;
    private static Text characterName;
    private static Image characterPortrait;



    //attackInfo
    public GameObject attackPanel;
    public static Text attackNameText;
    public static Text likelyDamageText;

    private static GameObject attackPanelObject;

    


    void Start()
    {
        //healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        currentHP = GameObject.Find("currentHPtext").GetComponent<Text>();
        maxHP = GameObject.Find("maxHPtext").GetComponent<Text>();
        characterName = GameObject.Find("characterNametext").GetComponent<Text>();
        characterPortrait = GameObject.Find("Character_portrait").GetComponent<Image>();

        statObject = GameObject.Find("StatusUI").gameObject;
        attackPanelObject = attackPanel;

    }


    public static void UpdateStatUI(Character currentCharacter)
    {
        ShowStatUI(true);
       // healthBar.maxValue = currentCharacter.characterMaxHealth;
      //  healthBar.value = currentCharacter.characterHealth;

        currentHP.text = currentCharacter.characterHealth.ToString();
        maxHP.text = "/"+currentCharacter.characterMaxHealth.ToString();

        characterName.text = currentCharacter.characterName;

        characterPortrait.sprite = currentCharacter.characterSprite;

    }

    public static void ShowStatUI(bool on)
    {
        //statObject.SetActive(on);
    }

    public void AbilityMenu(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }


    public void WalkButton()
    {
        if (!TurnManager.HasMoved)
        {
            MapCreatorManager.instance.ClearButton();
            TurnManager.currentlytakingTurn.Move();
            TurnManager.turnState = TurnState.Move;

        }
        else
        {
            Debug.Log("Already done this this turn");
        }
    }

    public void AttackButton()
    {
        if (!TurnManager.HasAttacked)
        {
            MapCreatorManager.instance.ClearButton();

            TurnManager.currentlytakingTurn.Attack(AttackType.Meelee);
            TurnManager.turnState = TurnState.Attack;

        }
        else
        {
            Debug.Log("Already done this this turn");
        }
    }

    public void RangeAttackButton()
    {
        if (!TurnManager.HasAttacked)
        {
            MapCreatorManager.instance.ClearButton();

            TurnManager.currentlytakingTurn.Attack(AttackType.Ranged);
            TurnManager.turnState = TurnState.Attack;

        }
        else
        {
            Debug.Log("Already done this this turn");
        }
    }

    public void AreaEffectButton()
    {
        if (!TurnManager.HasAttacked)
        {
            MapCreatorManager.instance.ClearButton();

            TurnManager.currentlytakingTurn.Attack(AttackType.AreaOfEffect);
            TurnManager.turnState = TurnState.Attack;

        }
        else
        {
            Debug.Log("Already done this this turn");
        }
    }


    public static void showDamage(Character chara, int amountOfDamage)
    {
        GameObject damageText = Instantiate(PrefabHolder.instance.damagePrefab) as GameObject;
        Text text = damageText.GetComponentInChildren<Text>();
        text.text = Mathf.Abs(amountOfDamage).ToString();
        if(amountOfDamage > 0)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
        damageText.transform.SetParent(chara.gameObject.transform);
        damageText.transform.localPosition = new Vector3(0, 2.5f, 0);
    }

   

    public static void AttackInformation()
    {  
        attackPanelObject.SetActive(true);
        if(attackNameText == null)
        {
            attackNameText = attackPanelObject.transform.FindChild("AttackNameText").GetComponent<Text>();
            likelyDamageText = attackPanelObject.transform.FindChild("LikelyDamageText").GetComponent<Text>();
        }

        attackNameText.text = TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].attackName;
        likelyDamageText.text = TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].likelyDamageHigh.ToString();

    }

    public void CancelAttackButton()
    {
        TurnManager.ClearAttack();
    }

    public void ConfirmAttack()
    {
        TurnManager.currentlytakingTurn.CompleteAttack(TurnManager.targetTile);
        MapCreatorManager.instance.ClearButton();
    }


}
