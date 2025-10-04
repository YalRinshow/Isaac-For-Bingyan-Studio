using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rbPlayer;
    private float speedMove = Constants.PLAYER_MOVE_SPEED;
    private float nextFire = 0.0f;

    public GameObject bomb;
    private int bombNumber = 10;

    public GameObject key;
    public int keyNumber = 2;

    private int playerHealth = Constants.PLAYER_HEALTH_LIMIT;

    public GameObject head;
    private Head playerHead;
    public static Player Instance { get; private set; }

    private float lastIFrames = 0.0f;
    private bool isInIFrames = false;

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
        TeardropCheck();
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
    }
    private void TeardropCheck()
    {
        if (Time.time < nextFire) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShootTeardrop(Vector2.up);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShootTeardrop(Vector2.down);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShootTeardrop(Vector2.left);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShootTeardrop(Vector2.right);
            nextFire = Time.time + Constants.TEARDROP_FIRE_RATE;
        }
    }
    private void ItemCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && bombNumber > 0)
        {
            bombNumber--;
            bomb.GetComponentInChildren<TextMeshProUGUI>().text = bombNumber.ToString("D2");
            GameObject createBomb = Instantiate(Prefabs.bombPrefab, rbPlayer.position, Quaternion.identity);
        }
    }
    public void GetBomb()
    {
        bombNumber++;
        bomb.GetComponentInChildren<TextMeshProUGUI>().text = bombNumber.ToString("D2");
    }
    public void GetKey()
    {
        keyNumber++;
        key.GetComponentInChildren<TextMeshProUGUI>().text = keyNumber.ToString("D2");
    }
    public void UseKey()
    {
        keyNumber--;
        key.GetComponentInChildren<TextMeshProUGUI>().text = keyNumber.ToString("D2");
    }
    public void GetHeart()
    {
        if (playerHealth == Constants.PLAYER_HEALTH_LIMIT) return;
        playerHealth = Mathf.Min(playerHealth + 2, Constants.PLAYER_HEALTH_LIMIT);
        UIManager.Instance.UpdateHeart(playerHealth);
    }
    private void ShootTeardrop(Vector2 direction)
    {
        GameObject teardrop = Instantiate(Prefabs.teardropPrefab, rbPlayer.position, Quaternion.identity);
        Teardrop teardropPhysics= teardrop.GetComponent<Teardrop>();
        teardropPhysics.SetDirection(direction);
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
}
