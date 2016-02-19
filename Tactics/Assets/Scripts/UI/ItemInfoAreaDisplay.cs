using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemInfoAreaDisplay : MonoBehaviour {

    public Image tileImage;
    public GameObject areaDisplayHolder;
    public List<Image> tiles;
    public int Rows;
    public float tileWidth;
    public int centerPosition = 12;
    public static ItemInfoAreaDisplay instance;


    void Awake()
    {
        Init();
        instance = this;
    }

    public void Init()
    {
        CreateAGrid();
        //LightUpRange(TargetAreaType.line, 2);
    }

    public void CreateAGrid()
    {
        for(float x = 0; x < Rows; x++)
        {
            for(float y = 0; y<Rows; y++)
            {
                Image newTile = Instantiate(tileImage);
                newTile.gameObject.transform.SetParent(areaDisplayHolder.transform, false);
                RectTransform tileRect = newTile.GetComponent<RectTransform>();
                tileRect.transform.localPosition = new Vector3((x*(tileWidth+ 5f)), (y * (-tileWidth - 5f)), 0);
                newTile.name = (tiles.Count).ToString();
                tiles.Add(newTile);
            }
        }
    }

    public void LightUpRange(TargetAreaType tat, float range)
    {
        if(range + 2 > Rows)
        {
            range = Rows - 2;
        }

        switch (tat)
        {
            case TargetAreaType.none:
                break;
            case TargetAreaType.self:
                tiles[12].GetComponent<Image>().color = Color.white;
                break;
            case TargetAreaType.circular:
                tiles[centerPosition].GetComponent<Image>().color = Color.white;
                for (int i = 0; i < range; i++)
                {
                    tiles[centerPosition - i].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition + i].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition - (i * Rows)].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition + (i * Rows)].GetComponent<Image>().color = Color.white;
                    

                    tiles[centerPosition + 4].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition - 4].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition + 6].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition - 6].GetComponent<Image>().color = Color.white;
                }
                break;
            case TargetAreaType.line:
                tiles[centerPosition].GetComponent<Image>().color = Color.white;
                for (int i = 0; i < range; i++)
                {
                    tiles[centerPosition - i].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition + i].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition - (i * Rows)].GetComponent<Image>().color = Color.white;
                    tiles[centerPosition + (i * Rows)].GetComponent<Image>().color = Color.white;
                }
                break;
            default:
                break;
        }
    }

    public void SlackLights()
    {
        foreach(Image i in tiles)
        {
            i.color = Color.grey;
        }
    }

}
