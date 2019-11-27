﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    //The user interface components
    public GameObject upgradeUI, pauseUI, loadingUI, gameOverUI;
    [SerializeField]
    private Button Upgrade_slot_1, Upgrade_slot_2, Upgrade_slot_3;

    //The needed Prefabs
    public Room[] startRoomPrefab, endRoomPrefab, specialRoomPrefab;

    //The List of prefabs
    public List<Room> roomPrefabs = new List<Room>();
    public List<Weapon> weaponPrefabs = new List<Weapon>();
    public List<Food> foodPrefabs = new List<Food>();
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public List<Coins> coinsPrefab = new List<Coins>();
    public VendingMachine vendingMachinePrefab;

    //The array control total rooms in game
    private int[] iterationRange = {6,7,8,9};

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
    private List<Coins> placedCoins = new List<Coins>();
    private List<VendingMachine> placedVendingMachine = new List<VendingMachine>();

    //For the Room mesh layer
    private LayerMask roomLayerMask;

    //Spawning the player in the right place
    private GameObject player;

    private bool finishLevelBuilding;

    //Player Hunger
    private float timer;

    private void Start()
    {
        //The default game status
        finishLevelBuilding = false;
        loadingUI.SetActive(false);
        upgradeUI.SetActive(false);
        pauseUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        roomLayerMask = LayerMask.GetMask("Room");

        //Starting to Generate the game level
        StartCoroutine("GenerateLevel");

        //Button listener
        Upgrade_slot_1.onClick.AddListener(onClickUpgradeHealthButton);
        Upgrade_slot_2.onClick.AddListener(onClickUpgradeHealthButton);
        Upgrade_slot_3.onClick.AddListener(onClickUpgradeSpeedButton);
    }

    private void Update()
    {        
        if (upgradeUI.activeSelf)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(12);
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

        //For Death reason
        if (PlayerStatus.Instance.getHunger() == 0)
        {
            PlayerStatus.Instance.setPlayerKilledBy("Starve to death");
        }

        //The excpetion of Player die at the 0 level
        else if (PlayerStatus.Instance.getPlayerReachLevels() == 0 && PlayerStatus.Instance.getHealth() == 0 || PlayerStatus.Instance.getHunger() == 0)
        {
            PlayerStatus.Instance.setPlayerKilledBy("Do you really trying");
        }

        if (PlayerStatus.Instance.getHealth() == 0 || PlayerStatus.Instance.getHunger() == 0)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            gameOverUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseUI.activeSelf)
        {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pauseUI.activeSelf)
        {
            Time.timeScale = 1;
        }

        pauseUI.SetActive(Time.timeScale == 0 && !gameOverUI.activeSelf);

        timer += Time.deltaTime;
        if (timer > 1f && finishLevelBuilding && PlayerStatus.Instance.getHunger() > 0)
        {
            PlayerStatus.Instance.setHunger(-1, "");
            timer = 0f;
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

    //Placing different rooms
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
                if (!currentRoom.spwanPoint_Coins.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spwanPoint_Coins.Length; i++)
                    {
                        PlaceCoins(coinsPrefab[Random.Range(0, coinsPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spwanPoint_VendingMachines.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spwanPoint_VendingMachines.Length; i++)
                    {
                        PlaceVendingMachine(vendingMachinePrefab, currentRoom, i);
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
        for(int i = 0; i < specialRoom.spwanPoint_Coins.Length; i++)
        {
            PlaceCoins(coinsPrefab[1], specialRoom, i);
        }
    }

    //Position the room at to the target exit
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

    //Check the room is overlap or not
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

    //Reset the level
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
        foreach(Coins coins in placedCoins)
        {
            if (!coins.Equals(null)) Destroy(coins.gameObject);
        }
        foreach (VendingMachine vendingMachine in placedVendingMachine)
        {
            if (!vendingMachine.Equals(null)) Destroy(vendingMachine.gameObject);
        }
        placedWeapons.Clear();
        placedRooms.Clear();
        placedFoods.Clear();
        placedEnemys.Clear();
        placedCoins.Clear();
        placedVendingMachine.Clear();
        availableExits.Clear();
        StartCoroutine("GenerateLevel");
    }

    //Placing interactable items or enemy
    private void PlaceWeapon(Weapon weapon, Room room, int spwaningPoint)
    {
            Weapon currentSpawnedWeapon = Instantiate(weapon, room.spwanPoint_Weapon[spwaningPoint]) as Weapon;
            currentSpawnedWeapon.transform.parent = this.transform;
            placedWeapons.Add(currentSpawnedWeapon);
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

    private void PlaceCoins(Coins coins, Room room, int spawningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if(room is SpecialRoom)
        {
            Coins currentSpawnedCoins = Instantiate(coins, room.spwanPoint_Coins[spawningPoint]) as Coins;
            currentSpawnedCoins.transform.parent = this.transform;
            placedCoins.Add(currentSpawnedCoins);
        }
        else if (coins.spawningRate >= rand)
        {
            Coins currentSpawnedCoins = Instantiate(coins, room.spwanPoint_Coins[spawningPoint]) as Coins;
            currentSpawnedCoins.transform.parent = this.transform;
            placedCoins.Add(currentSpawnedCoins);
        }
        
    }

    private void PlaceVendingMachine(VendingMachine vendingMachine, Room room, int spawningPoint)
    {
        VendingMachine currentSpawnedVendingMachine = Instantiate(vendingMachine, room.spwanPoint_VendingMachines[spawningPoint]) as VendingMachine;
        currentSpawnedVendingMachine.transform.parent = this.transform;
        placedVendingMachine.Add(currentSpawnedVendingMachine);
    }

    //Button
    private void onClickUpgradeHealthButton()
    {
        PlayerStatus.Instance.setHealth(2, "upgradeHealth");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickUpgradeSpeedButton()
    {
        PlayerStatus.Instance.setSpeed(5, "upgradeSpeed");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickUpgradeHealingButton()
    {

    }

    private void onClickRestoringToFullHealth()
    {
        PlayerStatus.Instance.setHealth(100, "");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }
}
