using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();
    public Vector2 iterationRange = new Vector2(3, 10);

    List<Exit> availableExits = new List<Exit>();

    StartRoom startRoom;
    EndRoom endRoom;

    List<Room> placedRooms = new List<Room>();

    LayerMask roomLayerMask;

    private void Start()
    {
        roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        PlaceStartRoom();
        Debug.Log("Place start room");
        yield return interval;

        int iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);

        for(int i = 0; i < iterations; i++)
        {
            PlaceRoom();
            Debug.Log("Place room");
            yield return interval;
        }

        PlaceEndRoom();
        Debug.Log("Place end room");
        yield return interval;
    }

    private void AddExitsToList(Room room, ref List<Exit> list)
    {
        foreach(Exit exit in room.exits)
        {
            int rand = Random.Range(0, list.Count);
            list.Insert(rand, exit);
        }
    }

    private void PlaceStartRoom()
    {
        startRoom = Instantiate(startRoomPrefab) as StartRoom;
        startRoom.transform.parent = this.transform;

        AddExitsToList(startRoom, ref availableExits);

        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
    }

    private void PlaceEndRoom()
    {
        endRoom = Instantiate(endRoomPrefab) as EndRoom;
        endRoom.transform.parent = this.transform;

        List<Exit> allAvailabeExits = new List<Exit>(availableExits);
        Exit exit = endRoom.exits[0];

        bool roomPlaced = false;

        foreach (Exit availableExit in allAvailabeExits)
        {
            Room room = (Room)endRoom;
            PositionRoomAtExit(ref room, exit, availableExit);

            if (CheckRoomOverlap(room))
            {
                continue;
            }
            roomPlaced = true;
            exit.gameObject.SetActive(false);
            availableExits.Remove(exit);

            availableExit.gameObject.SetActive(false);
            availableExits.Remove(availableExit);

            break;
        }

        if (!roomPlaced)
        {
            ResetLevel();
        }
    }

    private void PlaceRoom()
    {
        Room currentRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        List<Exit> allAvailableExits = new List<Exit>(availableExits);
        List<Exit> currentExits = new List<Exit>();
        AddExitsToList(currentRoom, ref currentExits);

        AddExitsToList(currentRoom, ref availableExits);

        bool roomPlaced = false;

        foreach(Exit availableExit in allAvailableExits)
        {
            foreach(Exit currentExit in currentExits)
            {
                PositionRoomAtExit(ref currentRoom, currentExit, availableExit);

                if (CheckRoomOverlap(currentRoom))
                {
                    continue;
                }

                roomPlaced = true;

                placedRooms.Add(currentRoom);

                currentExit.gameObject.SetActive(false);
                availableExits.Remove(currentExit);

                availableExit.gameObject.SetActive(false);
                availableExits.Remove(availableExit);

                break;
            }

            if (roomPlaced)
            {
                break;
            }
        }

        if (!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetLevel();
        }
    }

    private void PositionRoomAtExit(ref Room room, Exit exit, Exit targetExit)
    {
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;

        Vector3 targetExitEuler = targetExit.transform.eulerAngles;
        Vector3 roomExitEuler = exit.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(roomExitEuler.y, targetExitEuler.y);
        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);

        Vector3 roomPositionOffset = exit.transform.position - room.transform.position;
        room.transform.position = targetExit.transform.position - roomPositionOffset;
    }

    private bool CheckRoomOverlap(Room room)
    {
        Bounds bounds = room.RoomBounds;
        bounds.Expand(-0.1f);

        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, room.transform.rotation, roomLayerMask);
        if(colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                if (c.transform.parent.gameObject.Equals(room.gameObject))
                {
                    continue;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ResetLevel()
    {
        StopCoroutine("GenerateLevel");
        if (startRoom)
        {
            Destroy(startRoom.gameObject);
        }
        if (endRoom)
        {
            Destroy(endRoom.gameObject);
        }
        foreach(Room room in placedRooms)
        {
            Destroy(room.gameObject);
        }
        placedRooms.Clear();
        availableExits.Clear();
        StartCoroutine("GenerateLevel");
    }
}
