﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> RoomPrefab = new List<GameObject>();

    [SerializeField]
    private Transform origin;

    // Start is called before the first frame update
    void Start()
    {
        SpawnStartingRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnStartingRooms()
    {
        GameObject StartingPoint = Instantiate(RoomPrefab[(int)Random.value].gameObject, origin) as GameObject;
        GameObject[] exitTemp = GameObject.FindGameObjectsWithTag("Exit");
        for (int roomCount = 0; roomCount < RoomPrefab.Capacity; roomCount++)
        {
            for (int exitCount = 0; exitCount < exitTemp.Length; exitCount++)
            {
                GameObject cloneRoom = Instantiate(RoomPrefab[(int)Random.value], exitTemp[exitCount].transform);
                GameObject[] cloneRoomExit = GameObject.FindGameObjectsWithTag("Exit");
                float offsetX, offsetY, offsetZ;
                offsetX = cloneRoom.transform.localPosition.x - cloneRoomExit[exitCount].transform.position.x;
                offsetY = cloneRoom.transform.localPosition.y - cloneRoomExit[exitCount].transform.position.y;
                offsetZ = cloneRoom.transform.localPosition.z - cloneRoomExit[exitCount].transform.position.z;
                cloneRoom.transform.position = exitTemp[exitCount].transform.position - new Vector3(offsetX, offsetY, offsetZ);
            }
        }
    }
}
