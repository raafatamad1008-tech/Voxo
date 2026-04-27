using UnityEngine;

public class VoxoManager : MonoBehaviour
{
    public static VoxoManager Instance;

    [Header("Level Settings")]
    public GameObject squarePrefab;
    public Transform[] gates; // اسحب الـ 4 بوابات هنا في الـ Inspector
    public int dimension = 5;

    void Awake() { Instance = this; }

    void Start() {
        GenerateLevel();
    }

    public void GenerateLevel() {
        // كود التوليد السابق مع ربط كل مربع ببوابة
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        float maxAvailableSize = Mathf.Min(screenWidth, screenHeight) * 0.65f;
        float cellSize = maxAvailableSize / dimension;

        for (int x = 0; x < dimension; x++) {
            for (int y = 0; y < dimension; y++) {
                float posX = (x * cellSize) - (maxAvailableSize / 2f) + (cellSize / 2f);
                float posY = (y * cellSize) - (maxAvailableSize / 2f) + (cellSize / 2f);

                GameObject go = Instantiate(squarePrefab, transform);
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localScale = Vector3.one * (cellSize / 1f) * 0.9f;

                // إعطاء كل مربع مرجع للبوابة التي يجب أن يذهب إليها
                VoxoSquare squareScript = go.GetComponent<VoxoSquare>();
                squareScript.targetGate = gates[Random.Range(0, gates.Length)];
            }
        }
    }
}