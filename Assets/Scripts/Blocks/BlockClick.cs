using UnityEngine;

public class BlockClick : MonoBehaviour
{
    void OnMouseDown()
    {
        PieceColorShuffle shuffle = GetComponentInParent<PieceColorShuffle>();

        if (shuffle != null)
        {
            shuffle.ShuffleColors();
        }
    }
}