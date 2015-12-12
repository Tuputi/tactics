using UnityEngine;
using System.Collections;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;
    public GameObject tile_base;

    void Awake()
    {
        instance = this;
    }
}
