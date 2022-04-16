using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyAnGun : MonoBehaviour
{
    [SerializeField] private int _price;

    [SerializeField] private Text _priceText;

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name) == 1)
        {
            if (PlayerPrefs.GetString("Language") == "en")
            {
                _priceText.text = "Bought";
            }
            else
            {
                _priceText.text = "Куплено";
            }

            _priceText.color = Color.red;
        }
        else if (PlayerPrefs.GetInt(gameObject.name) == 2)
        {
            if (PlayerPrefs.GetString("Language") == "en")
            {
                _priceText.text = "Chosen";
            }
            else
            {
                _priceText.text = "Выбрано";
            }

            _priceText.color = Color.white;
        }
    }

    public void Buy()
    {
        if (PlayerPrefs.GetInt(gameObject.name) == 0)
        {
            if (PlayerPrefs.GetInt("Money") >= _price)
            {
                ShopManager.Singleton.SellBuyAudio.PlayOneShot(ShopManager.Singleton.SellBuyAudio.clip);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _price);
                ShopManager.Singleton.MoneyText.text = $"{PlayerPrefs.GetInt("Money")}$";

                Chosen();
            }
            else
            {
                ShopManager.Singleton.ErrorSellBuyAudio.PlayOneShot(ShopManager.Singleton.ErrorSellBuyAudio.clip);
                StartCoroutine(ErrorBuy());
            }
        }
        else
        {
            Chosen();
            ShopManager.Singleton.SelectAudio.PlayOneShot(ShopManager.Singleton.SelectAudio.clip);
        }
    }

    IEnumerator ErrorBuy()
    {
        ShopManager.Singleton.MoneyText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        ShopManager.Singleton.MoneyText.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        ShopManager.Singleton.MoneyText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        ShopManager.Singleton.MoneyText.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        ShopManager.Singleton.MoneyText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        ShopManager.Singleton.MoneyText.color = Color.green;
    }

    private void Chosen()
    {
        if (gameObject.name != "Dude")
        {
            for (int i = 0; i < ShopManager.Singleton.Guns.Length; i++)
            {
                if (PlayerPrefs.GetInt($"{ShopManager.Singleton.Guns[i].name}") == 2)
                {
                    PlayerPrefs.SetInt($"{ShopManager.Singleton.Guns[i].name}", 1);
                }
            }
        }

        PlayerPrefs.SetInt($"{gameObject.name}", 2);

        for (int i = 0; i < ShopManager.Singleton.Guns.Length; i++)
        {
            if (PlayerPrefs.GetInt($"{ShopManager.Singleton.Guns[i].name}") == 1)
            {
                if (PlayerPrefs.GetString("Language") == "en")
                {
                    ShopManager.Singleton.Guns[i].GetComponentInChildren<Text>().text = "Bought";
                }
                else
                {
                    ShopManager.Singleton.Guns[i].GetComponentInChildren<Text>().text = "Куплено";
                }

                ShopManager.Singleton.Guns[i].GetComponentInChildren<Text>().color = Color.red;
            }
        }

        if (PlayerPrefs.GetString("Language") == "en")
        {
            _priceText.text = "Chosen";
        }
        else
        {
            _priceText.text = "Выбрано";
        }

        _priceText.color = Color.white;
    }
}
