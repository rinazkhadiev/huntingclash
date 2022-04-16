using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SellAnAnimal : MonoBehaviour
{
    [SerializeField] private Text _howMuchText;
    [SerializeField] private GameObject _animalObject;

    private int _sellCount;

    private void Start()
    {
        _sellCount = int.Parse(_howMuchText.text) * 10;
    }

    public void Sell()
    {
        if (int.Parse(_howMuchText.text) > 0)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _sellCount);
            ShopManager.Singleton.MoneyText.text = $"{PlayerPrefs.GetInt("Money")}$";
            PlayerPrefs.SetInt($"{_animalObject.name}", 0);

            _howMuchText.text = "0";
            GetComponentInChildren<Text>().text = "0$";
            _sellCount = 0;

            ShopManager.Singleton.SellBuyAudio.PlayOneShot(ShopManager.Singleton.SellBuyAudio.clip);
        }
        else
        {
            ShopManager.Singleton.ErrorSellBuyAudio.PlayOneShot(ShopManager.Singleton.ErrorSellBuyAudio.clip);
            StartCoroutine(ErrorSell());
        }
    }

    IEnumerator ErrorSell()
    {
        _howMuchText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _howMuchText.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        _howMuchText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _howMuchText.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        _howMuchText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _howMuchText.color = Color.green;
    }
    
}
