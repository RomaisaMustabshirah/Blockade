using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public float fallInterval = 0.8f;
    public float softDropMultiplier = 4f;
    public float horizontalMoveSpeed = 12f; // units per second, independent of falling

    private float fallTimer = 0f;
    private float currentFallInterval;
    private bool touchDropping = false;

    private GridManager gridManager;
    private PieceSpawner pieceSpawner;

    private int gridPosX;
    private int gridPosY;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pieceSpawner = FindFirstObjectByType<PieceSpawner>();

        gridPosX = Mathf.RoundToInt(transform.position.x / gridManager.cellSize);
        gridPosY = Mathf.RoundToInt(transform.position.y / gridManager.cellSize);

        currentFallInterval = fallInterval;

        transform.position = new Vector3(
            gridPosX * gridManager.cellSize,
            gridPosY * gridManager.cellSize,
            0
        );
    }

    void Update()
    {
        HandleInput();

        fallTimer += Time.deltaTime;
        if (fallTimer >= currentFallInterval)
        {
            HandleFalling();
            fallTimer = 0f;
        }

        SmoothToGrid();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            TryMove(-1, 0);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            TryMove(1, 0);

        bool softDropping = Input.GetKey(KeyCode.DownArrow)
                 || Input.GetKey(KeyCode.S)
                 || touchDropping;

        currentFallInterval = softDropping ? fallInterval / softDropMultiplier : fallInterval;
    }

    void HandleFalling()
    {
        if (CanMove(0, -1))
        {
            gridPosY -= 1;
        }
        else
        {
            LockPiece();
        }
    }

    void TryMove(int x, int y)
    {
        if (CanMove(x, y))
        {
            gridPosX += x;
            gridPosY += y;
        }
    }

    // X and Y now glide at independent speeds: X is fast/snappy,
    // Y matches the current fall rate (normal or soft-drop).
    void SmoothToGrid()
    {
        float targetX = gridPosX * gridManager.cellSize;
        float targetY = gridPosY * gridManager.cellSize;

        float fallSpeed = gridManager.cellSize / currentFallInterval;

        float newX = Mathf.MoveTowards(transform.position.x, targetX, horizontalMoveSpeed * Time.deltaTime);
        float newY = Mathf.MoveTowards(transform.position.y, targetY, fallSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, newY, 0);
    }

    bool CanMove(int x, int y)
    {
        foreach (Transform block in transform)
        {
            int localGridX = Mathf.RoundToInt(block.localPosition.x / gridManager.cellSize);
            int localGridY = Mathf.RoundToInt(block.localPosition.y / gridManager.cellSize);

            int checkX = gridPosX + localGridX + x;
            int checkY = gridPosY + localGridY + y;

            if (!gridManager.IsInsideGrid(checkX, checkY))
                return false;

            if (!gridManager.IsCellEmpty(checkX, checkY))
                return false;
        }

        return true;
    }

    void LockPiece()
    {
        transform.position = new Vector3(
            gridPosX * gridManager.cellSize,
            gridPosY * gridManager.cellSize,
            0
        );

        Transform[] blocks = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            blocks[i] = transform.GetChild(i);

        foreach (Transform block in blocks)
        {
            int localGridX = Mathf.RoundToInt(block.localPosition.x / gridManager.cellSize);
            int localGridY = Mathf.RoundToInt(block.localPosition.y / gridManager.cellSize);

            int finalX = gridPosX + localGridX;
            int finalY = gridPosY + localGridY;

            block.position = new Vector3(
                finalX * gridManager.cellSize,
                finalY * gridManager.cellSize,
                0
            );

            gridManager.AddBlock(finalX, finalY, block);
            block.SetParent(null);
        }
        AudioManager.Instance.PlayDrop();

        Destroy(gameObject);

        gridManager.ResolveBoard();

    if (gridManager.HasBlocksInTopRow())
    {
    GameManager.Instance.GameOver();
    return;
    }

pieceSpawner.SpawnPiece();
    }
public void MoveLeft()
{
    TryMove(-1, 0);
}

public void MoveRight()
{
    TryMove(1, 0);
}

public void RotatePiece()
{
    // We'll implement rotation here once we confirm
    // how your pieces are structured.
}

public void StartDrop()
{
    touchDropping = true;
}

public void StopDrop()
{
    touchDropping = false;
}
public void HardDrop()
{
    while (CanMove(0, -1))
    {
        gridPosY--;
    }

    transform.position = new Vector3(
        gridPosX * gridManager.cellSize,
        gridPosY * gridManager.cellSize,
        0
    );

    LockPiece();
}
}