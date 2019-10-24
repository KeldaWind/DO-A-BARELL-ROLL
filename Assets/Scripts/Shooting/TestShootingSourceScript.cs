using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootingSourceScript : MonoBehaviour
{
    [SerializeField] ShootingSystem shootingSystem = default;

    private void Start()
    {
        shootingSystem.SetUp(DamageTag.Environment, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            shootingSystem.OnInputPressed();
        else if (Input.GetKey(KeyCode.T))
            shootingSystem.OnInputMaintained();
        else if (Input.GetKeyUp(KeyCode.T))
            shootingSystem.OnInputReleased();

        shootingSystem.UpdateSystem();
    }
}
