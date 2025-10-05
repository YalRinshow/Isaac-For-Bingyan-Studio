using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType
    {
        Key,
        Bomb,
        Heart,
        TheInnerEye,
        RazorBlade,
        TheBookOfSin,
        Null
    }
    public static void GenerateItem(ItemType itemType, Vector2 position)
    {
        Transform parentTransform = Map.rooms[Map.currentRoomNumber].GetComponent<Transform>();
        GameObject newObject = Instantiate(GetItemPrefab(itemType), parentTransform);
        newObject.transform.localPosition = new Vector3(position.x, position.y, 15);
        newObject.transform.localRotation = Quaternion.identity;
    }
    private static GameObject GetItemPrefab(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Key => Prefabs.keyDroppedPrefab,
            ItemType.Bomb => Prefabs.bombDroppedPrefab,
            ItemType.Heart => Prefabs.heartDroppedPrefab,
            ItemType.TheInnerEye => Prefabs.theInnerEyePrefab,
            ItemType.RazorBlade => Prefabs.razorBladePrefab,
            ItemType.TheBookOfSin => Prefabs.theBookOfSinPrefab,
            _ => null
        };
    }
}
