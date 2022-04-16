using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Singleton { get; private set; }

    public Text MoneyText;

    public AudioSource SellBuyAudio;
    public AudioSource SelectAudio;
    public AudioSource ErrorSellBuyAudio;

    public GameObject[] Guns;

    [SerializeField] private GameObject[] _animalObject;

    [SerializeField] private Text[] _howMuchTexts;

    [SerializeField] private Text[] _sellCountText;

    private void Start()
    {
        Singleton = this;
        MoneyText.text = $"{PlayerPrefs.GetInt("Money")}$";

        for (int i = 0; i < _howMuchTexts.Length; i++)
        {
            _howMuchTexts[i].text = PlayerPrefs.GetInt($"{_animalObject[i].name}").ToString();
        }

        for (int i = 0; i < _sellCountText.Length; i++)
        {
            _sellCountText[i].text = $"{int.Parse(_howMuchTexts[i].text) * 10}$";
        }

        if(PlayerPrefs.GetInt($"{Guns[1].name}") == 0)
        {
            PlayerPrefs.SetInt($"{Guns[0].name}", 2);
        }
    }
}