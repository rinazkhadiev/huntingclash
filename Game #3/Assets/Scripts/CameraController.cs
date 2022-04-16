using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IDragHandler
{
    [SerializeField] private float _sensitivity = 6f;

    private float _moveX;
    private float _moveY;
    private Transform _cameraTransform;
    private float _deadYPosition = 3;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;

        if (PlayerPrefs.HasKey("Sens"))
        {
            AllObjects.Singleton.SensitivityBar.value = PlayerPrefs.GetFloat("Sens");
        }
        else
        {
            AllObjects.Singleton.SensitivityBar.value = 6f;
        }

        _deadYPosition = (PlayerPrefs.GetInt("Part") * 100);
        _moveX = 245f;
    }

    private void Update()
    {
        _sensitivity = AllObjects.Singleton.SensitivityBar.value;

        if (AllObjects.Singleton.PauseMenu.activeSelf)
        {
            PlayerPrefs.SetFloat("Sens", AllObjects.Singleton.SensitivityBar.value);
        }

        if (Shoot.Singleton.SlowMove)
        {
            _cameraTransform.transform.position = Shoot.Singleton.BulletClone.transform.position;
        }
        else
        {
            if (!Character.Singleton.IsDead)
            {
                _cameraTransform.position = Character.Singleton.Transform.position;
                _cameraTransform.rotation = Quaternion.Euler(_moveY, _moveX, 0);
                Character.Singleton.Transform.rotation = Quaternion.Euler(new Vector3(0, _moveX, 0));
            }
            else
            {
                _moveY = 90;
                _cameraTransform.rotation = Quaternion.Euler(_moveY, _moveX, 0);
                _deadYPosition +=  Time.deltaTime;
                _cameraTransform.position = new Vector3(_cameraTransform.position.x, _deadYPosition, _cameraTransform.position.z);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Character.Singleton.IsDead)
        {
            _moveY -= eventData.delta.y / _sensitivity;
            _moveY = Mathf.Clamp(_moveY, -40, 40);

            _moveX += eventData.delta.x / _sensitivity;
            if (_moveX < -360) _moveX += 360;
            if (_moveX > 360) _moveX -= 360;
            _moveX = Mathf.Clamp(_moveX, -360, 360);
        }
    }
}