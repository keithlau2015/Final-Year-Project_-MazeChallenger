using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();
    public List<Weapon> weaponPrefabs = new List<Weapon>();
    public List<Food> foodPrefabs = new List<Food>();
    private int[] iterationRange = {6,7,8,11,12,13};

    private List<Exit> availableExits = new List<Exit>();
    
    private StartRoom startRoom;
    private EndRoom endRoom;

    private List<Room> placedRooms = new List<Room>();
    private List<Weapon> placedWeapons = new List<Weapon>();

    private LayerMask roomLayerMask;

    private GameObject player;

    private bool finishBuildLevel = false;

    private void Start()
    {
        roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine("GenerateLevel");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        PlaceStartRoom();
        Debug.Log("Place start room");
        yield return interval;

        int iterations = iterationRange[Random.Range(0, iterationRange.Length)];

        for(int i = 0; i < iterations; i++)
        {
            PlaceRoom();
            Debug.Log("Place room");
            yield return interval;
        }

        PlaceEndRoom();
        Debug.Log("Place end room");
        yield return interval;

        player.transform.position = startRoom.playerStartingPosition.position;
        player.transform.rotation = Quaternion.identity;
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

            finishBuildLevel = true;

            break;
        }

        if (!roomPlaced)
        {
            ResetLevel();
            finishBuildLevel = false;
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
                if(!currentRoom.spwanPoint_Weapon.Equals(null))PlaceWeapon(weaponPrefabs[Random.Range(0, weaponPrefabs.Capacity)], currentRoom);
                currentExit.gameObject.SetActive(false);
                availableExits.Remove(currentExit);

                availableExit.gameObject.SetActive(false);
                availableExits.Remove(availableExit);
                break;
            }
        }

        if (roomPlaced == false)
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
        bounds.Expand(-2.12f);

        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, room.transform.rotation, roomLayerMask);
        if(colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                if (c.transform.parent.gameObject.Equals(room.gameObject))
                {
                    Debug.Log("continue");
                    continue;
                }
                else
                {
                    Debug.Log("the room the overlap");
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
        foreach(Weapon weapon in placedWeapons)
        {
            Destroy(weapon.gameObject);
        }
        placedWeapons.Clear();
        placedRooms.Clear();
        availableExits.Clear();
        StartCoroutine("GenerateLevel");
    }

    private void PlaceWeapon(Weapon weapon, Room room)
    {
        if (weapon.probability > 1)
        {
            Debug.Log("The weapon spwaning rate is out of range");
        }
        float rand = Random.Range(0f, 1f);
        if(rand <= weapon.probability)
        {
            Weapon currentSpawnedWeapon = Instantiate(weapon, room.spwanPoint_Weapon) as Weapon;
            currentSpawnedWeapon.transform.parent = this.transform;
            placedWeapons.Add(currentSpawnedWeapon);
        }
    }
}
