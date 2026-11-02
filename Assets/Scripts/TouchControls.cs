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

    public void OnPointerDown(PointerEventData e)
    {
        startPos = e.position;
        lastStepX = e.position.x;
        dropping = false;
        moved = false;

        if (logEvents) Debug.Log("TouchControls: pointer down at " + e.position);
    }

    public void OnDrag(PointerEventData e)
    {
        // ---- Horizontal movement: one step per stepDistance ----
        float dx = e.position.x - lastStepX;

        while (dx >= stepDistance)
        {
            if (player != null) player.MoveRight();
            lastStepX += stepDistance;
            dx = e.position.x - lastStepX;
            moved = true;
        }

        while (dx <= -stepDistance)
        {
            if (player != null) player.MoveLeft();
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

            if (player != null) player.HardDrop();
        }
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (logEvents) Debug.Log("TouchControls: pointer up, dropping=" + dropping + ", moved=" + moved);

        if (dropping)
        {
            // Only needed if HardDrop() sets a continuous "fast fall" state.
            // If HardDrop() teleports the piece instantly, this is harmless.
            if (player != null) player.StopDrop();
        }
        else if (!moved && Vector2.Distance(startPos, e.position) <= tapMaxMove)
        {
            onRotate.Invoke();
        }

        dropping = false;
    }
}