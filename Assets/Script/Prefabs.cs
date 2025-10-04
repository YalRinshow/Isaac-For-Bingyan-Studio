using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static GameObject roomPrefab;
    public static GameObject doorPrefab;

    public static GameObject teardropPrefab;
    public static GameObject bombPrefab;
    public static GameObject bombDroppedPrefab;

    public static GameObject spikesPrefab;
    public static GameObject rockPrefab;

    public static GameObject attackFlyPrefab;
    public static GameObject nerveEndingPrefab;
    public static GameObject knightPrefab;

    private static string path = "Prefabs/";

    private static string ROOM = "Room";
    private static string DOOR = "Door";
    private static string TEARDROP = "Teardrop";
    private static string BOMB = "Bomb";
    private static string BOMBDROPPED = "BombDropped";
    private static string SPIKES = "Spikes";
    private static string ROCK = "Rock";
    private static string ATTACKFLY = "AttackFly";
    private static string NERVEENDING = "NerveEnding";
    private static string KNIGHT = "Knight";

    public static void LoadPrefabs()
    {
        roomPrefab = Resources.Load<GameObject>(path + ROOM);
        doorPrefab = Resources.Load<GameObject>(path + DOOR);
        teardropPrefab = Resources.Load<GameObject>(path + TEARDROP);
        bombPrefab = Resources.Load<GameObject>(path + BOMB);
        bombDroppedPrefab = Resources.Load<GameObject>(path + BOMBDROPPED);
        spikesPrefab = Resources.Load<GameObject>(path + SPIKES);
        rockPrefab = Resources.Load<GameObject>(path + ROCK);
        attackFlyPrefab = Resources.Load<GameObject>(path + ATTACKFLY);
        nerveEndingPrefab = Resources.Load<GameObject>(path + NERVEENDING);
        knightPrefab = Resources.Load<GameObject>(path + KNIGHT);
    }
}
