using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private GameObject map;
    [SerializeField] private GameObject pauseScreen;

    public GameObject puzzle1;
    public GameObject puzzle2;
    public GameObject puzzle3;
    public GameObject puzzle4;
    public GameObject puzzle5;
    public GameObject puzzle6;

    public GameObject balina3D;
    public GameObject balina2D;
    public GameObject goblinPrinces;


    public int bodyCount = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleMap()
    {
        if (GameManager.Instance.CurrentState == GameState.Exploring)
        {
            GameManager.Instance.ChangeState(GameState.Map);
            openMap();
        }
        else if (GameManager.Instance.CurrentState == GameState.Map)
        {
            GameManager.Instance.ChangeState(GameState.Exploring);
            closeMap();
        }
    }

    public void TogglePause()
    {
        if (GameManager.Instance.CurrentState == GameState.Exploring)
        {
            GameManager.Instance.ChangeState(GameState.Paused);
            pause();
        }
        else if (GameManager.Instance.CurrentState == GameState.Paused)
        {
            GameManager.Instance.ChangeState(GameState.Exploring);
            resume();
        }
    }

    public void openMap()
    {
        map.SetActive(true);
    }
    public void closeMap()
    {
        map.SetActive(false);   
    }
    public void pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f; 
    }
    public void resume()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }
    public void testDialogue1()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("TestSpeaker1", "Merhaba, haritadaki son koordinatları kontrol ettin mi?"),
            new DialogueLine("TestSpeaker2", "Evet, radar sistemini yeni güncelledim. Oraya gitmemiz biraz tehlikeli olabilir."),
            new DialogueLine("TestSpeaker3", "O zaman dikkatli olmalıyız.")
        };

        DialogueManager.Instance.StartDialogue(conversation);
    }

    public void whaleOn1()
    {
        WhaleManager.Instance.EnterWhaleMode();
    }

    public void restrictedKraken()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Whale", "Krakene tükürürürüm seni, yolunu değiştir."),
            new DialogueLine("Prenses", "...")
        };

        DialogueManager.Instance.StartDialogue(conversation);
        GameManager.Instance.ChangeState(GameState.OnWhale);
    }

    public void startIsland2()
    {
        balina3D.transform.position = new Vector3(-0.24f, 0.53f, -25.81f);
        goblinPrinces.transform.position = new Vector3(-5, 4.02f, -16.94f);
        Physics.SyncTransforms();
        balina2D.GetComponent<RectTransform>().anchoredPosition = new Vector2(69f, -155f);
        GameManager.Instance.ChangeState(GameState.Exploring);
    }
    public void startIsland3()
    {
        balina3D.transform.position = new Vector3(3.42f, -5.07f, -31.48f);
        goblinPrinces.transform.position = new Vector3(17.19f, 3.81f, -11.8f);
        Physics.SyncTransforms();
        balina2D.GetComponent<RectTransform>().anchoredPosition = new Vector2(329f, 85f);
        GameManager.Instance.ChangeState(GameState.Exploring);
    }
    public void startIsland4()
    {
        balina3D.transform.position = new Vector3(11.04f, 0.59f, 9.07f);
        goblinPrinces.transform.position = new Vector3(14.9f, 3.89f, 13.25f);
        Physics.SyncTransforms();
        balina2D.GetComponent<RectTransform>().anchoredPosition = new Vector2(243f, 293f);
        GameManager.Instance.ChangeState(GameState.Exploring);
    }
    public void startIsland5()
    {
        goblinPrinces.transform.position = new Vector3(-8.87f, 1.77f, 17.52f);
        Physics.SyncTransforms();
        GameManager.Instance.ChangeState(GameState.Exploring);
    }

    public void interactFirstMap()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Prenses", "Ayyy noluyo noluyo gözüme perde indi."),
            new DialogueLine("Hayalet", "Radikal feminist goblin prensesi!"),
            new DialogueLine("Hayalet", "Ben sevgili kocacığın goblin prens."),
            new DialogueLine("Hayalet", "İnsanlar beni parçalara ayırdı."),
            new DialogueLine("Hayalet", "Bizden asırlardır korkuyorlar su boyunda balina beslerken tek düşürdüler."),
            new DialogueLine("Hayalet", "Neden mi sana dokunmadılar? Radikal feminizmin onları kaçırdı sanırım."),
            new DialogueLine("Hayalet", "Beni diriltmek için birtek sana güvenebilirim. Bedenimin parçalarını beni kaçırdıkları adaya dağıttılar onları bul."),
            new DialogueLine("Hayalet", "Al sana bu adanın harita parçaları..."),
            new DialogueLine("Hayalet", "Sonrakileri kendin bulman gerekecek."),
            new DialogueLine("Prenses", "OFF. Ama çok işş"),
            new DialogueLine("Hayalet", "Sen bu işi slaylersin.")
        };

        DialogueManager.Instance.StartDialogue(conversation, () =>
        {
            puzzle1.SetActive(true);
        });
    }
    public void interactSecondMap()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Prenses", "Ikinci harita kalıntılarını da buldum..."),
            new DialogueLine("Hayalet", "Aferin iyi ilerliyorsun.")
        };

        DialogueManager.Instance.StartDialogue(conversation, () =>
        {
            puzzle2.SetActive(true);
        });
    }

    public void interactThirdMap()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Prenses", "Üçüncü harita kalıntılarını da buldum..."),
            new DialogueLine("Hayalet", "Çok az kaldı son adaya vardığında benim parçalarımı birleştirebileceğin bir çember bulacaksın.")
        };

        DialogueManager.Instance.StartDialogue(conversation, () =>
        {
            puzzle3.SetActive(true);
        });
    }

    public void interactFourthMap()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Prenses", "Dördüncü harita kalıntılarını da buldum...")
        };

        DialogueManager.Instance.StartDialogue(conversation, () =>
        {
            puzzle4.SetActive(true);
        });
    }

    public void interactFifthMap()
    {
        List<DialogueLine> conversation = new List<DialogueLine>
        {
            new DialogueLine("Prenses", "Bu da son harita olmalı."),
            new DialogueLine("Prenses", "Bebeğimin parçalarını toplamak için son adaya son bir iğrenc yolculuk."),
            new DialogueLine("Hayalet", "Az kaldı seni bekliyorum canım.")
        };

        DialogueManager.Instance.StartDialogue(conversation, () =>
        {
            puzzle5.SetActive(true);
        });
    }

    public void interactCatacoumb()
    {
        if (bodyCount == 0)
        {
            List<DialogueLine> conversation1 = new List<DialogueLine>
            {
                new DialogueLine("Prenses", "Sanırım ceset parçalarını buraya toplamalıyım.")
            };
            DialogueManager.Instance.StartDialogue(conversation1);
        }
        if (bodyCount < 9)
        {
            string message = "9 parça içinden " + bodyCount + " parça buldum.";
            List<DialogueLine> conversation2 = new List<DialogueLine>
            {
                new DialogueLine("Prenses", message),
                new DialogueLine("Prenses", "Bekle beni aşkitom."),
            };
            DialogueManager.Instance.StartDialogue(conversation2);
            return;
        }

        List<DialogueLine> conversation = new List<DialogueLine>
            {
                new DialogueLine("Prenses", "Uzun bekleyişin sonu geldi. Umarım yeterince hızlı olurum."),
                new DialogueLine("Prenses", "Bekle beni bebeğim."),
            };
        DialogueManager.Instance.StartDialogue(conversation);
        puzzle6.SetActive(true);
    }
    public void addBodyCount()
    {
        bodyCount++;
        List<DialogueLine> conversation = new List<DialogueLine>
            {
                new DialogueLine("Prenses", bodyCount + ". parçayı buldum.")
            };
        DialogueManager.Instance.StartDialogue(conversation);
    }
}