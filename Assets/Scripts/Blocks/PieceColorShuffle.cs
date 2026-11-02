using UnityEngine;
using System.Collections.Generic;

public class PieceColorShuffle : MonoBehaviour
{
    public void ShuffleColors()
    {
    AudioManager.Instance.PlayShuffle();

    List<Material> materials = new List<Material>();

        foreach (Transform block in transform)
        {
            Renderer renderer = block.GetComponent<Renderer>();

            if (renderer != null)
                materials.Add(renderer.sharedMaterial); // was: renderer.material
        }

        // Shuffle the materials
        for (int i = materials.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            Material temp = materials[i];
            materials[i] = materials[j];
            materials[j] = temp;
        }

        int index = 0;

        foreach (Transform block in transform)
        {
            Renderer renderer = block.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial = materials[index]; // was: renderer.material
                index++;
            }
        }
    }
}