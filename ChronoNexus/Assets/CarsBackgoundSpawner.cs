using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarsBackgoundSpawner : MonoBehaviour
{
    [SerializeField] private Transform _pointToStart;
    [SerializeField] private Transform _pointToEnd;
    [SerializeField] private Transform _teleportPoint;
    [SerializeField] private Image _fade;
    [SerializeField] private float _angleY;

    [Range(1, 10)]
    [SerializeField] private int _secondsBeforeSpawn;

    [Range(20, 50)]
    [SerializeField] private int _carMoveSpeed;

    [SerializeField] private List<GameObject> _carsToMoveOnLevel;

    private int iterCar=0;

    private void Start()
    {
        Invoke("ResetCar", 0);
    }

    private void ResetCar()
    {
        _carsToMoveOnLevel[iterCar].transform.position = _pointToStart.position;
        _carsToMoveOnLevel[iterCar].GetComponent<CarMovableObject>().InstallPointOnCar(_pointToEnd.position, _carMoveSpeed, _teleportPoint, _fade);

        iterCar++;
        if (iterCar >= _carsToMoveOnLevel.Count) iterCar = 0;

        StartCoroutine(TimeOutBeforeMove());
    }

    IEnumerator TimeOutBeforeMove()
    {
        yield return new WaitForSeconds(_secondsBeforeSpawn);
        ResetCar();
    }
}
