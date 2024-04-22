using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public float rotationSwitchTime = 2f; // Время, через которое объект меняет направление вращения
    public float movementSwitchTime = 2f; // Время, через которое объект меняет направление движения вверх-вниз
    public float slowDownFactor = 0.5f; // Коэффициент замедления
    public float movementSpeed = 1f; // Скорость движения вверх и вниз

    private float rotationTimer;
    private float movementTimer;
    private float movementDirection = 1f; // Направление движения (1 - вверх, -1 - вниз)

    private void Update()
    {
        rotationTimer += Time.deltaTime;
        movementTimer += Time.deltaTime;

        float rotationT = Mathf.Clamp01(rotationTimer / rotationSwitchTime); // Нормализованное время для вращения
        float movementT = Mathf.Clamp01(movementTimer / movementSwitchTime); // Нормализованное время для движения

        float rotationSlowDown = Mathf.Lerp(1f, slowDownFactor, rotationT); // Коэффициент замедления вращения
        transform.Rotate(rotationSpeed * rotationSlowDown * Time.deltaTime);

        float movementSlowDown = Mathf.Lerp(1f, slowDownFactor, movementT); // Коэффициент замедления движения
        transform.position += Vector3.up * movementSpeed * movementDirection * movementSlowDown * Time.deltaTime; // Движение вверх и вниз

        if (rotationT >= 1f)
        {
            // Меняет знак одной из осей вращения
            rotationSpeed.y *= -1;
            rotationTimer -= rotationSwitchTime;
        }

        if (movementT >= 1f)
        {
            // Меняет направление движения
            movementDirection *= -1;
            movementTimer -= movementSwitchTime;
        }
    }
}
