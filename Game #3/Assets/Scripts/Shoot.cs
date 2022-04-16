using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public static Shoot Singleton { get; private set; }

    [SerializeField] private GameObject[] _gunObjects;

    [SerializeField] private GameObject _aimUI;
    [SerializeField] private GameObject _bulletPoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem _shootVFX;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private Text _bulletValueText;
    [SerializeField] private Slider _aimPlusSlider;
    [SerializeField] private Button _shootButton;

    [SerializeField] private string[] _gunNames;
    private int _currentGun;


    public GameObject BulletClone { get; private set; }

    public bool SlowMove { get; private set; }

    private RaycastHit _hit;
    private Camera _camera;
    private int _bulletSpeed = 100;
    private int _bulletValue = 2;
    private int _bulletValueConst = 2;
    private bool _aiming;
    private bool _bulletOn;

    private void Start()
    {
        Singleton = this;
        _camera = Camera.main;
        _bulletOn = true;

        for (int i = 0; i < _gunNames.Length; i++)
        {
            if(PlayerPrefs.GetInt(_gunNames[i]) == 2)
            {
                _currentGun = i;
            }
        }

        for (int i = 0; i < _gunObjects.Length; i++)
        {
            _gunObjects[i].SetActive(false);
        }

        if (_currentGun == 0)
        {
            _bulletValueConst = 2;
            _bulletValue = _bulletValueConst;
        }
        else if (_currentGun == 1)
        {
            _bulletValueConst = 5;
            _bulletValue = _bulletValueConst;
        }

        _gunObjects[_currentGun].SetActive(true);
        _bulletValueText.text = _bulletValue + $"/{_bulletValueConst}";
    }

    public void StartShoot()
    {
        if (_bulletValue > 0)
        {
            if (!SlowMove)
            {
                if (Physics.Raycast(AllObjects.Singleton.ShootPoint.transform.position, AllObjects.Singleton.ShootPoint.transform.forward, out _hit))
                {
                    if (_hit.collider.GetComponentInParent<Animal>())
                    {
                        _hit.collider.GetComponentInParent<Animal>().HealthChange();
                        Dog.Singleton.CurrentAnimal = _hit.collider.gameObject;

                        if (_hit.collider.GetComponentInParent<Animal>().Health > 0)
                        {
                            SlowMove = true;
                            _bulletOn = true;
                        }
                        else
                        {
                            _bulletOn = false;
                            Dog.Singleton.CurrentAnimal = null;
                        }
                    }
                }

                _shootVFX.Play();
                _shootSound.Play();

                Vector3 startPos = _bulletPoint.transform.position;

                if (_bulletOn)
                {  
                    BulletClone = Instantiate(_bullet, startPos, transform.rotation);
                    BulletClone.GetComponent<Rigidbody>().AddForce(transform.forward * _bulletSpeed, ForceMode.Impulse);
                    Destroy(BulletClone, 2.5f);
                }

                _bulletValue -= 1;
                _bulletValueText.text = _bulletValue + $"/{_bulletValueConst}";
            }
            else
            {
                SlowMove = false;
                Time.timeScale = 1;
                GunSetActive();
            }
        }
        else
        {
            StartCoroutine(ReloadWait());
        }
    }

    private void Update()
    {
        if (_aiming)
        {
            _camera.fieldOfView = _aimPlusSlider.value;

            AllObjects.Singleton.SensitivityBar.maxValue = 50;
            if(AllObjects.Singleton.SensitivityBar.value < 50f)
            {
                AllObjects.Singleton.SensitivityBar.value = 6 + ((_aimPlusSlider.maxValue * 1.4f) - _aimPlusSlider.value);
            }
            else
            {
                AllObjects.Singleton.SensitivityBar.value = 50f;
            }
        }
        else
        {
            AllObjects.Singleton.SensitivityBar.maxValue = 10;
            if (PlayerPrefs.HasKey("Sens"))
            {
                AllObjects.Singleton.SensitivityBar.value = PlayerPrefs.GetFloat("Sens");
            }
            else
            {
                AllObjects.Singleton.SensitivityBar.value = 6f;
            }
        }

        if (SlowMove)
        {
            Time.timeScale = 0.2f;
            StartCoroutine(SlowMoveWait());
            AimingOff();

            for (int i = 0; i < AllObjects.Singleton.GunObjects.Length; i++)
            {
                AllObjects.Singleton.GunObjects[i].SetActive(false);
            }

            if (Vector3.Distance(_hit.collider.GetComponentInParent<Animal>().Transform.position, BulletClone.transform.position) < 5 && BulletClone != null)
            {
                SlowMove = false;
                Time.timeScale = 1;
                GunSetActive();
            }
        }
    }

    public void Aiming()
    {
        _aimUI.SetActive(true);
        _aiming = true;

        AllObjects.Singleton.AimOffButton.SetActive(true);
        AllObjects.Singleton.AimOnButton.SetActive(false);

        for (int i = 0; i < AllObjects.Singleton.GunObjects.Length; i++)
        {
            AllObjects.Singleton.GunObjects[i].SetActive(false);
        }
    }
    
    public void AimingOff()
    {
        _aimUI.SetActive(false);
        _camera.fieldOfView = 60;
        _aiming = false;

        AllObjects.Singleton.AimOffButton.SetActive(false);
        AllObjects.Singleton.AimOnButton.SetActive(true);
        GunSetActive();
    }

    private void GunSetActive()
    {
        AllObjects.Singleton.GunObjects[0].SetActive(true);

        _gunObjects[_currentGun].SetActive(true);
    }

    public void Reload()
    {
        if (_bulletValue < _bulletValueConst)
        {
            StartCoroutine(ReloadWait());
        }
    }

    public IEnumerator ReloadWait()
    {
        _reloadSound.Play();
        _shootButton.interactable = false;
        yield return new WaitForSeconds(1.7f);
        _bulletValue = _bulletValueConst;
        _bulletValueText.text = _bulletValue + $"/{_bulletValueConst}";
        _shootButton.interactable = true;
    }

    public IEnumerator SlowMoveWait()
    {
        yield return new WaitForSeconds(2);
        if (SlowMove)
        {
            SlowMove = false;
            Time.timeScale = 1;
            GunSetActive();
        }
    }
}