using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    Vector2 moveInput;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    TouchingDirections touchingDirections;
    private float horizontalInput;
    public Transform player;
    public Tilemap mazeTilemap;
    public TileBase userProvidedTile;
    public TileBase badTile;
    public int[,] levels;
    public bool[] created = new bool[500000];
    public int nJumps;
    private bool hasEncounteredBadTile = false;
    private bool isOnBadTile = false;
    

    //variable for the healthbar
    public int maxHealth = 4;
    public int health = 5;
    public int currentHealth;

    public HealthBar healthBar;
    
    Rigidbody2D rb;
    Animator animator;

    public Score score;
    



    public void Start()
    {
        GenFirstLev();
        created[0] = true;
        DrawSquare(new Vector3Int(0, 24, 0), 24);
        mazeTilemap.SetTile(new Vector3Int(24 - 1, 24 * 2, 0), null);
        mazeTilemap.SetTile(new Vector3Int(24 - 1, 24, 0), userProvidedTile);
        created[1] = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    void ShuffleArray(int[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            int temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
    void RecursiveBacktracking(int x, int y)
    {
        levels[x, y] = (Random.Range(0, 5) == 0) ? 2 : 0; // 2 represents badTile

        int[] directions = { 1, 2, 3, 4 };
        ShuffleArray(directions);

        foreach (int dir in directions)
        {
            int nx = x + 2 * ((dir == 2) ? -1 : (dir == 4) ? 1 : 0);
            int ny = y + 2 * ((dir == 1) ? -1 : (dir == 3) ? 1 : 0);

            if (nx > 0 && nx < 24 && ny > 0 && ny < 24 && levels[nx, ny] == 1)
            {
                levels[x + (nx - x) / 2, y + (ny - y) / 2] = (Random.Range(0, 5) == 0) ? 2 : 0;
                RecursiveBacktracking(nx, ny);
            }
        }
    }
    public void DrawSquare(Vector3Int start, int length)
    {
        levels = new int[24, 24];
        for (int i = 0; i < 24; i++)
        {
          for (int j = 0; j < 24; j++)
          {
              levels[i, j] = 1;
          }
        }
        created = new bool[500000];
        for (int i = 0; i < 500000; i++) 
        {
          created[i] = false;  
        }
        RecursiveBacktracking(1, 1);
        for (int i = 0; i < 24; i++)
        {
          for (int j = 0; j< 24; j++)
          {
              if (levels[i, j] == 1)
              {
                int randomNumber = UnityEngine.Random.Range(1,11);
                if (randomNumber == 1)
                {
                  mazeTilemap.SetTile(new Vector3Int(i+start.x, j+start.y, 0), badTile);
                } else {
                  mazeTilemap.SetTile(new Vector3Int(i+start.x, j+start.y, 0), levels[i, j] == 1 ? userProvidedTile : null);
                  }
              } else {
                mazeTilemap.SetTile(new Vector3Int(i+start.x, j+start.y, 0), levels[i, j] == 1 ? userProvidedTile : null);
              }
          }
        }
        for (int i = start.y; i < start.y + length; i++) 
        {
          mazeTilemap.SetTile(new Vector3Int(24, i, 0), userProvidedTile);  
        }
    }

    public void ClearSquare(Vector3Int start, int length)
    {
        for (int i = start.x; i <= start.x + length; i++)
        {
            for (int j = start.y; j <= start.y + length; j++)
            {
                mazeTilemap.SetTile(new Vector3Int(i,j, 0), null);
            }
        }
    }
    public void GenFirstLev()
    {
        DrawSquare(new Vector3Int(0, 0, 0), 24);
        mazeTilemap.SetTile(new Vector3Int(23, 24, 0), null);
        mazeTilemap.SetTile(new Vector3Int(23, 0, 0), userProvidedTile);
    }

    public void GenNextLev(int lev)
    {
        // Assuming a level size of 24x24

        // Check if any tile in the level above is not null

        if (!created[lev])
        {
            DrawSquare(new Vector3Int(0, 24 * (lev + 1), 0), 24);
        }
        mazeTilemap.SetTile(new Vector3Int(24 - 1, 24 * (lev + 1), 0), null);
        mazeTilemap.SetTile(new Vector3Int(24 - 1, 24 * lev, 0), userProvidedTile);
        score.SetText(lev + 1);
        ClearSquare(new Vector3Int(0, 24 * (lev - 2), 0), 24);
        created[lev] = true;
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    return walkSpeed;
                }
                else
                {
                    return airWalkSpeed;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _IsFacingRight = true;

    public bool IsFacingRight
    {
        get { return _IsFacingRight; }
        private set
        {
            if (_IsFacingRight != value)
            {
                //flip the local scale
                transform.localScale *= new Vector2(-1, 1);
            }

            _IsFacingRight = value;
        }
    }

        private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();

    }

    private void FixedUpdate(){
        rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);      

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void playerDead()
    {
        animator.SetTrigger(AnimationStrings.isDead);

        // Wait for the death animation to finish (you can use a coroutine for this)
        StartCoroutine(RestartSceneAfterAnimation());

    }

    private IEnumerator RestartSceneAfterAnimation()
    {
        // Wait for the duration of the death animation
        float deathAnimationDuration = GetDeathAnimationDuration(); // Implement this function

        yield return new WaitForSeconds(deathAnimationDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private float GetDeathAnimationDuration(){
        return 1.0f; // Placeholder value, replace with actual implementation
    }

    private void CheckForTraps(Vector3Int position)
    {
        TileBase tile = mazeTilemap.GetTile(position);

        if (tile == badTile && !isOnBadTile)
        {
            // Player is on a bad tile
            health--;
            healthBar.SetHealth(health); // Update the health bar UI
            Debug.Log("Health: " + health);

            isOnBadTile = true; // Set the flag to true to indicate that the player is currently on a bad tile

            if (health <= 0)
            {
                playerDead();
            }
        }
        else if (tile != badTile)
        {
            // Player has moved away from the bad tile
            isOnBadTile = false;
        }
    }


    public void onMove(InputAction.CallbackContext context)
    {   
        if (health <= 0)
        {
            playerDead();
            return; // Don't process further input if the player is dead
        }

        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        setFacingDirection(moveInput);

        float y = player.transform.position.y;
        float x = player.transform.position.x;
        
        // Reset the flag when the player moves to a new position
        hasEncounteredBadTile = false;

        if (y % 24 > 1f)
        {
            GenNextLev((int)(y) / 24);

            // increase score by 1
        }

        if (mazeTilemap.GetTile(new Vector3Int((int)x, (int)y, 0)) != null)
        {
            y += 1f;
        }

        // Call CheckForTraps regardless of player movement
        CheckForTraps(new Vector3Int((int)x, (int)y - 1, 0));
    }

    public void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    { 
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            nJumps++;
        }
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && !touchingDirections.IsOnWall;
    }
}