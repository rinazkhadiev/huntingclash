using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBear : Animal
{
    public bool _isDude { get; set; }

    [SerializeField] private GameObject _takeButton;
    [SerializeField] private bool _hunter;
    [SerializeField] private GameObject _bagImage;

    private int _distanceStandUp = 20;
    private int _distanceSitDown = 5;

    private void Start()
    {
        Health = Random.Range(2, 4);
        Speed = 0.5f;
        RunSpeed = 1.5f;

        Transform = GetComponent<Transform>();
        Animator = GetComponent<Animator>();
        
        do
        {
            Transform.position = new Vector3(Random.Range(-60, 70), PlayerPrefs.GetInt("Part") * 100, Random.Range(-70, 70));
            Transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0));
        } while (Vector3.Distance(Character.Singleton.Transform.position, Transform.position) < 50);

        StartCoroutine(AddNavMesh());
    }

    private void Update()
    {
        if (NavMeshAgent != null)
        {
            if (Health > 0)
            {
                if (!Character.Singleton.IsDead)
                {
                    if (!_isDude)
                    {
                        if (Character.Singleton.IsSideDown)
                        {
                            if (_hunter)
                            {
                                if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceSitDown && !IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                                    Animator.Play("walk");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceSitDown && IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                    Animator.Play("run");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceSitDown && NavMeshAgent != null)
                                {
                                    NavMeshAgent.isStopped = false;
                                    IsAfraid = false;
                                    NavMeshAgent.SetDestination(Character.Singleton.gameObject.transform.position);
                                    Animator.Play("run");
                                }
                            }
                            else
                            {
                                if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceSitDown && !IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                                    Animator.Play("walk");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceSitDown || IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                    Animator.Play("run");
                                }
                            }
                        }
                        else
                        {
                            if (_hunter)
                            {
                                if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && !IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                                    Animator.Play("walk");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                    Animator.Play("run");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceStandUp && NavMeshAgent != null)
                                {
                                    NavMeshAgent.isStopped = false;
                                    IsAfraid = false;
                                    NavMeshAgent.SetDestination(Character.Singleton.gameObject.transform.position);
                                    Animator.Play("run");
                                }
                            }
                            else
                            {
                                if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && !IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                                    Animator.Play("walk");
                                }

                                else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceStandUp || IsAfraid)
                                {
                                    NavMeshAgent.isStopped = true;
                                    Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                    Animator.Play("run");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_hunter)
                        {
                            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && !IsAfraid)
                            {
                                NavMeshAgent.isStopped = false;
                                NavMeshAgent.SetDestination(Character.Singleton.gameObject.transform.position);
                                Animator.Play("run");
                            }

                            else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && IsAfraid)
                            {
                                NavMeshAgent.isStopped = true;
                                Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                Animator.Play("run");
                            }

                            else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceStandUp && NavMeshAgent != null)
                            {
                                NavMeshAgent.isStopped = false;
                                IsAfraid = false;
                                NavMeshAgent.SetDestination(Character.Singleton.gameObject.transform.position);
                                Animator.Play("run");
                            }
                        }
                        else
                        {
                            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) > _distanceStandUp && !IsAfraid)
                            {
                                NavMeshAgent.isStopped = false;
                                NavMeshAgent.SetDestination(Character.Singleton.gameObject.transform.position);
                                Animator.Play("run");
                            }

                            else if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < _distanceStandUp || IsAfraid)
                            {
                                NavMeshAgent.isStopped = true;
                                Transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
                                Animator.Play("run");
                            }
                        }
                    }

                    if (_hunter)
                    {
                        if (AllObjects.Singleton.DangerAnimal == null)
                        {
                            AllObjects.Singleton.DangerAuido.mute = true;
                            AllObjects.Singleton.DangerText.gameObject.SetActive(false);

                            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < 40)
                            {
                                AllObjects.Singleton.DangerAnimal = gameObject;
                            }
                        }
                        else
                        {
                            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, AllObjects.Singleton.DangerAnimal.transform.position) < 40)
                            {
                                AllObjects.Singleton.DangerAuido.mute = false;
                                AllObjects.Singleton.DangerAuido.volume = 0.5f - (Vector3.Distance(Character.Singleton.gameObject.transform.position, AllObjects.Singleton.DangerAnimal.transform.position) / 100);
                                
                                AllObjects.Singleton.DangerText.gameObject.SetActive(true);
                                AllObjects.Singleton.DangerText.text = $"! {(int)Vector3.Distance(Character.Singleton.gameObject.transform.position, AllObjects.Singleton.DangerAnimal.transform.position)}m !";
                            }
                            else
                            {
                                AllObjects.Singleton.DangerAnimal = null;
                            }
                        }

                        if (Vector3.Distance(Character.Singleton.gameObject.transform.position, Transform.position) < 3 && !Character.Singleton.IsDead)
                        {
                            AllObjects.Singleton.MainSoruce.PlayOneShot(AllObjects.Singleton.FailClip);
                            Character.Singleton.IsDead = true;
                            AllObjects.Singleton.LoseImage.SetActive(true);

                            for (int i = 0; i < AllObjects.Singleton.GunObjects.Length; i++)
                            {
                                AllObjects.Singleton.GunObjects[i].SetActive(false);
                            }

                            for (int i = 0; i < AllObjects.Singleton.DeadUI.Length; i++)
                            {
                                AllObjects.Singleton.DeadUI[i].SetActive(false);
                            }

                            AllObjects.Singleton.IsDeadModel.SetActive(true);
                        }
                    }
                }
                else
                {
                    NavMeshAgent.isStopped = true;
                    Transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                    Animator.Play("walk");
                }
            }
            else if (Health <= 0 && gameObject.activeSelf)
            {
                if (Vector3.Distance(Character.Singleton.gameObject.transform.position, transform.position) < 3)
                {
                    _takeButton.SetActive(true);
                }
                else
                {
                    _takeButton.SetActive(false);
                }
            }
        }

        if(Transform.position.x < -100 || Transform.position.x > 100 || Transform.position.z > 100 || Transform.position.z < -100)
        {
            do
            {
                Transform.position = new Vector3(Random.Range(-60, 70), PlayerPrefs.GetInt("Part") * 100, Random.Range(-70, 70));
                Transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0));

                StartCoroutine(AnimalSetActive());
            } while (Vector3.Distance(Character.Singleton.Transform.position, Transform.position) < 50);
        }
    }

    public override void HealthChange()
    {
        Health -= 1;
        IsAfraid = true;

        if (Health <= 0)
        {
            NavMeshAgent.isStopped = true;
            Animator.Play("die");
            AllObjects.Singleton.DangerAnimal = null;
        }
    }

    public void Take()
    {
        Instantiate(_bagImage, AllObjects.Singleton.BagTransform.transform);
        _takeButton.SetActive(false);
	    Health = Random.Range(2, 4);
        IsAfraid = false;
        PlayerPrefs.SetInt($"{_bagImage.gameObject.name}", PlayerPrefs.GetInt($"{_bagImage.gameObject.name}") + 1);

        do
        {
            Transform.position = new Vector3(Random.Range(-60, 70), PlayerPrefs.GetInt("Part") * 100, Random.Range(-70, 70));
            Transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0));

            StartCoroutine(AnimalSetActive());
        } while (Vector3.Distance(Character.Singleton.Transform.position, Transform.position) < 50);
    }

    

    IEnumerator AddNavMesh()
    {
        yield return new WaitForSeconds(5);
        NavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        NavMeshAgent.baseOffset = -0.1f;

        if (_hunter)
        {
            NavMeshAgent.speed = 2.5f;
        }
        else
        {
            NavMeshAgent.speed = 2;
        }
    }

    IEnumerator AnimalSetActive()
    {
        Transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(5);
        Transform.localScale = Vector3.one;
    }
}
