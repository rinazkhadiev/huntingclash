using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    public static Dog Singleton { get; private set; }

    public GameObject CurrentAnimal { get; set; }

    private Animator _animator;
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    private void Start()
    {
        Singleton = this;
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        StartCoroutine(AddNavMesh());
    }

    private void Update()
    {
        if (CurrentAnimal != null)
        {
            if (Vector3.Distance(CurrentAnimal.transform.position, _transform.position) > 5 && _navMeshAgent != null)
            {
                _animator.Play("run");
                _navMeshAgent.speed = _runSpeed;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(CurrentAnimal.transform.position);
            }
            else if (Vector3.Distance(CurrentAnimal.transform.position, _transform.position) < 2 && _navMeshAgent != null)
            {
                _animator.Play("attack");
                _navMeshAgent.isStopped = true;
            }
        }
        else
        {

            if (Vector3.Distance(Character.Singleton.Transform.position, _transform.position) > 5 && Vector3.Distance(Character.Singleton.Transform.position, _transform.position) < 15 && _navMeshAgent != null)
            {
                _animator.Play("walk");
                _navMeshAgent.speed = _walkSpeed;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(Character.Singleton.Transform.position);
            }
            else if (Vector3.Distance(Character.Singleton.Transform.position, _transform.position) > 15 && Vector3.Distance(Character.Singleton.Transform.position, _transform.position) < 50 && _navMeshAgent != null)
            {
                _animator.Play("run");
                _navMeshAgent.speed = _runSpeed;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(Character.Singleton.Transform.position);
            }
            else if (Vector3.Distance(Character.Singleton.Transform.position, _transform.position) > 50)
            {
                if (_navMeshAgent != null)
                {
                    _navMeshAgent.Warp(new Vector3(Character.Singleton.Transform.position.x,
                                                   Character.Singleton.Transform.position.y,
                                                   Character.Singleton.Transform.position.z - 3));
                }
            }
            else if (Vector3.Distance(Character.Singleton.Transform.position, _transform.position) < 2 && _navMeshAgent != null)
            {
                _animator.Play("idle A");
                _navMeshAgent.isStopped = true;
            }
        }
    }
    IEnumerator AddNavMesh()
    {
        yield return new WaitForSeconds(5);
        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        _navMeshAgent.baseOffset = -0.1f;
    }
}
