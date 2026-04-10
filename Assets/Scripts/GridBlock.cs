using UnityEngine;
using UnityEngine.EventSystems;

public class GridBlock : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Baþlangýç Koordinatý")]
    public int currentX;
    public int currentY;

    [Header("Olmasý Gereken (Doðru) Koordinat")]
    public int targetX;
    public int targetY;

    private GridPuzzle puzzleManager;
    private RectTransform rectTransform;
    private Vector2 startDragPos;

    public void Init(GridPuzzle manager, float width, float height)
    {
        puzzleManager = manager;
        rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(width, height);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void UpdateVisualPosition()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        float xPos = (currentX * rectTransform.sizeDelta.x) + (rectTransform.sizeDelta.x / 2f);
        float yPos = (currentY * rectTransform.sizeDelta.y) + (rectTransform.sizeDelta.y / 2f);

        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragVector = eventData.position - startDragPos;

        if (dragVector.magnitude < 30f) return;

        int dx = 0;
        int dy = 0;

        if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
        {
            dx = dragVector.x > 0 ? 1 : -1;
        }
        else
        {
            dy = dragVector.y > 0 ? 1 : -1;
        }

        int targetGridX = currentX + dx;
        int targetGridY = currentY + dy;

        if (targetGridX >= 0 && targetGridX < puzzleManager.xGridCount &&
            targetGridY >= 0 && targetGridY < puzzleManager.yGridCount)
        {
            GridBlock adjacentBlock = puzzleManager.GetBlockAt(targetGridX, targetGridY);
            if (adjacentBlock != null)
            {
                puzzleManager.SwapBlocks(this, adjacentBlock);
            }
        }
    }
}