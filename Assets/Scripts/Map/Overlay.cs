using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {

    public OverlayType overlayType = OverlayType.Path;
    private GameObject prefab;
    public static Overlay instance;


    void Awake()
    {
        instance = this;
    }

    public void SetType(OverlayType typeOfOverlay, Tile tile)
    {
        overlayType = typeOfOverlay;
        switch (typeOfOverlay)
        {
            case OverlayType.Path:
                prefab = PrefabHolder.instance.Overlay_Path_Prefab;
                break;
            case OverlayType.Attack:
                prefab = PrefabHolder.instance.Overlay_Attack_Prefab;
                break;
            case OverlayType.Empty:
                prefab = PrefabHolder.instance.Overlay_Empty_Prefab;
                break;
        }
        GenerateVisuals(tile);
    }

    public void GenerateVisuals(Tile tile)
    {
        GameObject overlayContainer = tile.transform.FindChild("Overlay").gameObject;
        //remove children
        for (int i = 0; i < overlayContainer.transform.childCount; i++)
        {
            Destroy(overlayContainer.transform.GetChild(i).gameObject);
        }

        GameObject newOverlay= (GameObject)Instantiate(prefab, overlayContainer.transform.position, Quaternion.identity);
        newOverlay.transform.position = new Vector3(overlayContainer.transform.position.x, overlayContainer.transform.position.y, overlayContainer.transform.position.z);
        newOverlay.transform.parent = overlayContainer.transform;

    }

}
