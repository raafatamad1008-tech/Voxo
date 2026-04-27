using UnityEngine;
using System.Collections;

public class GridRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("عدد الثواني بين كل دورة")]
    public float rotationInterval = 5f; 
    
    [Tooltip("سرعة الدوران (كلما زاد الرقم كان الدوران أنعم)")]
    public float rotationSpeed = 5f;

    private float timer;
    private Quaternion targetRotation;

    void Start()
    {
        timer = rotationInterval;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            RotateGrid();
            timer = rotationInterval; // إعادة ضبط العداد
        }

        // تحريك الدوران بسلاسة نحو الزاوية المطلوبة
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void RotateGrid()
    {
        // حساب زاوية الدوران بناءً على عدد البوابات
        // إذا كان لديك 4 بوابات، الدوران سيكون 90 درجة (360 / 4)
        int gateCount = VoxoManager.Instance != null ? VoxoManager.Instance.gates.Length : 4;
        float angleStep = 360f / gateCount;

        // إضافة الزاوية الجديدة للزاوية الحالية
        targetRotation *= Quaternion.Euler(0, 0, angleStep);
        
        Debug.Log($"Rotating Grid by {angleStep} degrees. Next rotation in {rotationInterval}s");
    }
}