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
    public float dropDistance = 120f;  // downward drag that triggers a drop

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
    }

    public void OnDrag(PointerEventData e)
    {
        float dx = e.position.x - lastStepX;

        while (dx >= stepDistance)
        {
            if (player != null)
    player.MoveRight();
            lastStepX += stepDistance;
            dx = e.position.x - lastStepX;
            moved = true;
        }
        while (dx <= -stepDistance)
        {
            if (player != null)
    player.MoveLeft();
            lastStepX -= stepDistance;
            dx = e.position.x - lastStepX;
            moved = true;
        }

        if (!dropping && startPos.y - e.position.y >= dropDistance)
        {
            dropping = true;
            moved = true;
            if (player != null)
    player.StartDrop();
        }
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (dropping && player != null)
    player.StopDrop();
        else if (!moved && Vector2.Distance(startPos, e.position) <= tapMaxMove)
            onRotate.Invoke();
        dropping = false;
    }
}
