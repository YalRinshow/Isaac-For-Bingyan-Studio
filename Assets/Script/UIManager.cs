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
    int currentHeart;
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
    }
    public void UpdateHeart(int damage)
    {
        while (damage > 0)
        {
            if (currentHeart % 2 == 1)
            {
                heart[(currentHeart + 1 >> 1) - 1].sprite = emptyHeart;
            }
            else
            {
                heart[(currentHeart >> 1) - 1].sprite = halfHeart;
            }
            damage--;
            currentHeart--;
            if (currentHeart == 0)
            {
                GameManager.Instance.GameOver(false);
            }
        }
    }
}