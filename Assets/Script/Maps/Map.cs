using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int height = 9;
    public int width = 14;
    public Transform gridMap;
    public GameObject roomPrefab;
    public GameObject doorPrefab;

    public static Vector3[] roomDistanceDelta = new Vector3[4];
    private class connectInfo
    {
        public int[] connectedRoomNumber = new int[4];
    }
    private static List<connectInfo> roomInfo = new List<connectInfo>();
    private static List<Room> rooms = new List<Room>();
    private class roomGenerateInfo
    {
        public int roomNumber;
        public Vector2 roomPosition;
    }
    static int currentRoomNumber;
    int roomTotal = 10;
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

        GameObject startRoomPrefab = Instantiate(Instance.roomPrefab, Instance.gridMap);
        startRoomPrefab.transform.localPosition = new Vector3(0, 0, 20);
        startRoomPrefab.transform.localRotation = Quaternion.identity;
        Room startRoom = startRoomPrefab.GetComponent<Room>();
        rooms.Add(startRoom);
        startRoom.Initialize();
        startRoom.isEnemyClear = true;
        startRoom.isActivated = true;
        roomInfo.Add(new connectInfo());

        Player.Instance.transform.SetParent(startRoomPrefab.transform, false);
        Player.Instance.transform.localPosition = new Vector3(0, 0, 10);
        Player.Instance.transform.rotation = Quaternion.identity;

        GenerateRoom(0, 2, true);
        currentRoomNumber = 0;
    }
    private static void GenerateRoom(int roomNumber, int roomDir, bool isDoorOpen = false)
    {
        GameObject newRoomPrefab = Instantiate(Instance.roomPrefab, Instance.gridMap);
        newRoomPrefab.transform.localPosition = rooms[roomNumber].transform.localPosition + roomDistanceDelta[roomDir];
        newRoomPrefab.transform.localRotation = Quaternion.identity;
        Room newRoom = newRoomPrefab.GetComponent<Room>();
        rooms.Add(newRoom);
        newRoom.Initialize();

        GameObject newDoorPrefab = Instantiate(Instance.doorPrefab, rooms[roomNumber].transform);
        newDoorPrefab.transform.localPosition = rooms[roomNumber].doorPosition[roomDir];
        newDoorPrefab.transform.localRotation = Quaternion.Euler(0, 0, rooms[roomNumber].doorRotation[roomDir]);
        Door newDoor = newDoorPrefab.GetComponent<Door>();
        newDoor.isOpen = isDoorOpen;
        newDoor.roomDir = roomDir;
        newDoor.roomNumber = roomNumber;

        GameObject newRoomDoorPrefab = Instantiate(Instance.doorPrefab, newRoomPrefab.transform);
        newRoomDoorPrefab.transform.localPosition = newRoom.doorPosition[roomDir ^ 1];
        newRoomDoorPrefab.transform.localRotation = Quaternion.Euler(0, 0, newRoom.doorRotation[roomDir ^ 1]);
        Door newRoomDoor = newRoomDoorPrefab.GetComponent<Door>();
        newRoomDoor.isOpen = isDoorOpen;
        newRoomDoor.roomDir = roomDir ^ 1;
        newRoomDoor.roomNumber = rooms.Count - 1;

        roomInfo.Add(new connectInfo());
        roomInfo[roomNumber].connectedRoomNumber[roomDir] = rooms.Count - 1;
        roomInfo[rooms.Count - 1].connectedRoomNumber[roomDir ^ 1] = roomNumber;
    }
    public static void RoomTransfer(int roomNumber, int roomDir)
    {
        int nextRoom = roomInfo[roomNumber].connectedRoomNumber[roomDir];
        int nextDir = roomDir ^ 1;
        rooms[nextRoom].GenerateEnemisAndGround();
        Player.Instance.transform.SetParent(rooms[nextRoom].transform, false);
        Player.Instance.transform.localPosition = rooms[nextRoom].spawnPosition[nextDir];
        Player.Instance.transform.rotation = Quaternion.identity;
        currentRoomNumber = nextRoom;
    }
    public static bool CurrentRoomEnemyClear()
    {
        return rooms[currentRoomNumber].isEnemyClear;
    }
    public enum direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
