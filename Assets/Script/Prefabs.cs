using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Prefabs : MonoBehaviour
{
    public static GameObject roomPrefab;
    public static GameObject doorPrefab;
    public static GameObject miniRoomPrefab;

    public static GameObject teardropPrefab;
    public static GameObject enemyTeardropPrefab;
    public static GameObject bombPrefab;
    public static GameObject bombDroppedPrefab;
    public static GameObject keyDroppedPrefab;
    public static GameObject heartDroppedPrefab;

    public static GameObject theInnerEyePrefab;
    public static GameObject razorBladePrefab;
    public static GameObject theBookOfSinPrefab;

    public static GameObject spikesPrefab;
    public static GameObject rockPrefab;

    public static GameObject attackFlyPrefab;
    public static GameObject nerveEndingPrefab;
    public static GameObject knightPrefab;
    public static GameObject monstroPrefab;

    private static string path = "Prefabs/";

    private static string ROOM = "Room";
    private static string DOOR = "Door";
    private static string MINIROOM = "MiniRoom";

    private static string TEARDROP = "Teardrop";
    private static string ENEMYTEARDROP = "EnemyTeardrop";
    private static string BOMB = "Bomb";
    private static string BOMBDROPPED = "BombDropped";
    private static string KEYDROPPED = "KeyDropped";
    private static string HEARTDROPPED = "HeartDropped";

    private static string THEINNEREYE = "TheInnerEye";
    private static string RAZORBLADE = "RazorBlade";
    private static string THEBOOKOFSIN = "TheBookOfSin";

    private static string SPIKES = "Spikes";
    private static string ROCK = "Rock";
    private static string ATTACKFLY = "AttackFly";
    private static string NERVEENDING = "NerveEnding";
    private static string KNIGHT = "Knight";
    private static string MONSTRO = "Monstro";

    public static void LoadPrefabs()
    {
        roomPrefab = Resources.Load<GameObject>(path + ROOM);
        doorPrefab = Resources.Load<GameObject>(path + DOOR);
        miniRoomPrefab = Resources.Load<GameObject>(path + MINIROOM);

        teardropPrefab = Resources.Load<GameObject>(path + TEARDROP);
        enemyTeardropPrefab = Resources.Load<GameObject>(path + ENEMYTEARDROP);

        bombPrefab = Resources.Load<GameObject>(path + BOMB);
        bombDroppedPrefab = Resources.Load<GameObject>(path + BOMBDROPPED);
        keyDroppedPrefab = Resources.Load< GameObject>(path + KEYDROPPED);
        heartDroppedPrefab = Resources.Load<GameObject>(path+ HEARTDROPPED);

        theInnerEyePrefab = Resources.Load<GameObject>(path + THEINNEREYE);
        razorBladePrefab = Resources.Load<GameObject>(path + RAZORBLADE);
        theBookOfSinPrefab = Resources.Load<GameObject>(path + THEBOOKOFSIN);

        spikesPrefab = Resources.Load<GameObject>(path + SPIKES);
        rockPrefab = Resources.Load<GameObject>(path + ROCK);

        attackFlyPrefab = Resources.Load<GameObject>(path + ATTACKFLY);
        nerveEndingPrefab = Resources.Load<GameObject>(path + NERVEENDING);
        knightPrefab = Resources.Load<GameObject>(path + KNIGHT);
        monstroPrefab = Resources.Load<GameObject>(path + MONSTRO);
    }
}
