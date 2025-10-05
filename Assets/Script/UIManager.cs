using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] heart;
    public Image activeItem;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public GameObject[] energySlots;

    public Image hpImage;
    public Image hpEffectImage;
    public float maxBossHp;
    public float currentBossHp;
    public float bufferTime = 0.5f;
    Coroutine hpUpdateCoroutine;

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
    public void Initialize()
    {
        currentHeart = 6;
        heart[0].sprite = heart[1].sprite = heart[2].sprite = fullHeart;
        currentEnergy = 0;
        UpdateActiveItemImage(Constants.IMAGE_ITEM_FILE + Constants.ITEM_DEFAULT);
        UpdateEnergy(0);
        //currentEnergy = 6;
        //UpdateEnergy(6);
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
    public void UpdateActiveItem(ItemManager.ItemType itemType)
    {
        if (itemType == ItemManager.ItemType.Null) UpdateActiveItemImage(Constants.IMAGE_ITEM_FILE + Constants.ITEM_DEFAULT);
        if (itemType == ItemManager.ItemType.RazorBlade) UpdateActiveItemImage(Constants.IMAGE_ITEM_FILE + Constants.ITEM_RAZORBLADE);
        if (itemType == ItemManager.ItemType.TheBookOfSin) UpdateActiveItemImage(Constants.IMAGE_ITEM_FILE + Constants.ITEM_THEBOOKOFSIN);
    }
    private void UpdateSpriteRender(SpriteRenderer sprite, Color color)
    {
        sprite.color = color;
    }
    private void UpdateActiveItemImage(string imagePath)
    {
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            activeItem.sprite = sprite;
        }
        else
        {
            Debug.Log(Constants.IMAGE_LOAD_FAILED + imagePath);
        }
    }
    public void InitializeHealthBar(float health)
    {
        maxBossHp = health;
        currentBossHp = health;
    }
    public void DecreaseHealth(float damage)
    {
        currentBossHp = Mathf.Clamp(currentBossHp - damage, 0.0f, maxBossHp);
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        hpImage.fillAmount = currentBossHp / maxBossHp;
        if (hpUpdateCoroutine != null)
        {
            StopCoroutine(hpUpdateCoroutine);
        }
        hpUpdateCoroutine = StartCoroutine(UpdateHealthBarEffect());
    }
    private IEnumerator UpdateHealthBarEffect()
    {
        float effectLength = hpEffectImage.fillAmount - hpImage.fillAmount;
        float elapsedTime = 0.0f;
        while (elapsedTime < bufferTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            hpEffectImage.fillAmount = Mathf.Lerp(hpImage.fillAmount + effectLength, hpImage.fillAmount, elapsedTime / bufferTime);
            yield return null;
        }
        hpEffectImage.fillAmount = hpImage.fillAmount;
    }
}