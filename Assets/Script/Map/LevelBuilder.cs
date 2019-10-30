using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    //The user interface components
    public GameObject upgradeUI, pauseUI, loadingUI;
    [SerializeField]
    private Button Upgrade_slot_1, Upgrade_slot_2, Upgrade_slot_3;

    //The needed Prefabs
    public Room[] startRoomPrefab, endRoomPrefab, specialRoomPrefab;

    //The List of prefabs
    public List<Room> roomPrefabs = new List<Room>();
    public List<Weapon> weaponPrefabs = new List<Weapon>();
    public List<Food> foodPrefabs = new List<Food>();
    public List<Enemy> enemyPrefabs = new List<Enemy>();

    //The array control total rooms in game
    private int[] iterationRange = {6,7,8,9,11,12,13};

    //Saving for the exits which still available
    private List<Exit> availableExits = new List<Exit>();
    
    //Object for specific rooms
    private StartRoom startRoom;
    private EndRoom endRoom;
    private SpecialRoom specialRoom;

    //The List to saving prefabs which in the game
    private List<Room> placedRooms = new List<Room>();
    private List<Weapon> placedWeapons = new List<Weapon>();
    private List<Food> placedFoods = new List<Food>();
    private List<Enemy> placedEnemys = new List<Enemy>();

    //For the Room mesh layer
    private LayerMask roomLayerMask;

    //Spawning the player in the right place
    private GameObject player;

    private bool finishLevelBuilding;

    private void Start()
    {
        //The default game status
        finishLevelBuilding = false;
        loadingUI.SetActive(false);
        upgradeUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        roomLayerMask = LayerMask.GetMask("Room");

        //Starting to Generate the game level
        StartCoroutine("GenerateLevel");

        //Button listener
        Upgrade_slot_1.onClick.AddListener(onClickUpgradButton);
        Upgrade_slot_2.onClick.AddListener(onClickUpgradButton);
        Upgrade_slot_3.onClick.AddListener(onClickUpgradButton);
        //play the bg music
        FindObjectOfType<soundcontrol>().music_playing("game_bg");
    }

    private void Update()
    {
        if (upgradeUI.activeSelf)
        {
            ResetLevel();
        }
        if (!finishLevelBuilding)
        {
            loadingUI.SetActive(true);
            AudioListener.volume = 0f;
        }
        else
        {
            loadingUI.SetActive(false);
            AudioListener.volume = 1f;
        }
        if (PlayerStatus.Instance.getPlayerGetIntoNextLevel())
        {
            upgradeUI.SetActive(true);
        }
        else
        {
            upgradeUI.SetActive(false);
        }
    }

    IEnumerator GenerateLevel()
    {
        finishLevelBuilding = false;
        
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        //1 is the special room spawning rate
        if (1 >= Random.Range(0, 100) && specialRoomPrefab.Length != 0)
        {
            PlaceSpecialRoom();
            Debug.Log("Place a special room");
            player.transform.position = specialRoom.playerStartingPosition.position;
            player.transform.rotation = Quaternion.identity;
            finishLevelBuilding = true;
        }
        else
        {
            PlaceStartRoom();
            Debug.Log("Place start room");
            yield return interval;

            int iterations = iterationRange[Random.Range(0, iterationRange.Length)];

            for (int i = 0; i < iterations; i++)
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

            finishLevelBuilding = true;
        }
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
        startRoom = Instantiate(startRoomPrefab[Random.Range(0, startRoomPrefab.Length-1)]) as StartRoom;
        startRoom.transform.parent = this.transform;

        AddExitsToList(startRoom, ref availableExits);

        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
    }

    private void PlaceEndRoom()
    {
        endRoom = Instantiate(endRoomPrefab[Random.Range(0, endRoomPrefab.Length - 1)]) as EndRoom;
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

                if (!currentRoom.spwanPoint_Weapon.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spwanPoint_Weapon.Length; i++)
                    {
                        PlaceWeapon(weaponPrefabs[Random.Range(0, weaponPrefabs.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spwanPoint_Food.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spwanPoint_Food.Length; i++)
                    {
                        PlaceFood(foodPrefabs[Random.Range(0, foodPrefabs.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spwanPoint_Enemy.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spwanPoint_Enemy.Length; i++)
                    {
                        PlaceEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Capacity)], currentRoom, i);
                    }
                }
                if (roomPlaced)
                {
                    currentExit.gameObject.SetActive(false);
                    availableExits.Remove(currentExit);

                    availableExit.gameObject.SetActive(false);
                    availableExits.Remove(availableExit);
                }
                break;
            }
        }

        if (roomPlaced == false)
        {
            Destroy(currentRoom.gameObject);
            ResetLevel();
        }
    }

    private void PlaceSpecialRoom()
    {
        specialRoom = Instantiate(specialRoomPrefab[Random.Range(0, startRoomPrefab.Length - 1)]) as SpecialRoom;
        specialRoom.transform.parent = this.transform;

        specialRoom.transform.position = Vector3.zero;
        specialRoom.transform.rotation = Quaternion.identity;
        for (int i = 0; i < specialRoom.spwanPoint_Weapon.Length; i++)
        {
            PlaceWeapon(weaponPrefabs[Random.Range(0, weaponPrefabs.Capacity)], specialRoom, i);
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
        bounds.Expand(-2.15f);

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
        if (specialRoom)
        {
            Destroy(specialRoom.gameObject);
        }
        foreach(Room room in placedRooms)
        {
            Destroy(room.gameObject);
        }
        foreach(Weapon weapon in placedWeapons)
        {
            if(!weapon.Equals(null))Destroy(weapon.gameObject);
        }
        foreach(Food food in placedFoods)
        {
            if(!food.Equals(null)) Destroy(food.gameObject);
        }
        foreach (Enemy enemy in placedEnemys)
        {
            if (!enemy.Equals(null)) Destroy(enemy.gameObject);
        }
        placedWeapons.Clear();
        placedRooms.Clear();
        placedFoods.Clear();
        placedEnemys.Clear();
        availableExits.Clear();
        StartCoroutine("GenerateLevel");
    }

    private void PlaceWeapon(Weapon weapon, Room room, int spwaningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if (weapon.spawningRate >= rand)
        {
            Weapon currentSpawnedWeapon = Instantiate(weapon, room.spwanPoint_Weapon[spwaningPoint]) as Weapon;
            currentSpawnedWeapon.transform.parent = this.transform;
            placedWeapons.Add(currentSpawnedWeapon);
        }
    }

    private void PlaceEnemy(Enemy enemy, Room room, int spawningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if (enemy.getEnemySpwaningRate() >= rand)
        {
            Enemy currentSpawnedEnemy = Instantiate(enemy, room.spwanPoint_Enemy[spawningPoint]) as Enemy;
            currentSpawnedEnemy.transform.parent = this.transform;
            placedEnemys.Add(currentSpawnedEnemy);
        }
    }

    private void PlaceFood(Food food, Room room, int spawningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if (food.spawningRate >= rand)
        {
            Food currentSpawnedFood = Instantiate(food, room.spwanPoint_Food[spawningPoint]) as Food;
            currentSpawnedFood.transform.parent = this.transform;
            placedFoods.Add(currentSpawnedFood);
        }
    }

    private void onClickUpgradButton()
    {
        PlayerStatus.Instance.setHealth(10, "upgradeHealth");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }
}
