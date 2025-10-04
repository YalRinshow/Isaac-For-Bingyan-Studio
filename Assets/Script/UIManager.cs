using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] heart;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public GameObject[] energySlots;
    int currentHeart;
    int currentEnergy;
    public static UIManager Instance { get; private set; }
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
    private void Start()
    {
        currentHeart = 6;
        heart[0].sprite = heart[1].sprite = heart[2].sprite = fullHeart;
        currentEnergy = 0;
        UpdateEnergy(0);
    }
    public void UpdateEnergy(int energy)
    {
        currentEnergy = Mathf.Min(currentEnergy + energy, 6);
        for (int i = 0; i < currentEnergy; i++)
        {
            UpdateSpriteRender(energySlots[i].GetComponent<SpriteRenderer>(), Color.yellow);
        }
        for (int i = currentEnergy; i < 6; i++)
        {
            UpdateSpriteRender(energySlots[i].GetComponent<SpriteRenderer>(), Color.white);
        }
    }
    public void UpdateHeart(int health)
    {
        currentHeart = health;
        for (int i = 0; i < 3; i++) heart[i].sprite = emptyHeart;
        for (int i = 0; i < (currentHeart >> 1); i++)
        {
            heart[i].sprite = fullHeart;
        }
        if (currentHeart % 2 == 1)
        {
            heart[(currentHeart + 1 >> 1) - 1].sprite = halfHeart;
        }
    }
    private void UpdateSpriteRender(SpriteRenderer sprite, Color color)
    {
        sprite.color = color;
    }
}