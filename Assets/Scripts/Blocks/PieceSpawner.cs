using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Material[] blockMaterials;

    void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        GameObject newPiece = Instantiate(
            playerPrefab,
            new Vector3(3.6f, 12.2f, 0f),
            Quaternion.identity
        );

        int shape = Random.Range(0, 5);

        CreateShape(newPiece, shape);
        ApplyRandomColors(newPiece);
        TouchControls touchControls = FindFirstObjectByType<TouchControls>();

if (touchControls != null)
{
    touchControls.player = newPiece.GetComponent<PlayerController>();
}
    }
    
    void CreateShape(GameObject piece, int shape)
{
    Transform b0 = piece.transform.GetChild(0);
    Transform b1 = piece.transform.GetChild(1);
    Transform b2 = piece.transform.GetChild(2);
    Transform b3 = piece.transform.GetChild(3);

    switch (shape)
    {
        case 0: // O
            b0.localPosition = new Vector3(0, 0, 0);
            b1.localPosition = new Vector3(1.2f, 0, 0);
            b2.localPosition = new Vector3(0, 1.2f, 0);
            b3.localPosition = new Vector3(1.2f, 1.2f, 0);
            break;

        case 1: // I
            b0.localPosition = new Vector3(0, 0, 0);
            b1.localPosition = new Vector3(1.2f, 0, 0);
            b2.localPosition = new Vector3(2.4f, 0, 0);
            b3.localPosition = new Vector3(3.6f, 0, 0);
            break;

        case 2: // T
            b0.localPosition = new Vector3(0, 0, 0);
            b1.localPosition = new Vector3(1.2f, 0, 0);
            b2.localPosition = new Vector3(2.4f, 0, 0);
            b3.localPosition = new Vector3(1.2f, 1.2f, 0);
            break;

        case 3: // L
            b0.localPosition = new Vector3(0, 0, 0);
            b1.localPosition = new Vector3(0, 1.2f, 0);
            b2.localPosition = new Vector3(0, 2.4f, 0);
            b3.localPosition = new Vector3(1.2f, 2.4f, 0);
            break;

        case 4: // J
            b0.localPosition = new Vector3(1.2f, 0, 0);
            b1.localPosition = new Vector3(1.2f, 1.2f, 0);
            b2.localPosition = new Vector3(1.2f, 2.4f, 0);
            b3.localPosition = new Vector3(0, 2.4f, 0);
            break;
    }
}
    void ApplyRandomColors(GameObject piece)
{
    foreach (Transform block in piece.transform)
    {
        Renderer renderer = block.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material randomMaterial =
                blockMaterials[Random.Range(0, blockMaterials.Length)];

            renderer.material = randomMaterial;
        }
    }
}
    
}