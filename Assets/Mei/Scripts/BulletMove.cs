using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Transform targetPos;
    public float bulletSpeed = 10f;
    public float bulletDamage = 5f;

    void Update()
    {
        Debug.Log("1");
        if (targetPos == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos.position,
            bulletSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        BugLife bug = targetPos.GetComponent<BugLife>();
        if (bug != null)
            bug.TakeDamage(bulletDamage);

        Destroy(gameObject);
    }
}
