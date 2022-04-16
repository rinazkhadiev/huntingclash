using System.Collections;
using UnityEngine;

public class Duck : Animal
{
    private Animation _animation;

    private void Start()
    {
        Health = 1;

        _animation = GetComponent<Animation>();
        Transform = GetComponent<Transform>();

        Transform.position = new Vector3(Random.Range(150, 350), 23.4f,
                                            Random.Range(150, 350));
        Transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);

    }

    public override void HealthChange()
    {
        Health -= 1;

        if (Health <= 0)
        {
            _animation.Play("Arm_duck|dead");
            StartCoroutine(FalseGameObject());
        }
    }

    IEnumerator FalseGameObject()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
