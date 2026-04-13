using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WhaleManager : MonoBehaviour
{
    public static WhaleManager Instance { get; private set; }

    [Header("UI Referansları")]
    public RectTransform parentCanvasRect; // Ana Canvas
    public RectTransform mapRect;          // Harita Image'�
    public RectTransform whaleRect;        // Balina Image'�
    public Rigidbody2D whaleRb;            // Balinan�n Fizi�i

    [Header("Ayarlar")]
    public float moveSpeed = 400f;         // Balinan�n UI �zerindeki h�z�

    private Vector3 originalMapScale;
    private Vector2 originalMapPosition;
    private bool isWhaleMode = false;
    private Vector2 moveInput;

    [Header("Balina Konumları")]
    public List<Transform> whaleLocations;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnterWhaleMode()
    {
        // 1. Eski durumu kaydet
        mapRect.gameObject.SetActive(true);
        GameManager.Instance.ChangeState(GameState.Map);
        //Blackout ekle zaman kal�rsa.
        originalMapScale = mapRect.localScale;
        originalMapPosition = mapRect.anchoredPosition;

        // 2. Haritay� ekran�n geni�li�ine g�re Scale et (Fit to Width)
        float targetScale = parentCanvasRect.rect.width / mapRect.rect.width;
        mapRect.localScale = new Vector3(targetScale, targetScale, 1f);

        // 3. Haritay� merkeze al
        mapRect.anchoredPosition = Vector2.zero;

        // 4. Sistemi ve fizi�i a�
        isWhaleMode = true;
        whaleRb.simulated = true;
        GameManager.Instance.ChangeState(GameState.OnWhale);
    }

    // Bu metodu balina modundan ��karken �a��r
    public void ExitWhaleMode()
    {
        isWhaleMode = false;

        // Fizi�i ve h�z� durdur
        whaleRb.simulated = false;
        whaleRb.linearVelocity = Vector2.zero; // (Unity 6 kulland���n i�in linearVelocity, eskiyse velocity yaz)

        // Haritay� eski konum ve boyutuna geri getir
        mapRect.localScale = originalMapScale;
        mapRect.anchoredPosition = originalMapPosition;

        GameManager.Instance.ChangeState(GameState.Map);
    }

    private void Update()
    {
        if (!isWhaleMode) return;

        // Diyalog a��l�rsa (veya Pause edilirse) balina oldu�u yerde dursun
        if (GameManager.Instance.CurrentState != GameState.OnWhale)
        {
            if (whaleRb.linearVelocity != Vector2.zero) whaleRb.linearVelocity = Vector2.zero;
            return;
        }

        GetInput();

        HandleWhaleRotation();

        FollowWhaleVertical();
    }

    private void FixedUpdate()
    {
        if (!isWhaleMode || GameManager.Instance.CurrentState != GameState.OnWhale) return;

        // Balinay� Rigidbody ile hareket ettir (Duvarlara �arpabilmesi i�in bu �artt�r)
        // Harita b�y�d��� i�in h�z� mapRect.localScale.x ile �arp�yoruz ki h�z tutarl� kals�n
        whaleRb.linearVelocity = moveInput * moveSpeed * mapRect.localScale.x;
    }

    private void GetInput()
    {
        moveInput = Vector2.zero;
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1;

        moveInput = moveInput.normalized;
    }

    private void HandleWhaleRotation()
    { 
        if (moveInput.x < 0)
        {
            whaleRect.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x > 0)
        {
            whaleRect.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FollowWhaleVertical()
    {
        float whaleLocalY = whaleRect.localPosition.y;
        float targetMapY = -whaleLocalY * mapRect.localScale.y;

        float scaledMapHeight = mapRect.rect.height * mapRect.localScale.y;
        float canvasHeight = parentCanvasRect.rect.height;

        // Ta�ma pay�n� hesapla
        float maxY = Mathf.Max(0, (scaledMapHeight - canvasHeight) / 2f);
        float minY = -maxY;

        targetMapY = Mathf.Clamp(targetMapY, minY, maxY);

        // Haritay� yumu�ak bir �ekilde kayd�r (X ekseninde hep tam ortada kal�r)
        Vector2 currentPos = mapRect.anchoredPosition;
        currentPos.y = Mathf.Lerp(currentPos.y, targetMapY, Time.deltaTime * 10f);
        currentPos.x = 0;

        mapRect.anchoredPosition = currentPos;
    }
}