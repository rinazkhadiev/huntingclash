using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _value;
    [SerializeField] private float _speed;

    private float _distation;
    private Vector3 _startPos;
    private Vector3 _rotation = Vector3.zero;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _startPos = transform.position;
    }

    private void Update()
    {
        if (!Shoot.Singleton.SlowMove && !Character.Singleton.IsDead)
        {
            _distation += (transform.position - _startPos).magnitude;
            _startPos = transform.position;
            _rotation.z = Mathf.Sin(_distation * _speed) * _value;
            _transform.eulerAngles = _rotation + _transform.eulerAngles;
        }
    }
}