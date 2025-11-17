using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootCD = 1f;
    private float shootTimer;
    private Transform currentTarget;

    void Update()
    {
        if (currentTarget != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootCD)
            {
                shootTimer = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        BulletMove newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletMove>();
        newBullet.targetPos = currentTarget;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("2");
        if (currentTarget == null && collision.CompareTag("bug"))
        {
            currentTarget = collision.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    public void ClearTarget(Transform bugTransform)
    {
        if (currentTarget == bugTransform)
            currentTarget = null;
    }
}
