using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public static Character Singleton { get; private set; }
    public bool IsSideDown { get; set; }
    public bool IsDead { get; set; }
    public Transform Transform { get; private set; }
    public Vector3 SpawnPosition { get; set; }

    [SerializeField] private float _stepTime;
    [SerializeField] private float _playerSpeed = 2.0f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;

    private CharacterController _charController;
    private Transform _cameraTransform;
    private float _outWorldCount = 5;
    private bool _isStepping;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private bool _isJumping;
    private bool _deadZone;
    private bool _standUpping;

    private float _dudeButtonWaiting;
    private bool _dudeIsWaiting;

    private void Awake()
    {
        Singleton = this;
        Transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _charController = GetComponent<CharacterController>();

        Transform.position = new Vector3(Transform.position.x, (PlayerPrefs.GetInt("Part") * 100) + 2, Transform.position.z);

        if (PlayerPrefs.HasKey("Dude"))
        {
            if(PlayerPrefs.GetInt("Dude") == 1 || PlayerPrefs.GetInt("Dude") == 2)
            {
                AllObjects.Singleton.DudeButton.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        _groundedPlayer = _charController.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        _charController.Move(AllObjects.Singleton.JoyController.Horizontal() * _cameraTransform.right * Time.deltaTime * _playerSpeed);
        _charController.Move(AllObjects.Singleton.JoyController.Vertical() * _cameraTransform.forward * Time.deltaTime * _playerSpeed);

        if (!_isStepping && !_isJumping)
        {
            if (_charController.velocity.magnitude > 4f)
            {
                _stepTime = 0.5f;
                StartCoroutine(Step());
            }
            else if (_charController.velocity.magnitude > 2f && _charController.velocity.magnitude < 4f)
            {
                _stepTime = 0.8f;
                StartCoroutine(Step());
            }
            else if (_charController.velocity.magnitude < 2f && _charController.velocity.magnitude > 0.1f)
            {
                _stepTime = 1.5f;
                StartCoroutine(Step());
            }
        }

        if (_isJumping && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _charController.Move(_playerVelocity * Time.deltaTime);

        if (!_groundedPlayer && Transform.position.y < -10f + (PlayerPrefs.GetInt("Part") * 100))
        {
            Transform.position = new Vector3(Transform.position.x, (PlayerPrefs.GetInt("Part") * 100) + 2, Transform.position.z);
        }

        if (_deadZone)
        {
            Transform.position = Vector3.MoveTowards(Transform.position, new Vector3(0, AllObjects.Singleton.PartGameObject[PlayerPrefs.GetInt("Part")].transform.position.y, 0), 1f);
            Transform.position = new Vector3(Transform.position.x, AllObjects.Singleton.PartGameObject[PlayerPrefs.GetInt("Part")].transform.position.y + 2, Transform.position.z);
        }

        if(_standUpping)
        {
            if(Transform.position.y < 2.9f + (PlayerPrefs.GetInt("Part") * 100))
            {
                Transform.position = new Vector3(Transform.position.x, 3 + (PlayerPrefs.GetInt("Part") * 100), Transform.position.z);
            }
            else
            {
                _standUpping = false;
            }
        }

        if (_dudeIsWaiting)
        {
            _dudeButtonWaiting += Time.deltaTime;
            AllObjects.Singleton.DudeButton.GetComponent<Image>().fillAmount = _dudeButtonWaiting / 25;
        }
    }

    public void Jump()
    {
        StartCoroutine(JumpWait());
    }

    IEnumerator JumpWait()
    {
        _isJumping = true;
        yield return new WaitForSeconds(0.1f);
        _isJumping = false;
    }

    IEnumerator Step()
    {
        _isStepping = true;
        
        switch (PlayerPrefs.GetInt("Part"))
        {
            case 0:
                AllObjects.Singleton.StepAudio.PlayOneShot(AllObjects.Singleton.StepsClipsFirst[Random.Range(0, AllObjects.Singleton.StepsClipsFirst.Length)]);
                break;

            case 1:
                AllObjects.Singleton.StepAudio.PlayOneShot(AllObjects.Singleton.StepsClipsSecond[Random.Range(0, AllObjects.Singleton.StepsClipsSecond.Length)]);
                break;

            default:
                AllObjects.Singleton.StepAudio.PlayOneShot(AllObjects.Singleton.StepsClipsFirst[Random.Range(0, AllObjects.Singleton.StepsClipsFirst.Length)]);
                break;
        }

       yield return new WaitForSeconds(_stepTime);
        _isStepping = false;
    }


    public void SideDown()
    {
        IsSideDown = true;
        _charController.height = 1;
        _playerSpeed = 4;
    }

    public void StandUp()
    {
        IsSideDown = false;
        _charController.height = 2;
        _playerSpeed = 5;
        _standUpping = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            AllObjects.Singleton.DeadZoneVinet.SetActive(true);

            if (_outWorldCount > 0)
            {
                AllObjects.Singleton.OutWorldCountText.gameObject.SetActive(true);
                _outWorldCount -= Time.deltaTime;
                AllObjects.Singleton.OutWorldCountText.text = $"{(int)_outWorldCount}";
            }
            else
            {
                _deadZone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            AllObjects.Singleton.DeadZoneVinet.SetActive(false);
            AllObjects.Singleton.OutWorldCountText.gameObject.SetActive(false);
            _outWorldCount = 5;
            _deadZone = false;
        }
    }

    public void Dude()
    {
        StartCoroutine(DudeWait());
    }

    IEnumerator DudeWait()
    {
        for (int i = 0; i < AllObjects.Singleton.AllAnimals.Length; i++)
        {
            if (Random.Range(0, 3) == 1)
            {
                AllObjects.Singleton.AllAnimals[i]._isDude = true;
            }
        }

        AllObjects.Singleton.DudeButton.interactable = false;
        _dudeIsWaiting = true;

        yield return new WaitForSeconds(25);

        _dudeIsWaiting = false;
        _dudeButtonWaiting = 0;

        AllObjects.Singleton.DudeButton.interactable = true;

        for (int i = 0; i < AllObjects.Singleton.AllAnimals.Length; i++)
        {
            AllObjects.Singleton.AllAnimals[i]._isDude = false;
        }
    }
}
