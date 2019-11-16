using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    public float projectileForce = 500f;
    public Rigidbody2D projectile;
    private ProjectileShootTriggerable launcher;

    public override void Initialize(GameObject obj)
    {
        launcher = obj.GetComponent<ProjectileShootTriggerable>();
        launcher.projectileForce = projectileForce;
        launcher.projectile = projectile;
    }

    public override void TriggerAbility(Vector2 dir)
    {
        launcher.Launch(dir);
    }

}
