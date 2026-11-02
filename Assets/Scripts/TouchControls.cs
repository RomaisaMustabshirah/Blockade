using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public PlayerController player;

    [Header("Hook these to your existing methods")]
    public UnityEvent onMoveLeft;
    public UnityEvent onMoveRight;
    public UnityEvent onRotate;
    public UnityEvent onDropStart;
    public UnityEvent onDropEnd;

    [Header("Tuning (pixels)")]
    public float stepDistance = 60f;   // how far to drag for one column move
    public float tapMaxMove = 20f;     // max movement that still counts as a tap
    public float dropDistance = 80f;   // downward drag that triggers a hard drop

    [Header("Debug")]
    public bool logEvents = true;      // shows in the browser console (F12)

    Vector2 startPos;
    float lastStepX;
    bool dropping;
    bool moved;

    // Always returns the piece currently in play (pieces are destroyed and respawned)
    PlayerController CurrentPlayer
    {
        get
        {
            if (player == null)
                player = FindFirstObjectByType<PlayerController>();
            return player;
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        startPos = e.position;
        lastStepX = e.position.x;
        dropping = false;
        moved = false;

        if (logEvents) Debug.Log("TouchControls: pointer down at " + e.position + ", player found: " + (CurrentPlayer != null));
    }

    public void OnDrag(PointerEventData e)
    {
        // ---- Horizontal movement: one step per stepDistance ----
        float dx = e.position.x - lastStepX;

        while (dx >= stepDistance)
        {
            PlayerController p = CurrentPlayer;
            if (p != null) p.MoveRight();
            lastStepX += stepDistance;
            dx = e.position.x - lastStepX;
            moved = true;
        }

        while (dx <= -stepDistance)
        {
            PlayerController p = CurrentPlayer;
            if (p != null) p.MoveLeft();
            lastStepX -= stepDistance;
            dx = e.position.x - lastStepX;
            moved = true;
        }

        // ---- Downward drag: hard drop (fires once per touch) ----
        float downwardDistance = startPos.y - e.position.y;

        if (!dropping && downwardDistance > dropDistance)
        {
            dropping = true;
            moved = true;

            if (logEvents) Debug.Log("TouchControls: HARD DROP triggered");

            PlayerController p = CurrentPlayer;
            if (p != null) p.HardDrop();
        }
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (!dropping && !moved && Vector2.Distance(startPos, e.position) <= tapMaxMove)
        {
            onRotate.Invoke();
        }

        dropping = false;
    }
}