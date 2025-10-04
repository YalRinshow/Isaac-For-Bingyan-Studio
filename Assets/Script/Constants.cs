using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static float PLAYER_MOVE_SPEED = 2.0f;

    public static float TEARDROP_LIFETIME = 0.6f;
    public static float TEARDROP_FIRE_RATE = 0.2f;
    public static float TEARDROP_SPEED = 6.0f;
    public static float TEARDROP_DAMAGE = 3.5f;

    public static float BOMB_LIFETIME = 3.0f;
    public static float BOMB_RADIUS = 1.2f;
    public static int BOMB_DAMAGE = 114514;

    public static int SPIKES_DAMAGE_TO_PLAYER = 2;
    public static float SPIKES_DAMAGE_TO_ENEMY = 8.0f;

    public static int ENEMY_COLISION_DAMAGE = 1;
    public static float ENEMY_DEFAULT_HEALTH = 3;

    public static int ROOM_HEIGHT = 9;
    public static int ROOM_WIDTH = 14;
    public static int ROOM_DEFAULT_NUMBER = 10;

    public static string PREFAB_NOT_FOUND = "prefab not found!";

    public static string TAG_PLAYER = "Player";
    public static string TAG_ENEMY = "Enemy";
    public static string TAG_GROUND = "Ground";
    public static string TAG_WALL = "Wall";

    public static string DIRECTION_HORIZONTAL = "Horizontal";
    public static string DIRECTION_VERTICAL = "Vertical";

    public static string GAME_OVER_WIN = "Game Over!"+"\n"+"You Win!";
    public static string GAME_OVER_LOSE = "Game Over!"+"\n"+"You Lose!";
}
