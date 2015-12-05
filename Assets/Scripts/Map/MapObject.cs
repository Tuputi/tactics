using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour {

    public string ObjectName;
    public int ObjectId;
    MapObjectType type;

    public enum MapObjectType { none, tree}
}
