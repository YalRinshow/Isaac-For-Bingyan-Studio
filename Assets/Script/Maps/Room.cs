using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Room : MonoBehaviour
{
    public Vector2[] spawnPosition = new Vector2[4];
    public Vector2[] doorPosition = new Vector2[4];
    public float[] doorRotation = new float[4];
    public bool isEnemyClear = false;
    public bool isActivated = false;
    private List<Vector2> availablePositions = new List<Vector2>();
    private List<bool> usedPosition = new List<bool>();
    public void Initialize(bool enemyClear = false, bool activate = false)
    {
        isEnemyClear = enemyClear;
        isActivated = activate;
        doorPosition[0] = new Vector2(0, 3.4f);
        doorRotation[0] = 0.0f;
        doorPosition[1] = new Vector2(0, -4.4f);
        doorRotation[1] = 180.0f;
        doorPosition[2] = new Vector2(-6.4f, 0);
        doorRotation[2] = 90.0f;
        doorPosition[3] = new Vector2(6.4f, 0);
        doorRotation[3] = -90.0f;
        spawnPosition[0] = new Vector2(0, 2.2f);
        spawnPosition[1] = new Vector2(0, -3.0f);
        spawnPosition[2] = new Vector2(-5.0f, 0);
        spawnPosition[3] = new Vector2(5.0f, 0);
    }
    private void FixedUpdate()
    {
        if (!isActivated) return;
        if (isEnemyClear) return;
        if (EnemyClear())
        {
            isEnemyClear = true;
            UIManager.Instance.UpdateEnergy(1);
        }
    }
    public bool EnemyClear()
    {
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        if (enemies.Length > 0) return false;
        return true;
    }
    private void Shuffle()
    {
        int size = availablePositions.Count;
        for (int i = 0; i < size; i++)
        {
            int transPos = Random.Range(i, size);
            Vector2 t = availablePositions[i];
            availablePositions[i] = availablePositions[transPos];
            availablePositions[transPos] = t;
        }
    }
    public void GenerateEnemisAndGround()
    {
        if (isActivated) return;
        for (int x = -4; x <= 3; x++)
        {
            for (int y = -2; y <= 0; y++)
            {
                availablePositions.Add(new Vector2(x + 0.5f, y + 0.5f));
                usedPosition.Add(false);
            }
        }
        availablePositions.Add(new Vector2(-3.5f, 1.5f));
        availablePositions.Add(new Vector2(-3.5f, -2.5f));
        availablePositions.Add(new Vector2(3.5f, 1.5f));
        availablePositions.Add(new Vector2(3.5f, -2.5f));
        availablePositions.Add(new Vector2(-2.5f, 1.5f));
        availablePositions.Add(new Vector2(-2.5f, -2.5f));
        availablePositions.Add(new Vector2(2.5f, 1.5f));
        availablePositions.Add(new Vector2(2.5f, -2.5f));
        for (int i = 0; i < 8; i++) usedPosition.Add(false);
        Shuffle();
        isActivated = true;
        int size = availablePositions.Count;
        int randEnemis = Random.Range(1, 6);
        int randGrounds = Random.Range(0, 10);
        for (int i = 0; i < size && randEnemis > 0; i++)
        {
            if (usedPosition[i]) continue;
            GenerateEnemy(availablePositions[i]);
            randEnemis--;
            usedPosition[i] = true;
        }
        for (int i = 0; i < size && randGrounds > 0; i++)
        {
            if (usedPosition[i]) continue;
            if (!AvailabeForGround(availablePositions[i])) continue;
            GenerateGround(availablePositions[i]);
            randGrounds--;
            usedPosition[i] = true;
        }
    }
    private void GenerateEnemy(Vector2 position)
    {
        int randEnemy = Random.Range(0, 7);
        if (randEnemy == 0)
        {
            GenerateObject(Prefabs.knightPrefab, position, true);
        }
        else if (randEnemy % 2 == 1)
        {
            GenerateObject(Prefabs.attackFlyPrefab, position, true);
        }
        else
        {
            GenerateObject(Prefabs.nerveEndingPrefab, position, true);
        }
        
    }
    private void GenerateGround(Vector2 position)
    {
        int randGround = Random.Range(0, 4);
        if (randGround == 0 && AvailabeForSpikes(position))
        {
            GenerateObject(Prefabs.spikesPrefab, position);
        }
        else
        {
            GenerateObject(Prefabs.rockPrefab, position);
        }
    }
    private void GenerateObject(GameObject prefab, Vector2 position, bool isEnemy = false)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.transform.localPosition = new Vector3(position.x, position.y ,15);
        newObject.transform.localRotation = Quaternion.identity;
        if (isEnemy)
        {
            newObject.transform.localPosition = new Vector3(position.x, position.y ,10);
            Enemy enemy = newObject.GetComponent<Enemy>();
            enemy.Initialize(false);
        }
    }
    private bool AvailabeForSpikes(Vector2 position)
    {
        if (floatCompare(position.x, -0.5f) && floatCompare(position.y, 0.5f)) return false;
        if (floatCompare(position.x, 0.5f) && floatCompare(position.y, 0.5f)) return false;
        if (floatCompare(position.x, -0.5f) && floatCompare(position.y, -1.5f)) return false;
        if (floatCompare(position.x, 0.5f) && floatCompare(position.y, -1.5f)) return false;
        return true;
    }
    private bool AvailabeForGround(Vector2 position)
    {
        if (floatCompare(position.x, -3.5f) && floatCompare(position.y, 1.5f)) return false;
        if (floatCompare(position.x, -3.5f) && floatCompare(position.y, -2.5f)) return false;
        if (floatCompare(position.x, 3.5f) && floatCompare(position.y, 1.5f)) return false;
        if (floatCompare(position.x, 3.5f) && floatCompare(position.y, -2.5f)) return false;
        if (floatCompare(position.x, -2.5f) && floatCompare(position.y, 1.5f)) return false;
        if (floatCompare(position.x, -2.5f) && floatCompare(position.y, -2.5f)) return false;
        if (floatCompare(position.x, 2.5f) && floatCompare(position.y, 1.5f)) return false;
        if (floatCompare(position.x, 2.5f) && floatCompare(position.y, -2.5f)) return false;
        return true;
    }
    private bool floatCompare(float x, float y)
    {
        return Mathf.Abs(x - y) < 0.01;
    }

     //[Enemy] AttackFly : NerveEnding : Knight = 3 : 3 : 1
    // [Ground] Rock : Spikes = 3 : 1
}