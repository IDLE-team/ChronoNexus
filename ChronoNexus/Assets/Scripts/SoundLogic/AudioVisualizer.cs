using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject cubePrefab;
    public int spectrumSize = 512;
    public float lineScale = 5f;
    public float lineWidth = 0.1f;
    public int numCubes = 10; // Количество кубиков для визуализации спектра
    public FFTWindow _type;

    private GameObject[] cubes;

    void Start()
    {
        cubes = new GameObject[numCubes];

        for (int i = 0; i < numCubes; i++)
        {
            // Создать кубик и установить его позицию
            GameObject cube = Instantiate(cubePrefab, transform);
            cube.transform.localPosition = new Vector3(i , 0f, 0f);
            cubes[i] = cube;
        }
    }

    void Update()
    {
        float[] spectrumData = new float[spectrumSize];
        audioSource.GetSpectrumData(spectrumData, 0, _type);

        int spectrumChunkSize = spectrumSize / numCubes;

        for (int i = 0; i < numCubes; i++)
        {
            // Определить начальный и конечный индексы для каждого кубика
            int startIndex = i * spectrumChunkSize;
            int endIndex = (i + 1) * spectrumChunkSize;

            // Получить среднюю амплитуду для этого участка спектра
            float averageAmplitude = 0f;
            for (int j = startIndex; j < endIndex; j++)
            {
                averageAmplitude += spectrumData[j];
            }
            averageAmplitude /= spectrumChunkSize;

            // Изменить масштаб кубика в зависимости от амплитуды
            cubes[i].transform.localScale = new Vector3(1f, averageAmplitude * lineScale, 1f);
        }
    }
}