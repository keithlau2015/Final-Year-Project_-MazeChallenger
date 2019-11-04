using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Exit[] exits;
    public MeshCollider meshCollider;
    public Transform[] spwanPoint_Food, spwanPoint_Weapon, spwanPoint_Enemy, spwanPoint_Coins;

    public Bounds RoomBounds
    {
        get { return meshCollider.bounds; }
    }
}
