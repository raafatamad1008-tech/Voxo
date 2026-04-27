using UnityEngine;

public class Square : MonoBehaviour
{
    public Transform targetGate; 
    private bool isMoving = false;
    private Vector3 moveDirection; // تم تغييرها لـ Vector3 لمنع التعارض
    public float speed = 15f;

    void Start()
    {
        // التأكد من وجود كولايدر برمجياً لضمان عمل النقر
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    public void SetDirection(Vector2 dir)
{
    moveDirection = dir;

    // نبحث عن السهم في الأبناء (Children) باسمه
    Transform arrowTransform = transform.Find("Arrow"); 

    if (arrowTransform != null)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    else
    {
        Debug.LogWarning("لم يتم العثور على GameObject باسم Arrow داخل المربع!");
    }
}

    void OnMouseDown()
    {
        if (!isMoving && targetGate != null)
        {
            // حساب الاتجاه باستخدام Vector3 بالكامل
            moveDirection = (targetGate.position - transform.position).normalized;
            isMoving = true;
            
            // إلغاء تفعيل الكولايدر حتى لا يصطدم به شيء أثناء حركته
            if(GetComponent<Collider2D>()) GetComponent<Collider2D>().enabled = false;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // الآن الجمع صريح وواضح للمحرك (Vector3 + Vector3)
            transform.position += moveDirection * speed * Time.deltaTime;

            // تدمير المربع عند الابتعاد عن المركز بمسافة كبيرة (للتنظيف)
            if (Vector3.Distance(transform.position, Vector3.zero) > 15f)
            {
                Destroy(gameObject);
            }
        }
    }
}