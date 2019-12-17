using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    //The user interface components
    public GameObject upgradeUI, pauseUI, loadingUI, gameOverUI, ObjectiveText;

    [SerializeField]
    private Button Gift_slot_1, Gift_slot_2, Gift_slot_3, Gift_slot_4, Gift_slot_5, Gift_slot_6;

    [SerializeField]
    private GameObject Gift_1, Gift_2, Gift_3, Gift_4, Gift_5, Gift_6;

    //The needed Prefabs
    public Room[] startRoomPrefab, endRoomPrefab, specialRoomPrefab;

    //The List of prefabs
    public List<Room> roomPrefabs = new List<Room>();
    public List<Weapon> weaponPrefab = new List<Weapon>();
    public List<Food> foodPrefabs = new List<Food>();
    public List<Enemy> enemyPrefab = new List<Enemy>();
    public List<Coins> coinsPrefab = new List<Coins>();
    public List<Books> booksPrefab = new List<Books>();
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
    private List<Books> placedBooks = new List<Books>();
    private List<VendingMachine> placedVendingMachine = new List<VendingMachine>();

    //For the Room mesh layer
    private LayerMask roomLayerMask;

    //Spawning the player in the right place
    private GameObject player;

    private bool finishLevelBuilding, generationOnce;

    //Player Hunger
    private float timer, hungerTime;

    //checking is sanity is drop
    private int sanityStage;

    private void Start()
    {
        //The default game status
        finishLevelBuilding = false;
        loadingUI.SetActive(false);
        upgradeUI.SetActive(false);
        pauseUI.SetActive(false);
        ObjectiveText.SetActive(false);
        Gift_1.SetActive(false);
        Gift_2.SetActive(false);
        Gift_3.SetActive(false);
        Gift_4.SetActive(false);
        Gift_5.SetActive(false);
        Gift_6.SetActive(false);
        generationOnce = false;
        sanityStage = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        roomLayerMask = LayerMask.GetMask("Room");

        //Starting to Generate the game level
        StartCoroutine("GenerateLevel");

        //Button listener
        Gift_slot_1.onClick.AddListener(onClickUpgradeHealingButton);
        Gift_slot_2.onClick.AddListener(onClickUpgradeHealthButton);
        Gift_slot_3.onClick.AddListener(onClickUpgradeSpeedButton);
        Gift_slot_4.onClick.AddListener(onClickUpgradeHungerButton);
        Gift_slot_5.onClick.AddListener(onClickRestoringToFullHealth);
        Gift_slot_6.onClick.AddListener(onClickRestoringToFullHunger);
    }

    private void Update()
    {
        if (PlayerStatus.Instance.getSanity() <= 35 && PlayerStatus.Instance.getSanity() > 20 && sanityStage == 0)
        {
            PlayerStatus.Instance.setHunger(-25, "Total Hunger");
            sanityStage = 1;
        }
        else if (PlayerStatus.Instance.getSanity() < 20 && PlayerStatus.Instance.getSanity() > 0 && sanityStage == 1)
        {
            PlayerStatus.Instance.setHunger(-25, "Total Hunger");
            PlayerStatus.Instance.setSpeed(-10, "");
            sanityStage = 2;
        }
        else if (PlayerStatus.Instance.getSanity() <= 0 && sanityStage == 2)
        {
            PlayerStatus.Instance.setSpeed(0, "set2zero");
            PlayerStatus.Instance.setPlayerKilledBy("You have Collapse");
        }

        if (finishLevelBuilding)
        {
            ObjectiveText.SetActive(true);
        }
        if (upgradeUI.activeSelf)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(12);
            ResetLevel();
        }
        if (!finishLevelBuilding)
        {
            loadingUI.SetActive(true);
            if(!generationOnce) RandomPickGift();
            AudioListener.volume = 0f;
        }
        else
        {
            loadingUI.SetActive(false);
            generationOnce = false;
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
            PlayerStatus.Instance.setPlayerKilledBy("Starved to death");
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
        if (timer > 0.5f)
        {
            hungerTime+=0.5f;
            timer = 0f;
        }
        if(hungerTime >= PlayerStatus.Instance.getHungerTime() && finishLevelBuilding && PlayerStatus.Instance.getHunger() > 0 && !PlayerStatus.Instance.getPlayerInTheSafeHouse())
        {
            PlayerStatus.Instance.setHunger(-1, "");
            hungerTime = 0;
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
            PlayerStatus.Instance.setPlayerInTheSafeHouse(true);
            PlaceSpecialRoom();
            Debug.Log("Place a special room");
            player.transform.position = specialRoom.playerStartingPosition.position;
            player.transform.rotation = Quaternion.identity;
            finishLevelBuilding = true;
        }
        else
        {
            PlayerStatus.Instance.setPlayerInTheSafeHouse(false);
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

            //restore litte bit of hunger
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().popDescriptionText("+ Hunger");
            PlayerStatus.Instance.setHunger(15, "");

            PlayerStatus.Instance.setCoins(+PlayerStatus.Instance.getRandomStartUpMoney());
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
        int index = 0;
        //if (PlayerStatus.Instance.getPlayerReachLevels() < 10) index = 0;
        //else if (PlayerStatus.Instance.getPlayerReachLevels() >= 10 && PlayerStatus.Instance.getPlayerReachLevels() < 20) index = 0;
        startRoom = Instantiate(startRoomPrefab[index]) as StartRoom;
        startRoom.transform.parent = this.transform;

        AddExitsToList(startRoom, ref availableExits);

        if (!startRoom.startUpWeapon.Equals(null) && PlayerStatus.Instance.getSelectBetterWeapon() == 1)
        {
            Weapon currentSpawnedWeapon = Instantiate(weaponPrefab[1], startRoom.startUpWeapon) as Weapon;
            currentSpawnedWeapon.transform.parent = this.transform;
            placedWeapons.Add(currentSpawnedWeapon);
        }
        else
        {
            Weapon currentSpawnedWeapon = Instantiate(weaponPrefab[PlayerStatus.Instance.getSelectBetterWeapon()], startRoom.startUpWeapon) as Weapon;
            currentSpawnedWeapon.transform.parent = this.transform;
            placedWeapons.Add(currentSpawnedWeapon);
        }

        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
    }

    private void PlaceEndRoom()
    {
        int index = 0;
        //if (PlayerStatus.Instance.getPlayerReachLevels() < 10) index = 0;
        //else if (PlayerStatus.Instance.getPlayerReachLevels() >= 10 && PlayerStatus.Instance.getPlayerReachLevels() < 20) index = 0;
        endRoom = Instantiate(endRoomPrefab[Random.Range(0, endRoomPrefab.Length)]) as EndRoom;
        endRoom.transform.parent = this.transform;

        List<Exit> allAvailabeExits = new List<Exit>(availableExits);
        Exit exit = endRoom.exits[Random.Range(0,1)];

        bool roomPlaced = false;

        foreach (Exit availableExit in allAvailabeExits)
        {
            Room room = (Room)endRoom;
            PositionRoomAtExit(ref room, exit, availableExit);

            if (CheckRoomOverlap(endRoom))
            {
                Destroy(endRoom.gameObject);
                ResetLevel();
            }

            if (!endRoom.spawnPoint_Weapon.Equals(null))
            {
                for (int i = 0; i < endRoom.spawnPoint_Weapon.Length; i++)
                {
                    PlaceWeapon(weaponPrefab[Random.Range(0, weaponPrefab.Capacity)], endRoom, i);
                }
            }
            if (!endRoom.spawnPoint_Books.Equals(null))
            {
                for (int i = 0; i < endRoom.spawnPoint_Books.Length; i++)
                {
                    PlaceBooks(booksPrefab[Random.Range(0, booksPrefab.Capacity)], endRoom, i);
                }
            }

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
        int minRange = 0, maxRange = 0;
        if(PlayerStatus.Instance.getPlayerReachLevels() < 10)
        {
            maxRange = 15;
            //minRange = 13;
            //maxRange = 23;
        }
        else if(PlayerStatus.Instance.getPlayerReachLevels() >= 10 && PlayerStatus.Instance.getPlayerReachLevels() < 20)
        {
            maxRange = 15;
            //minRange = 13;
            //maxRange = 23;
        }
        Room currentRoom = Instantiate(roomPrefabs[Random.Range(minRange, maxRange)]) as Room;
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

                if (!currentRoom.spawnPoint_Weapon.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spawnPoint_Weapon.Length; i++)
                    {
                        PlaceWeapon(weaponPrefab[Random.Range(0, weaponPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_Coins.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spawnPoint_Coins.Length; i++)
                    {
                        PlaceCoins(coinsPrefab[Random.Range(0, coinsPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_Books.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spawnPoint_Books.Length; i++)
                    {
                        PlaceBooks(booksPrefab[Random.Range(0, booksPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_Food.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spawnPoint_Food.Length; i++)
                    {
                        PlaceFood(foodPrefabs[Random.Range(0, foodPrefabs.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_Enemy.Equals(null))
                {
                    for(int i = 0; i < currentRoom.spawnPoint_Enemy.Length; i++)
                    {
                        PlaceEnemy(enemyPrefab[Random.Range(0, enemyPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_Coins.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spawnPoint_Coins.Length; i++)
                    {
                        PlaceCoins(coinsPrefab[Random.Range(0, coinsPrefab.Capacity)], currentRoom, i);
                    }
                }
                if (!currentRoom.spawnPoint_VendingMachines.Equals(null))
                {
                    for (int i = 0; i < currentRoom.spawnPoint_VendingMachines.Length; i++)
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
        specialRoom = Instantiate(specialRoomPrefab[Random.Range(0, specialRoomPrefab.Length)]) as SpecialRoom;
        specialRoom.transform.parent = this.transform;

        specialRoom.transform.position = Vector3.zero;
        specialRoom.transform.rotation = Quaternion.identity;
        for (int i = 0; i < specialRoom.spawnPoint_Weapon.Length; i++)
        {
            PlaceWeapon(weaponPrefab[Random.Range(0, weaponPrefab.Capacity)], specialRoom, i);
        }
        for(int i = 0; i < specialRoom.spawnPoint_Coins.Length; i++)
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
        bounds.Expand(-0.1f);

        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size/2, room.transform.rotation, roomLayerMask);
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
        foreach (Books books in placedBooks)
        {
            if (!books.Equals(null)) Destroy(books.gameObject);
        }
        placedWeapons.Clear();
        placedRooms.Clear();
        placedFoods.Clear();
        placedEnemys.Clear();
        placedCoins.Clear();
        placedVendingMachine.Clear();
        placedBooks.Clear();
        availableExits.Clear();
        StartCoroutine("GenerateLevel");
    }

    //Placing interactable items or enemy
    private void PlaceBooks(Books books, Room room , int spawningPoint)
    {
        Books currentSpawnedBooks = Instantiate(books, room.spawnPoint_Books[spawningPoint]) as Books;
        currentSpawnedBooks.transform.parent = this.transform;
        placedBooks.Add(currentSpawnedBooks);
    }

    private void PlaceWeapon(Weapon weapon, Room room, int spawningPoint)
    {
        Weapon currentSpawnedWeapon = Instantiate(weapon, room.spawnPoint_Weapon[spawningPoint]) as Weapon;
        currentSpawnedWeapon.transform.parent = this.transform;
        placedWeapons.Add(currentSpawnedWeapon);
    }

    private void PlaceEnemy(Enemy enemy, Room room, int spawningPoint)
    {
        Enemy currentSpawnedEnemy = Instantiate(enemy, room.spawnPoint_Enemy[spawningPoint]) as Enemy;
        currentSpawnedEnemy.transform.parent = this.transform;
        placedEnemys.Add(currentSpawnedEnemy);
    }

    private void PlaceFood(Food food, Room room, int spawningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if (food.spawningRate >= rand)
        {
            Food currentSpawnedFood = Instantiate(food, room.spawnPoint_Food[spawningPoint]) as Food;
            currentSpawnedFood.transform.parent = this.transform;
            placedFoods.Add(currentSpawnedFood);
        }
    }

    private void PlaceCoins(Coins coins, Room room, int spawningPoint)
    {
        float rand = Random.Range(0f, 1f);
        if(room is SpecialRoom)
        {
            Coins currentSpawnedCoins = Instantiate(coins, room.spawnPoint_Coins[spawningPoint]) as Coins;
            currentSpawnedCoins.transform.parent = this.transform;
            placedCoins.Add(currentSpawnedCoins);
        }
        else if (coins.spawningRate >= rand)
        {
            Coins currentSpawnedCoins = Instantiate(coins, room.spawnPoint_Coins[spawningPoint]) as Coins;
            currentSpawnedCoins.transform.parent = this.transform;
            placedCoins.Add(currentSpawnedCoins);
        }
        
    }

    private void PlaceVendingMachine(VendingMachine vendingMachine, Room room, int spawningPoint)
    {
        VendingMachine currentSpawnedVendingMachine = Instantiate(vendingMachine, room.spawnPoint_VendingMachines[spawningPoint]) as VendingMachine;
        currentSpawnedVendingMachine.transform.parent = this.transform;
        placedVendingMachine.Add(currentSpawnedVendingMachine);
    }

    //Button
    private void onClickUpgradeHungerButton()
    {
        PlayerStatus.Instance.setHunger(10, "Total Hunger");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickUpgradeHealthButton()
    {
        PlayerStatus.Instance.setHealth(1, "Total Health");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickUpgradeSpeedButton()
    {
        PlayerStatus.Instance.setSpeed(5, "upgradeSpeed");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickUpgradeHealingButton()
    {
        if (!PlayerStatus.Instance.getActiveHealingSkill()) PlayerStatus.Instance.setActiveHealingSkill(true);
        else PlayerStatus.Instance.setHealingTime(1);
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickRestoringToFullHealth()
    {
        PlayerStatus.Instance.setHealth(PlayerStatus.Instance.getTotalHealth(), "");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void onClickRestoringToFullHunger()
    {
        PlayerStatus.Instance.setHunger(PlayerStatus.Instance.getTotalHunger(), "");
        PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
    }

    private void RandomPickGift()
    {
        generationOnce = true;
        int rand = Random.Range(0, 6);
        switch(rand)
        {
            case 1:
                Gift_1.SetActive(true);
                Gift_2.SetActive(true);
                Gift_3.SetActive(true);
                
                Gift_4.SetActive(false);
                Gift_5.SetActive(false);
                Gift_6.SetActive(false);
                break;
            case 2:
                Gift_4.SetActive(true);
                Gift_2.SetActive(true);
                Gift_3.SetActive(true);

                Gift_1.SetActive(false);
                Gift_5.SetActive(false);
                Gift_6.SetActive(false);
                break;
            case 3:
                Gift_1.SetActive(true);
                Gift_5.SetActive(true);
                Gift_3.SetActive(true);

                Gift_2.SetActive(false);
                Gift_4.SetActive(false);
                Gift_6.SetActive(false);
                break;
            case 4:
                Gift_1.SetActive(true);
                Gift_2.SetActive(true);
                Gift_6.SetActive(true);

                Gift_3.SetActive(false);
                Gift_4.SetActive(false);
                Gift_5.SetActive(false);
                break;
            case 5:
                Gift_4.SetActive(true);
                Gift_5.SetActive(true);
                Gift_6.SetActive(true);

                Gift_1.SetActive(false);
                Gift_2.SetActive(false);
                Gift_3.SetActive(false);
                break;
            case 6:
                Gift_4.SetActive(true);
                Gift_5.SetActive(true);
                Gift_3.SetActive(true);

                Gift_1.SetActive(false);
                Gift_2.SetActive(false);
                Gift_6.SetActive(false);
                break;
        }
    }
}
