using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    [SerializeField] Animator weapon;
    [SerializeField] CameraShake cameraShake;

    private void Start()
    {
        weapon.SetBool("boolStrongAttack", false);
        weapon.SetBool("boolLightAttack", false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LightAttack());
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(StrongAttack());
        }
    }
    public IEnumerator StrongAttack()
    {
        weapon.SetBool("boolStrongAttack", true);
        weapon.SetTrigger("Strong");
        yield return new WaitForSeconds(2f);
        weapon.SetBool("boolStrongAttack", false);
    }
    IEnumerator LightAttack()
    {
        weapon.SetBool("boolLightAttack", true);
        weapon.SetTrigger("Light");
        yield return new WaitForSeconds(1f);
        weapon.SetBool("boolLightAttack", false);
    }
}
