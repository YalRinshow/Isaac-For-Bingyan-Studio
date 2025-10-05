using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public int height = Constants.ROOM_HEIGHT;
    public int width = Constants.ROOM_WIDTH;
    public Transform gridMap;
    public Sprite bossRoomDoor;

    public static Vector3[] roomDistanceDelta = new Vector3[4];
    private class connectInfo
    {
        public int[] connectedRoomNumber = new int[4];
        public Door[] doors = new Door[4];
    }
    private static List<connectInfo> roomInfo = new List<connectInfo>();
    public static List<Room> rooms = new List<Room>();
    private class roomGenerateInfo
    {
        public int roomNumber;
        public Vector2 roomPosition;
    }
    public static int currentRoomNumber;
    int roomTotal = 0;
    public struct point
    {
        public int x, y;
        public static point operator +(point a, point b)
        {
            return new point() { x = a.x + b.x, y = a.y + b.y };
        }
    }
    static int[] dirWeight = new int[4];
    static point[] dir = new point[4] { 
        new point() { x = 0, y = 1 }, 
        new point() { x = 0, y = -1 }, 
        new point() { x = -1, y = 0 }, 
        new point() { x = 1, y = 0 } 
    };
    public static Map Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Initialize()
    {
        GenerateMap();
    }
    private void GenerateMap()
    {
        roomDistanceDelta[0] = new Vector3(0, height * 2, 0);
        roomDistanceDelta[1] = new Vector3(0, 0 - height * 2, 0);
        roomDistanceDelta[2] = new Vector3(0 - width * 2, 0, 0);
        roomDistanceDelta[3] = new Vector3(width * 2, 0, 0);

        GameObject startRoomPrefab = Instantiate(Prefabs.roomPrefab, Instance.gridMap);
        startRoomPrefab.transform.localPosition = new Vector3(0, 0, 20);
        startRoomPrefab.transform.localRotation = Quaternion.identity;
        Room startRoom = startRoomPrefab.GetComponent<Room>();
        rooms.Add(startRoom);
        startRoom.Initialize(true, true);
        roomInfo.Add(new connectInfo());

        Player.Instance.transform.SetParent(startRoomPrefab.transform, false);
        Player.Instance.transform.localPosition = new Vector3(0, 0, 10);
        Player.Instance.transform.rotation = Quaternion.identity;

        GetDirWeight();
        Queue<point> roomPoints = new Queue<point>();
        Dictionary<point, int> visPoints = new Dictionary<point, int>();
        Dictionary<int, point> roomCoordinate = new Dictionary<int, point>();
        roomPoints.Enqueue(new point() { x = 0, y = 0 });
        visPoints.Add(new point() { x = 0, y = 0 }, 0);
        while (roomPoints.Count > 0)
        {
            point currentPoint = roomPoints.Dequeue();
            roomCoordinate.Add(visPoints[currentPoint], currentPoint);
            int connectRooms = (currentPoint.x == 0 && currentPoint.y == 0 ? Random.Range(1, 5) : Random.Range(0, 5));
            int currentRoom = visPoints[currentPoint];
            for (int i = 0; i < 20 && connectRooms > 0 && roomTotal < Constants.ROOM_DEFAULT_NUMBER; i++)
            {
                int randDir = GetRandomDir();
                point nextPoint = currentPoint + dir[randDir];
                if (visPoints.ContainsKey(nextPoint)) continue;
                connectRooms--;
                GenerateRoom(currentRoom, randDir, currentRoom == 0);
                roomTotal++;
                visPoints.Add(nextPoint, rooms.Count - 1);
                roomPoints.Enqueue(nextPoint);
            }
        }
        bool isBossRoomGenerated = false;
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            point currentPoint = roomCoordinate[i];
            int currentRoom = visPoints[currentPoint];
            for (int j = 0; j < 20 && !isBossRoomGenerated; j++)
            {
                int randDir = GetRandomDir();
                point nextPoint = currentPoint + dir[randDir];
                if (visPoints.ContainsKey(nextPoint)) continue;
                GenerateRoom(currentRoom, randDir, false, true);
                isBossRoomGenerated = true;
                break;
            }
        }
        currentRoomNumber = 0;
    }
    private static void GetDirWeight()
    {
        int bigDir = Random.Range(0, 4);
        dirWeight[bigDir] = Random.Range(40, 70);
        int resWeight = 100 - dirWeight[bigDir];
        for (int i = 0; i < 4; i++)
        {
            if (i == bigDir) continue;
            dirWeight[i] = Random.Range(0, resWeight + 1);
            resWeight -= dirWeight[i];
        }
        if (resWeight > 0)
        {
            dirWeight[3] += resWeight;
            resWeight = 0;
        }
        for (int i = 1; i < 4; i++)
        {
            dirWeight[i] += dirWeight[i - 1];
        }
    }
    private static int GetRandomDir()
    {
        int rand = Random.Range(0, 100);
        for (int i = 0; i < 4; i++)
        {
            if (rand < dirWeight[i]) return i;
        }
        return Random.Range(0, 4);
    }
    private static void GenerateRoom(int roomNumber, int roomDir, bool isDoorOpen = false, bool isToBossRoom = false)
    {
        GameObject newRoomPrefab = Instantiate(Prefabs.roomPrefab, Instance.gridMap);
        newRoomPrefab.transform.localPosition = rooms[roomNumber].transform.localPosition + roomDistanceDelta[roomDir];
        newRoomPrefab.transform.localRotation = Quaternion.identity;
        Room newRoom = newRoomPrefab.GetComponent<Room>();
        rooms.Add(newRoom);
        newRoom.Initialize(false, false);

        GameObject newDoorPrefab = Instantiate(Prefabs.doorPrefab, rooms[roomNumber].transform);
        newDoorPrefab.transform.localPosition = rooms[roomNumber].doorPosition[roomDir];
        newDoorPrefab.transform.localRotation = Quaternion.Euler(0, 0, rooms[roomNumber].doorRotation[roomDir]);
        Door newDoor = newDoorPrefab.GetComponent<Door>();
        newDoor.isOpen = isDoorOpen;
        newDoor.roomDir = roomDir;
        newDoor.roomNumber = roomNumber;

        GameObject newRoomDoorPrefab = Instantiate(Prefabs.doorPrefab, newRoomPrefab.transform);
        newRoomDoorPrefab.transform.localPosition = newRoom.doorPosition[roomDir ^ 1];
        newRoomDoorPrefab.transform.localRotation = Quaternion.Euler(0, 0, newRoom.doorRotation[roomDir ^ 1]);
        Door newRoomDoor = newRoomDoorPrefab.GetComponent<Door>();
        newRoomDoor.isOpen = isDoorOpen;
        newRoomDoor.roomDir = roomDir ^ 1;
        newRoomDoor.roomNumber = rooms.Count - 1;

        if (isToBossRoom)
        {
            SpriteRenderer newDoorImage = newDoorPrefab.GetComponent<SpriteRenderer>();
            SpriteRenderer newRoomDoorImage = newRoomDoorPrefab.GetComponent<SpriteRenderer>();
            newDoorImage.sprite = Instance.bossRoomDoor;
            newRoomDoorImage.sprite = Instance.bossRoomDoor;
        }

        roomInfo.Add(new connectInfo());
        roomInfo[roomNumber].connectedRoomNumber[roomDir] = rooms.Count - 1;
        roomInfo[roomNumber].doors[roomDir] = newDoor;
        roomInfo[rooms.Count - 1].connectedRoomNumber[roomDir ^ 1] = roomNumber;
        roomInfo[rooms.Count - 1].doors[roomDir ^ 1] = newRoomDoor;
    }
    public static void RoomTransfer(int roomNumber, int roomDir)
    {
        int nextRoom = roomInfo[roomNumber].connectedRoomNumber[roomDir];
        int nextDir = roomDir ^ 1;
        currentRoomNumber = nextRoom;
        rooms[nextRoom].GenerateEnemisAndGround();
        Door nextRoomDoor = roomInfo[nextRoom].doors[nextDir];
        nextRoomDoor.isOpen = true;
        Player.Instance.transform.SetParent(rooms[nextRoom].transform, false);
        Player.Instance.transform.localPosition = rooms[nextRoom].spawnPosition[nextDir];
        Player.Instance.transform.rotation = Quaternion.identity;
        Player.Instance.StopTeardropEffect();
        if (currentRoomNumber == rooms.Count - 1)
        {
            GameManager.Instance.PlayBossMusic();
        }
    }
    public static bool CurrentRoomEnemyClear()
    {
        return rooms[currentRoomNumber].EnemyClear();
    }
    public enum direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
