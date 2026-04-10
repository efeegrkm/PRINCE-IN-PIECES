using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GridPuzzle : MonoBehaviour
{
    [Header("Puzzle Boyutlarý")]
    public int xGridCount = 3;
    public int yGridCount = 3;

    [Header("Harita Parçalarý")]
    [Tooltip("Tüm GridBlock objelerini (harita parçalarýný) buraya sürükleyin")]
    public List<GridBlock> blocks;

    [Header("Görsel Ayarlar (Grid Çizgileri)")]
    public Color gridLineColor = new Color(0, 0, 0, 0.5f); 
    public float gridLineThickness = 4f; 

    private RectTransform puzzleRect;
    private float blockWidth;
    private float blockHeight;
    private GameObject linesContainer;

    void OnEnable()
    {
        InitializePuzzle();
    }

    private void InitializePuzzle()
    {
        int expectedBlockCount = xGridCount * yGridCount;

        if (blocks.Count != expectedBlockCount)
        {
            Debug.LogError($"[GridPuzzle] HATA: Blok sayýsý uyuþmuyor! {xGridCount}x{yGridCount} grid için {expectedBlockCount} blok olmalý, ancak Inspector'da {blocks.Count} blok atadýnýz.");
            return;
        }

        puzzleRect = GetComponent<RectTransform>();

        blockWidth = puzzleRect.rect.width / xGridCount;
        blockHeight = puzzleRect.rect.height / yGridCount;

        foreach (var block in blocks)
        {
            block.Init(this, blockWidth, blockHeight);
            block.UpdateVisualPosition();
        }
        CreateGridLines();
    }
    private void CreateGridLines()
    {
        if (linesContainer != null) Destroy(linesContainer);

        linesContainer = new GameObject("GridLines");
        linesContainer.transform.SetParent(puzzleRect, false);

        RectTransform containerRt = linesContainer.AddComponent<RectTransform>();
        containerRt.anchorMin = Vector2.zero;
        containerRt.anchorMax = Vector2.one;
        containerRt.offsetMin = Vector2.zero;
        containerRt.offsetMax = Vector2.zero;

        for (int i = 1; i < xGridCount; i++)
        {
            CreateLine(new Vector2(i * blockWidth, puzzleRect.rect.height / 2f), new Vector2(gridLineThickness, puzzleRect.rect.height), "V_Line_" + i);
        }

        for (int i = 1; i < yGridCount; i++)
        {
            CreateLine(new Vector2(puzzleRect.rect.width / 2f, i * blockHeight), new Vector2(puzzleRect.rect.width, gridLineThickness), "H_Line_" + i);
        }

        linesContainer.transform.SetAsLastSibling();
    }

    private void CreateLine(Vector2 pos, Vector2 size, string lineName)
    {
        GameObject lineObj = new GameObject(lineName);
        lineObj.transform.SetParent(linesContainer.transform, false);

        Image lineImg = lineObj.AddComponent<Image>();
        lineImg.color = gridLineColor;

        lineImg.raycastTarget = false;

        RectTransform rt = lineObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = size;
        rt.anchoredPosition = pos;
    }

    public void SwapBlocks(GridBlock blockA, GridBlock blockB)
    {
        int tempX = blockA.currentX;
        int tempY = blockA.currentY;

        blockA.currentX = blockB.currentX;
        blockA.currentY = blockB.currentY;

        blockB.currentX = tempX;
        blockB.currentY = tempY;

        blockA.UpdateVisualPosition();
        blockB.UpdateVisualPosition();

        CheckWinCondition();
    }

    public GridBlock GetBlockAt(int x, int y)
    {
        foreach (var block in blocks)
        {
            if (block.currentX == x && block.currentY == y)
                return block;
        }
        return null;
    }

    private void CheckWinCondition()
    {
        foreach (var block in blocks)
        {
            if (block.currentX != block.targetX || block.currentY != block.targetY)
            {
                return;
            }
        }

        FinishedPuzzleSuccessfully();
    }

    private void FinishedPuzzleSuccessfully()
    {

        if (linesContainer != null)
        {
            linesContainer.SetActive(false);
        }

        // gameObject.SetActive(false); 
    }
}