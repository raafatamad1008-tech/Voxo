


using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Assets")]
    public GameObject squarePrefab;
    public Sprite[] squareSprites; // اسحب كل صور المربعات الملونة هنا

    [Header("Settings")]
    [Range(0.1f, 0.9f)] public float screenUsage = 0.65f; // نسبة الاستحواذ على الشاشة

    private int currentDimension;

    void Start()
    {
        // تجربة توليد شبكة عشوائية عند البداية
        GenerateLevel(Random.Range(3, 9));
    }

    public void GenerateLevel(int dimension)
    {
        ClearOldGrid();
        currentDimension = dimension;



        // 1. حساب أبعاد الكاميرا
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // 2. تحديد حجم الشبكة (بناءً على العرض أو الارتفاع أيهما أصغر لضمان عدم الخروج من الشاشة)
        float maxAvailableSize = Mathf.Min(screenWidth, screenHeight) * screenUsage;
        float cellSize = maxAvailableSize / dimension;

        for (int x = 0; x < dimension; x++)
        {
            for (int y = 0; y < dimension; y++)
            {
                // 3. حساب الموقع (التوسيط الرياضي)
                // نبدأ من (-نصف الحجم الكلي) ونضيف نصف حجم المربع لنكون في المركز بالضبط
                float posX = (x * cellSize) - (maxAvailableSize / 2f) + (cellSize / 2f);
                float posY = (y * cellSize) - (maxAvailableSize / 2f) + (cellSize / 2f);

                GameObject newSquare = Instantiate(squarePrefab, transform);
                // داخل حلقة الـ Loop في GenerateLevel
                // ... الكود السابق للحجم والموقع ...

                Square squareScript = newSquare.AddComponent<Square>();

                // اختيار اتجاه عشوائي (أعلى، أسفل، يمين، يسار)
                Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
                // squareScript.SetDirection(directions[Random.Range(0, directions.Length)]);
                newSquare.transform.localPosition = new Vector3(posX, posY, 0);

                // 4. جعل الحجم متناسباً
                SpriteRenderer sr = newSquare.GetComponent<SpriteRenderer>();
                if (sr != null && squareSprites.Length > 0)
                {
                    // اختيار شكل/لون عشوائي من المصفوفة
                    sr.sprite = squareSprites[Random.Range(0, squareSprites.Length)];

                    float spriteSize = sr.sprite.bounds.size.x;
                    float scaleFactor = cellSize / spriteSize;
                    newSquare.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1) * 0.92f;
                }
            }
        }
    }

    void ClearOldGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}