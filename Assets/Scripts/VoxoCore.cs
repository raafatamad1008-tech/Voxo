using UnityEngine;
using System.Collections.Generic;

public class VoxoCore : MonoBehaviour
{
    public static VoxoCore Instance;

    [Header("References")]
    public GameObject squarePrefab;
    public Transform gridParent; // اسحب كائن GridParent هنا
    public Transform[] gates;    // اسحب الـ 4 بوابات هنا

    [Header("Settings")]
    public int dimension = 5;
    public float rotationInterval = 5f; // التحكم في ثواني الدوران
    public float rotationSpeed = 10f;

    private float timer;
    private Quaternion targetRotation;

    void Awake() { Instance = this; }

    void Start() {
        timer = rotationInterval;
        targetRotation = gridParent.rotation;
        GenerateLevel();
    }

    void Update() {
        // منطق عداد الدوران
        timer -= Time.deltaTime;
        if (timer <= 0) {
            RotateGrid();
            timer = rotationInterval;
        }

        // تنعيم الدوران
        gridParent.rotation = Quaternion.Lerp(gridParent.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void GenerateLevel() {
        foreach (Transform child in gridParent) Destroy(child.gameObject);

        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        float maxArea = Mathf.Min(screenWidth, screenHeight) * 0.65f;
        float cellSize = maxArea / dimension;

        for (int x = 0; x < dimension; x++) {
            for (int y = 0; y < dimension; y++) {
                float posX = (x * cellSize) - (maxArea / 2f) + (cellSize / 2f);
                float posY = (y * cellSize) - (maxArea / 2f) + (cellSize / 2f);

                GameObject go = Instantiate(squarePrefab, gridParent); // توليد داخل الـ Parent
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localScale = Vector3.one * (cellSize / 1f) * 0.9f;

                // إضافة سكريبت المربع وربطه ببوابة عشوائية
                VoxoSquare sq = go.AddComponent<VoxoSquare>();
                sq.targetGate = gates[Random.Range(0, gates.Length)];
            }
        }
    }

    void RotateGrid() {
        float angleStep = 360f / gates.Length;
        targetRotation *= Quaternion.Euler(0, 0, angleStep);
    }
}