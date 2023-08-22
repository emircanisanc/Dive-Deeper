using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    public override bool HandleFire(Transform cam)
    {
        if (!canFire)
        {
            return false;
        }
        if (!isShooting)
        {
            isShooting = true;
        }
        if (Time.timeSinceLevelLoad >= nextAttackTime)
        {
            /* PassiveMuzzle();
            ActiveMuzzle();
 */
            Vector3 startPos = cam.position;
            Vector3 dir = cam.forward + UnityEngine.Random.insideUnitSphere * sprayAmount;
            OnFire?.Invoke();
            SendLine(dir); // DEBUG LINE
            FireBullet(startPos, dir);
            nextAttackTime = Time.timeSinceLevelLoad + fireRate;
            return true;
        }
        return false;
    }
}
