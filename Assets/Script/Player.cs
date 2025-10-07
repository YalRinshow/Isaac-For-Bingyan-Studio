using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody2D rbPlayer;
    private float speedMove = Constants.PLAYER_MOVE_SPEED ;
    private float nextFire = 0.0f;

    public GameObject bomb;
    private int bombNumber = 2;

    public GameObject key;
    public int keyNumber = 0;

    private int playerHealth = Constants.PLAYER_HEALTH_LIMIT;
    private int playerEnergy = 0;

    public GameObject head;
    private Head playerHead;
    public static Player Instance { get; private set; }

    private float lastIFrames = 0.0f;
    private bool isInIFrames = false;

    private bool theInnerEyeMode = false;

    public bool teardropEffectInCurrentRoom = false;

    private ItemManager.ItemType currentActiveItem = ItemManager.ItemType.Null;

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
        Initialize();
    }
    private void Update()
    {
        IFramesCheck();
        if (theInnerEyeMode) TheInnerEyeTearDropCheck();
        else TeardropCheck();
        ItemCheck();
    }
    private void IFramesCheck()
    {
        if (!isInIFrames) return;
        if (Time.time > lastIFrames + Constants.PLAYER_I_FRAMES_TIME)
        {
            isInIFrames = false;
            playerHead.StopFlash();
        }
    }
    private void FixedUpdate()
    {
        float moveX = Input.GetAxis(Constants.DIRECTION_HORIZONTAL);
        float moveY = Input.GetAxis(Constants.DIRECTION_VERTICAL);
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        if (Mathf.Abs(moveX) < 0.01f)
        {
            playerHead.UpdateHeadImage(moveY > 0 ? 0 : 1);
        }
        else
        {
             playerHead.UpdateHeadImage(moveX > 0 ? 3 : 2);
        }
        rbPlayer.velocity = moveDirection * speedMove;
    }
    public void Initialize()
    {
        bomb.GetComponentInChildren<TextMeshProUGUI>().text = bombNumber.ToString("D2");
        key.GetComponentInChildren<TextMeshProUGUI>().text = keyNumber.ToString("D2");
        rbPlayer = GetComponent<Rigidbody2D>();
        playerHead = head.GetComponent<Head>();
        Teardrop.teardropDamage = Constants.TEARDROP_DAMAGE;
        SpriteRenderer spriteRenderer = Prefabs.teardropPrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
    }
    private void TheInnerEyeTearDropCheck()
    {
        if (Time.time < nextFire) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(TheInnerEyeShoot(Vector2.up, rbPlayer.position));
            nextFire = Time.time + Constants.TEARDROP_THE_INNER_EYE_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(TheInnerEyeShoot(Vector2.down, rbPlayer.position));
            nextFire = Time.time + Constants.TEARDROP_THE_INNER_EYE_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(TheInnerEyeShoot(Vector2.left, rbPlayer.position));
            nextFire = Time.time + Constants.TEARDROP_THE_INNER_EYE_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(TheInnerEyeShoot(Vector2.right, rbPlayer.position));
            nextFire = Time.time + Constants.TEARDROP_THE_INNER_EYE_FIRE_RATE;
        }
    }
    private IEnumerator TheInnerEyeShoot(Vector2 direction, Vector2 postion)
    {
        for (int i = 0; i < 3; i++)
        {
            ShootTeardrop(direction, postion);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void TeardropCheck()
    {
        if (Time.time < nextFire) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShootTeardrop(Vector2.up, rbPlayer.position);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShootTeardrop(Vector2.down, rbPlayer.position);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShootTeardrop(Vector2.left, rbPlayer.position);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShootTeardrop(Vector2.right, rbPlayer.position);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
    }
    private void ShootTeardrop(Vector2 direction, Vector2 postion)
    {
        GameObject teardrop = Instantiate(Prefabs.teardropPrefab, postion, Quaternion.identity);
        Teardrop teardropPhysics = teardrop.GetComponent<Teardrop>();
        teardropPhysics.SetDirection(direction);
    }
    private void ItemCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && bombNumber > 0)
        {
            bombNumber--;
            bomb.GetComponentInChildren<TextMeshProUGUI>().text = bombNumber.ToString("D2");
            GameObject createBomb = Instantiate(Prefabs.bombPrefab, Map.rooms[Map.currentRoomNumber].gameObject.transform);
            createBomb.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 15);
            createBomb.transform.localRotation = Quaternion.identity;
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentActiveItem != ItemManager.ItemType.Null)
        {
            UseActiveItem();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Teardrop.teardropDamage = 114514;
            SpriteRenderer spriteRenderer = Prefabs.teardropPrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.black;
        }
    }
    public void GetItem(ItemManager.ItemType itemType, bool isActiveItem)
    {
        if (isActiveItem)
        {
            GetActiveItem(itemType);
            return;
        }
        if (itemType == ItemManager.ItemType.Key) GetKey();
        if (itemType == ItemManager.ItemType.Bomb) GetBomb();
        if (itemType == ItemManager.ItemType.Heart) GetHeart();
        if (itemType == ItemManager.ItemType.TheInnerEye) GetTheInnerEye();
    }
    private void GetActiveItem(ItemManager.ItemType itemType)
    {
        if (currentActiveItem != ItemManager.ItemType.Null)
        {
            ItemManager.GenerateItem(currentActiveItem, transform.localPosition);
        }
        UIManager.Instance.UpdateActiveItem(itemType);
        currentActiveItem = itemType;
    }
    private void UseActiveItem()
    {
        bool useActiveItem = false;
        if (currentActiveItem == ItemManager.ItemType.RazorBlade && playerHealth > 2)
        {
            Teardrop.teardropDamage = Constants.TEARDROP_RAZOR_BLADE_DAMAGE;
            SpriteRenderer spriteRenderer = Prefabs.teardropPrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            TakeDamage(2);
            teardropEffectInCurrentRoom = true;
            useActiveItem = true;
        }
        if (currentActiveItem == ItemManager.ItemType.TheBookOfSin && playerEnergy == 6)
        {
            playerEnergy = 0;
            UIManager.Instance.UpdateEnergy(-6);
            int randItem = Random.Range(0, 3);
            ItemManager.ItemType item = ItemManager.ItemType.Null;
            if (randItem == 0) item = ItemManager.ItemType.Key;
            if (randItem == 1) item = ItemManager.ItemType.Bomb;
            if (randItem == 2) item = ItemManager.ItemType.Heart;
            ItemManager.GenerateItem(item, transform.localPosition);
            useActiveItem = true;
        }
        if (useActiveItem)
        {
            UIManager.Instance.UpdateActiveItem(ItemManager.ItemType.Null);
            currentActiveItem = ItemManager.ItemType.Null;
        }
    }
    public void StopTeardropEffect()
    {
        if (teardropEffectInCurrentRoom)
        {
            Teardrop.teardropDamage = Constants.TEARDROP_DAMAGE;
            SpriteRenderer spriteRenderer = Prefabs.teardropPrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
            teardropEffectInCurrentRoom = false;
        }
    }
    private void GetKey()
    {
        keyNumber++;
        key.GetComponentInChildren<TextMeshProUGUI>().text = keyNumber.ToString("D2");
    }
    public void GetBomb()
    {
        bombNumber++;
        bomb.GetComponentInChildren<TextMeshProUGUI>().text = bombNumber.ToString("D2");
    }
    private void GetHeart()
    {
        if (playerHealth == Constants.PLAYER_HEALTH_LIMIT) return;
        playerHealth = Mathf.Min(playerHealth + 2, Constants.PLAYER_HEALTH_LIMIT);
        UIManager.Instance.UpdateHeart(playerHealth);
    }
    private void GetTheInnerEye()
    {
        theInnerEyeMode = true;
    }
    public void UseKey()
    {
        keyNumber--;
        key.GetComponentInChildren<TextMeshProUGUI>().text = keyNumber.ToString("D2");
    }
    public void TakeDamage(int damage = 2)
    {
        if (isInIFrames && Time.time < lastIFrames + Constants.PLAYER_I_FRAMES_TIME) return;
        if (damage >= playerHealth)
        {
            playerHealth = 0;
            UIManager.Instance.UpdateHeart(playerHealth);
            GameManager.Instance.GameOver(false);
            Destroy(gameObject);
        }
        playerHealth -= damage;
        isInIFrames = true;
        lastIFrames = Time.time;
        playerHead.StartFlash();
        UIManager.Instance.UpdateHeart(playerHealth);
    }
    public void AddEnergy(int energy)
    {
        playerEnergy = Mathf.Min(playerEnergy + energy, 6);
    }
}
