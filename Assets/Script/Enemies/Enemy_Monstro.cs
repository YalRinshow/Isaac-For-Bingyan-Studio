using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Monstro : Enemy
{

    private bool isJumping = false;

    private float jumpHeight = 2.0f;
    private float maxJumpDistance = 3.0f;
    private float jumpRate = 2.0f;
    private float jumpTime = 1.5f;

    private float disappearTime = 1.5f;
    private float landTime = 1.0f;
    private float bigJumpHeight = 15.0f;
    private float bigJumpRate = 6.0f;
    private float fireRate = 3.0f;

    private float lastShoot = -Mathf.Infinity;
    private float lastJump = -Mathf.Infinity;
    private float lastBigJump = -Mathf.Infinity;

    private bool haveBeginAction = false;

    private Coroutine currentJumpCoroutine;
    private CapsuleCollider2D jumpCollider;
    private CircleCollider2D groundCollider;
    private struct newTeardrop
    {
        public Vector2 direction;
        public float speed;
    };
    public override void Initialize(bool dropKey = false)
    {
        health = 250.0f;
        rbEnemy = GetComponent<Rigidbody2D>();
        GetDropItem(dropKey);
        UIManager.Instance.InitializeHealthBar(health);
        jumpCollider = GetComponentInChildren<CapsuleCollider2D>();
        groundCollider = GetComponentInChildren<CircleCollider2D>();
        jumpCollider.enabled = false;
        isElite = true;
    }
    private void Update()
    {
        if (Map.currentRoomNumber != belongingRoomNumber) return;
        if (!isJumping)
        {
            // [Jump : BigJump : Shoot] = 1 : 2 : 3
            int randAction = Random.Range(0, 4);
            if (randAction < 1 && Time.time > lastJump + jumpRate)
            {
                haveBeginAction = true;
                JumpToPlayer();
            }
            else if (randAction < 3 && Time.time > lastBigJump + bigJumpRate && Time.time > lastJump + jumpRate && haveBeginAction)
            {
                haveBeginAction = true;
                StartCoroutine(BigJumpToPlayer());
            }
            else if (randAction < 6 && Time.time > lastShoot + fireRate)
            {
                haveBeginAction = true;
                ShootEnemyTeardrop();
            }
        }
    }
    private void ShootEnemyTeardrop(bool normalShoot = true)
    {
        lastShoot = Time.time;
        int teardropNumber = normalShoot ? Random.Range(5, 10) : Random.Range(8, 16);
        Vector2 directionToPlayer = (Player.Instance.rbPlayer.position - rbEnemy.position).normalized;
        Debug.Log(directionToPlayer);
        float angle = Mathf.Acos(directionToPlayer.x) * Mathf.Rad2Deg;
        float startAngle = angle - 40.0f;
        float endAngle = angle + 40.0f;
        float startSpeed = 2.0f;
        float endSpeed = 4.0f;
        for (int i = 0; i < teardropNumber; i++)
        {
            newTeardrop teardrop = GetRandomTeardrop(startAngle, endAngle, startSpeed, endSpeed);
            ShootEnemyTeardrop(teardrop.direction, rbEnemy.position, teardrop.speed);
        }
    }
    private newTeardrop GetRandomTeardrop(float startAngle, float endAngle, float startSpeed, float endSpeed)
    {
        float randAngle = Random.Range(startAngle, endAngle) * Mathf.Deg2Rad;
        float randSpeed = Random.Range(startSpeed, endSpeed);
        Vector2 newDirection = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));
        return new newTeardrop() { direction = newDirection, speed = randSpeed };
    }
    private void ShootEnemyTeardrop(Vector2 direction, Vector2 position, float speed)
    {
        GameObject enemyTeardrop = Instantiate(Prefabs.enemyTeardropPrefab, position, Quaternion.identity);
        EnemyTeardrop enemyTeardropPhysics = enemyTeardrop.GetComponent<EnemyTeardrop>();
        enemyTeardropPhysics.SetDirectionAndSpeed(direction, speed);
    }
    private void StartJumpCollider()
    {
        jumpCollider.enabled = true;
        groundCollider.enabled = false;
    }
    private void StopJumpCollider()
    {
        jumpCollider.enabled = false;
        groundCollider.enabled = true;
    }
    public override void TakeTeardropDamage(float damage, Vector2 direction)
    {
        TakeDamage(damage);
    }
    private IEnumerator BigJumpToPlayer()
    {
        StartJumpCollider();
        isJumping = true;

        rbEnemy.simulated = false;

        float startHeight = transform.localPosition.y;
        float targetHeight = transform.localPosition.y + bigJumpHeight;
        yield return StartCoroutine(JumpOutOfScreen(targetHeight));

        if (currentJumpCoroutine != null) StopCoroutine(currentJumpCoroutine);
        Vector2 targetPosition = FindSafePosition(Player.Instance.transform.localPosition);
        transform.localPosition = new Vector2(targetPosition.x, transform.localPosition.y);
        targetHeight = targetPosition.y;

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(LandOnPlayer(targetHeight));

        rbEnemy.simulated = true;

        isJumping = false;

        StopJumpCollider();
        lastShoot = Time.time;
        ShootEnemyTeardrop(false);
    }
    private IEnumerator JumpOutOfScreen(float targetHeight)
    {
        lastBigJump = Time.time;
        float elapsedTime = 0.0f;
        while (elapsedTime < disappearTime)
        {
            elapsedTime += Time.deltaTime;
            float target = Mathf.Lerp(transform.localPosition.y, targetHeight, elapsedTime / disappearTime);
            transform.localPosition = new Vector2(transform.localPosition.x, target);
            yield return null;
        }
    }
    private IEnumerator LandOnPlayer(float targetHeight)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < landTime)
        {
            elapsedTime += Time.deltaTime;
            float target = Mathf.Lerp(transform.localPosition.y, targetHeight, elapsedTime / landTime);
            transform.localPosition = new Vector2(transform.localPosition.x, target);
            yield return null;
        }
        
    }
    private Vector2 FindSafePosition(Vector2 position)
    {
        if (isSafePosition(position)) return position;
        return FindRandomSafePosition(position, 0.2f);
    }
    private Vector2 FindRandomSafePosition(Vector2 position, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            float distance = Random.Range(0.0f, radius);
            Vector2 targetPosition = position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            if (isSafePosition(targetPosition)) return targetPosition;
        }
        return position;
    }
    bool isSafePosition(Vector2 position)
    {
        Vector3 target = new Vector3(position.x, position.y, 10);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(Constants.TAG_WALL))
            {
                return false;
            }
        }
        return true;
    }
    private void JumpToPlayer()
    {
        StartJumpCollider();
        Vector3 playerPosition = Player.Instance.transform.localPosition;
        Vector3 direction = (playerPosition - transform.localPosition).normalized;
        float distance = Mathf.Min(Vector3.Distance(transform.localPosition, playerPosition), maxJumpDistance);
        Vector3 jumpTarget = transform.localPosition + direction * distance;
        if (currentJumpCoroutine != null) StopCoroutine(currentJumpCoroutine);
        currentJumpCoroutine = StartCoroutine(JumpRoutine(jumpTarget));
        StopJumpCollider();
    }
    private IEnumerator JumpRoutine(Vector3 targetPosition)
    {
        isJumping = true;
        lastJump = Time.time;
        Vector3 startPosition = transform.localPosition;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime / jumpTime;
            progress = Mathf.Clamp01(progress);
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            float verticalOffset = 4.0f * progress * (1.0f - progress) * jumpHeight;
            transform.localPosition = new Vector3(horizontalPosition.x, startPosition.y + verticalOffset, horizontalPosition.z);
            yield return null;
        }
        isJumping = false;
    }
    private void OnDestroy()
    {
        if (currentJumpCoroutine != null)
        {
            StopCoroutine(currentJumpCoroutine);
        }
    }
}
