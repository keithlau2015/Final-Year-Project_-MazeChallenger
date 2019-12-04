using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Exit[] exits;
    public MeshCollider meshCollider;
    public Transform[] spawnPoint_Food, spawnPoint_Weapon, spawnPoint_Enemy, spawnPoint_Coins, spawnPoint_VendingMachines, spawnPoint_Books;

    public Bounds RoomBounds
    {
        get { return meshCollider.bounds; }
    }
}
