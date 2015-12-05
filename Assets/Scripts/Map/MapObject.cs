using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour {

    public string ObjectName;
    public Tile position;
    public int ObjectId;
    public MapObjectType type;
    public Facing facing = Facing.Up;

    public enum MapObjectType { none, obstacle}

    void Awake(){
       
    }


    public void ChangeFacing(Facing faceDirection)
    {
        GameObject rotateObj = this.gameObject;
        float rotation = -1f;
        switch (faceDirection)
        {
            case Facing.Left:
                facing = Facing.Left;
                if (position != null)
                {
                    position.objectFacing = facing;
                }
                rotation = 180f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Right:
                facing = Facing.Right;
                if (position != null)
                {
                    position.objectFacing = facing;
                }
                rotation = 0f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Up:
                facing = Facing.Up;
                if (position != null)
                {
                    position.objectFacing = facing;
                }
                rotation = 270f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Down:
                facing = Facing.Down;
                if (position != null)
                {
                    position.objectFacing = facing;
                }
                rotation = 90f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            default:
                break;
        }
    }
}
