using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 12;
    public float cellSize = 1.2f;

    public GameObject blockPrefab;

    public float clearEffectDuration = 0.6f;
    public int fragmentCount = 6;
    public float explosionForce = 3f;

    private Transform[,] grid;

    void Awake()
    {
        grid = new Transform[width, height];
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsCellEmpty(int x, int y)
    {
        if (!IsInsideGrid(x, y))
            return false;

        return grid[x, y] == null;
    }

    public void AddBlock(int x, int y, Transform block)
    {
        if (IsInsideGrid(x, y))
        {
            grid[x, y] = block;
        }
    }

    public bool HasBlocksInTopRow()
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)
                return true;
        }

        return false;
    }

    Material GetMaterial(int x, int y)
    {
        Transform block = grid[x, y];
        if (block == null) return null;

        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer == null) return null;

        return renderer.sharedMaterial;
    }

    public bool ClearMatches()
    {
        bool[,] toClear = new bool[width, height];
        bool foundAny = false;
        int clearedBlocks = 0;

        for (int y = 0; y < height; y++)
        {
            int runStart = 0;
            Material runMat = GetMaterial(0, y);

            for (int x = 1; x <= width; x++)
            {
                Material currentMat = (x < width) ? GetMaterial(x, y) : null;
                bool sameAsRun = (currentMat != null && runMat != null && currentMat == runMat);

                if (!sameAsRun)
                {
                    int runLength = x - runStart;
                    if (runMat != null && runLength >= 4)
                    {
                        for (int rx = runStart; rx < x; rx++)
                        {
                            toClear[rx, y] = true;
                            foundAny = true;
                        }
                    }

                    runStart = x;
                    runMat = currentMat;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            int runStart = 0;
            Material runMat = GetMaterial(x, 0);

            for (int y = 1; y <= height; y++)
            {
                Material currentMat = (y < height) ? GetMaterial(x, y) : null;
                bool sameAsRun = (currentMat != null && runMat != null && currentMat == runMat);

                if (!sameAsRun)
                {
                    int runLength = y - runStart;
                    if (runMat != null && runLength >= 4)
                    {
                        for (int ry = runStart; ry < y; ry++)
                        {
                            toClear[x, ry] = true;
                            foundAny = true;
                        }
                    }

                    runStart = y;
                    runMat = currentMat;
                }
            }
        }
        // 2x2 square matches
for (int x = 0; x < width - 1; x++)
{
    for (int y = 0; y < height - 1; y++)
    {
        Material mat = GetMaterial(x, y);
        if (mat == null) continue;

        Material right = GetMaterial(x + 1, y);
        Material up = GetMaterial(x, y + 1);
        Material diagonal = GetMaterial(x + 1, y + 1);

        if (right == mat && up == mat && diagonal == mat)
        {
            toClear[x, y] = true;
            toClear[x + 1, y] = true;
            toClear[x, y + 1] = true;
            toClear[x + 1, y + 1] = true;
            foundAny = true;
        }
    }
}

        if (!foundAny)
            return false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (toClear[x, y] && grid[x, y] != null)
                {
                    clearedBlocks++;

                    Transform block = grid[x, y];
                    grid[x, y] = null;

                    StartCoroutine(PlayClearEffect(block));
                }
            }
        }

        if (clearedBlocks == 4)
        {
            ScoreManager.Instance.AddScore(10);
        }
        else if (clearedBlocks >= 5)
        {
            ScoreManager.Instance.AddScore(20);
        }
        AudioManager.Instance.PlayClear();
        return true;
    }

    IEnumerator PlayClearEffect(Transform block)
    {
        if (block == null) yield break;

        Vector3 originalPos = block.position;
        Vector3 originalScale = block.localScale;
        Renderer originalRenderer = block.GetComponent<Renderer>();
        Material originalMat = originalRenderer != null ? originalRenderer.sharedMaterial : null;

        // Quick punchy flash/pop before the block vanishes
        float popDuration = 0.08f;
        float t = 0f;
        Vector3 popScale = originalScale * 1.4f;

        while (t < popDuration)
        {
            t += Time.deltaTime;
            if (block == null) yield break;
            block.localScale = Vector3.Lerp(originalScale, popScale, t / popDuration);
            yield return null;
        }

        // Hide the original block and spawn fragments in its place
        if (block != null)
            block.gameObject.SetActive(false);

        SpawnFragments(originalPos, originalScale, originalMat);

        if (block != null)
            Destroy(block.gameObject, clearEffectDuration);
    }

    void SpawnFragments(Vector3 origin, Vector3 parentScale, Material mat)
    {
        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject frag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(frag.GetComponent<Collider>()); // no physics collisions needed

            frag.transform.position = origin;
            frag.transform.localScale = parentScale * 0.35f;

            Renderer fragRenderer = frag.GetComponent<Renderer>();
            if (fragRenderer != null && mat != null)
                fragRenderer.sharedMaterial = mat;

            // Random outward direction, biased slightly upward for a "pop" feel
            Vector3 dir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.3f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            StartCoroutine(AnimateFragment(frag.transform, dir));
        }
    }

    IEnumerator AnimateFragment(Transform frag, Vector3 direction)
    {
        Vector3 startPos = frag.position;
        Vector3 startScale = frag.localScale;
        float t = 0f;

        while (t < clearEffectDuration)
        {
            t += Time.deltaTime;
            if (frag == null) yield break;

            float p = t / clearEffectDuration;

            // Fly outward, decelerating, with slight downward drift (gravity feel)
            Vector3 offset = direction * explosionForce * p;
            offset.y -= p * p * 2f; // gravity pull

            frag.position = startPos + offset;
            frag.localScale = Vector3.Lerp(startScale, Vector3.zero, p);
            frag.Rotate(Vector3.one, 500f * Time.deltaTime); // tumbling spin

            yield return null;
        }

        if (frag != null)
            Destroy(frag.gameObject);
    }

    public void ApplyGravity()
    {
        for (int x = 0; x < width; x++)
        {
            CompactColumn(x);
        }
    }

    void CompactColumn(int x)
    {
        int writeY = 0;

        for (int y = 0; y < height; y++)
        {
            if (grid[x, y] != null)
            {
                Transform block = grid[x, y];

                if (y != writeY)
                {
                    grid[x, y] = null;
                    grid[x, writeY] = block;

                    block.position = new Vector3(
                        x * cellSize,
                        writeY * cellSize,
                        0
                    );
                }

                writeY++;
            }
        }
    }

    public bool TrySwap(int x1, int y1, int x2, int y2)
    {
        Transform blockA = grid[x1, y1];
        Transform blockB = grid[x2, y2];

        if (blockA == null || blockB == null)
            return false;

        grid[x1, y1] = blockB;
        grid[x2, y2] = blockA;

        blockA.position = new Vector3(x2 * cellSize, y2 * cellSize, 0);
        blockB.position = new Vector3(x1 * cellSize, y1 * cellSize, 0);

        bool createsMatch = HasMatchAt(x1, y1) || HasMatchAt(x2, y2);

        if (!createsMatch)
        {
            grid[x1, y1] = blockA;
            grid[x2, y2] = blockB;

            blockA.position = new Vector3(x1 * cellSize, y1 * cellSize, 0);
            blockB.position = new Vector3(x2 * cellSize, y2 * cellSize, 0);

            return false;
        }

        ResolveBoard();
        return true;
    }

    bool HasMatchAt(int x, int y)
    {
        Material mat = GetMaterial(x, y);
        if (mat == null) return false;

        int count = 1;
        int cx = x - 1;
        while (cx >= 0 && GetMaterial(cx, y) == mat) { count++; cx--; }
        cx = x + 1;
        while (cx < width && GetMaterial(cx, y) == mat) { count++; cx++; }
        if (count >= 4) return true;

        count = 1;
        int cy = y - 1;
        while (cy >= 0 && GetMaterial(x, cy) == mat) { count++; cy--; }
        cy = y + 1;
        while (cy < height && GetMaterial(x, cy) == mat) { count++; cy++; }
        if (count >= 4) return true;

        return false;
    }

    public void ResolveBoard()
    {
        ApplyGravity();

        bool matchFound;
        do
        {
            matchFound = ClearMatches();
            if (matchFound)
            {
                ApplyGravity();
            }
        } while (matchFound);
    }
}