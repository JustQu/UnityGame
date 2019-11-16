using UnityEngine;
using System.Collections;

public class ProjectileShootTriggerable : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D projectile;                           
    public Transform bulletSpawn;                        
    [HideInInspector] public float projectileForce = 250f;

    public void Launch(Vector2 dir)
    {
        Rigidbody2D clonedBullet = Instantiate(projectile, bulletSpawn.position + new Vector3(dir.x, dir.y), transform.rotation) as Rigidbody2D;

        clonedBullet.AddForce(dir * projectileForce);
    }
}
