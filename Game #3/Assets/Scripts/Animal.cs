using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Transform Transform { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }
    public int Health { get; set; }
    public float Speed { get; set; }
    public float RunSpeed { get; set; }
    public bool IsAfraid { get; set; } = false;
    public virtual void HealthChange() { }
}

