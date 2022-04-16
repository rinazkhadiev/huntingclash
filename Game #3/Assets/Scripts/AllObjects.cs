using UnityEngine;
using UnityEngine.UI;

public class AllObjects : MonoBehaviour
{
    [Header("Scripts")]
    public JoyStickController JoyController;
    public AnalyticsEventManager AnalyticsEvent;

    [Header("World")]
    public GameObject[] PartGameObject;

    [Header("GameObjects")]
    public GameObject ShootPoint;
    public GameObject[] GunObjects;
    public GameObject IsDeadModel;
    public AnimalBear[] AllAnimals;
    public GameObject DangerAnimal;

    [Header("UI")]
    public GameObject DeadZoneVinet;
    public Text OutWorldCountText;
    public Slider SensitivityBar;
    public GameObject PauseMenu;
    public GameObject BagTransform;
    public GameObject AimOnButton;
    public GameObject AimOffButton;
    public GameObject LoseImage;
    public GameObject[] DeadUI;
    public Button DudeButton;

    public Text DangerText;

    [Header("Audio")]
    public AudioSource StepAudio;
    public AudioSource MainSoruce;
    public AudioClip FailClip;

    public AudioClip[] StepsClipsFirst;
    public AudioClip[] StepsClipsSecond;

    public AudioSource DangerAuido;



    public static AllObjects Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        for (int i = 0; i < PartGameObject.Length; i++)
        {
            if (i != PlayerPrefs.GetInt("Part"))
            {
                PartGameObject[i].SetActive(false);
            }
        }
    }
}
