using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
    private bool _forRestart;
    private int _partNumber;

    private bool _rewardError;
    private float _rewardTimer = 10;

    [SerializeField] private GameObject _errorText;
    [SerializeField] private Button _moneyPlusButton;
    [SerializeField] private GameObject _getLicence;

    [SerializeField] private AnalyticsEventManager _analyticsEventManager;

    private void Start()
    {
#if UNITY_ANDROID
        string appKey = "13c04bc69";
#else
        string appKey = "unexpected_platform";
#endif


        IronSource.Agent.init(appKey);
        StartCoroutine(InterstitialLoad());

        if (SceneManager.GetActiveScene().name == "Main")
        {
            StartCoroutine(ErrorTextShow(10));
        }
    }

    private void OnEnable()
    {
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        StartCoroutine(InterstitialLoad());
    }

    void InterstitialAdClosedEvent()
    {
        if (_forRestart)
        {
            if (SceneManager.GetActiveScene().name == "Main")
            {
                _getLicence.SetActive(false);
                SceneLoader.Singleton.SceneStartToLoading(_partNumber);
            }
            else
            {
                AllObjects.Singleton.AnalyticsEvent.OnEvent("ContinuedOrRestarted");
                AllObjects.Singleton.AnalyticsEvent.OnEvent("RestartFail");
                SceneManager.LoadScene("Play");
            }
        }
        else
        {
            AllObjects.Singleton.AnalyticsEvent.OnEvent("GoToMenu");
            SceneManager.LoadScene("Main");
        }
        _forRestart = false;
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
        ShopManager.Singleton.MoneyText.text = $"{PlayerPrefs.GetInt("Money")}$";
        ShopManager.Singleton.SellBuyAudio.PlayOneShot(ShopManager.Singleton.SellBuyAudio.clip);

        AllObjects.Singleton.AnalyticsEvent.OnEvent("MoneyPlus");
    }

    public void Restart()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            _forRestart = true;
            IronSource.Agent.showInterstitial();
        }
        else
        {
            AllObjects.Singleton.AnalyticsEvent.OnEvent("ContinuedOrRestarted");
            AllObjects.Singleton.AnalyticsEvent.OnEvent("RestartFail");
            SceneManager.LoadScene("Play");
        }
    }

    public void GetLicence(int PartNumber)
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            _forRestart = true;
            _partNumber = PartNumber;
            StartCoroutine(GetLicenceInterstitial());
        }
        else
        {
            SceneLoader.Singleton.SceneStartToLoading(PartNumber);
        }
    }

    public void MoneyPlus()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            StartCoroutine(ErrorTextShow(5));
            _analyticsEventManager.OnEvent("MoneyPlusError");
        }
    }

    public void GoToMenu()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            AllObjects.Singleton.AnalyticsEvent.OnEvent("GoToMenuFail");
            SceneManager.LoadScene("Main");
        }
    }

    private void Update()
    {
        if (_rewardError)
        {
            _rewardTimer -= Time.deltaTime;
            _errorText.GetComponent<Text>().text = $"{(int)_rewardTimer}";
        }
    }

    IEnumerator ErrorTextShow(int time)
    {
        _moneyPlusButton.interactable = false;
        _rewardError = true;
        _errorText.SetActive(true);
        yield return new WaitForSeconds(time);

        _moneyPlusButton.interactable = true;
        _rewardError = false;
        _rewardTimer = 5;
        _errorText.SetActive(false);
    }

    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }

    IEnumerator InterstitialLoad()
    {
        yield return new WaitForSeconds(5);
        IronSource.Agent.loadInterstitial();
    }

    IEnumerator GetLicenceInterstitial()
    {
        _getLicence.SetActive(true);
        yield return new WaitForSeconds(3);
        IronSource.Agent.showInterstitial();
    }
}
