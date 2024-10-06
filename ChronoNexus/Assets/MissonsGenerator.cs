using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissonsGenerator : MonoBehaviour
{
    [SerializeField] private List<LevelData> _allGameLevels = new List<LevelData>();

    [SerializeField] private int missions = 5;

    private List<LevelPin> _pins = new List<LevelPin>();

    private List<int> _valuesGenerated = new List<int>();

    private void Start()
    {
        _pins = gameObject.GetComponentsInChildren<LevelPin>().ToList();

        foreach (var item in _pins)
        {
            item.gameObject.SetActive(false);
        }

        GenerateMap();
    }
    public void GenerateMap()
    {
        _pins.Shuffle();
        for (int i = 0; i < missions; i++)
        {
            _pins[i].gameObject.SetActive(true);
            while (_valuesGenerated.Count< missions)
            {
                var num = Random.Range(0,_allGameLevels.Count);
                if (!_valuesGenerated.Contains(num))
                {
                    _valuesGenerated.Add(num);
                    _pins[i].SetLevelData(_allGameLevels[num]);
                    break;
                }
            }

        }

    }


    public void Shuffle(List<LevelPin> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
