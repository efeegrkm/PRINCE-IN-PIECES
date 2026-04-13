using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class WhaleInteractionZone : MonoBehaviour
{
    [Header("Etkileşim Ayarları")]
    [Tooltip("True ise balina çarpar çarpmaz tetiklenir. False ise yanına gidip E'ye basmak gerekir.")]
    public bool isAutoTrigger = true;

    [Tooltip("Sınır bölgesi ise balinayı içeri sokmamak için geri itsin mi?")]
    public bool applyPushback = true;
    public float pushbackForce = 150f;

    [Header("Tetiklenecek Olaylar (Inspector'dan Atayın)")]
    public UnityEvent OnInteractEvent;

    private bool isPlayerInZone = false;
    private Rigidbody2D playerRb;

    private void Update()
    {
        // E�er E ile etkile�im modundaysak, oyuncu b�lgedeyse ve oyun Balina modundaysa E tu�unu dinle
        if (!isAutoTrigger && isPlayerInZone && GameManager.Instance.CurrentState == GameState.OnWhale)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                ExecuteInteraction();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �arpan �ey balina m� ve balina modunda m�y�z?
        if (other.CompareTag("Player") && GameManager.Instance.CurrentState == GameState.OnWhale)
        {
            isPlayerInZone = true;
            playerRb = other.GetComponent<Rigidbody2D>();

            // E�er otomatik tetiklenme a��ksa beklemeden �al��t�r
            if (isAutoTrigger)
            {
                ExecuteInteraction();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            playerRb = null;
        }
    }

    private void ExecuteInteraction()
    {
        // Geri itme a��ksa balinay� geldi�i y�ne do�ru it
        if (applyPushback && playerRb != null)
        {
            Vector2 pushDir = (playerRb.transform.position - transform.position).normalized;
            playerRb.AddForce(pushDir * pushbackForce, ForceMode2D.Impulse);
        }

        // Inspector'dan ba�lanan t�m olaylar� (Diyalog, sahne ge�i�i, ses �alma vs.) tetikle
        OnInteractEvent?.Invoke();
    }
}