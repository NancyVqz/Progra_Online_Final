using System.Collections;
using Fusion;
using UnityEngine;

public class Handgun : Weapon 
{
    //un Rpc es un protocolo para mandar a llamar un metodo en diferentes clientes. Rpc Sources es quien lo llama y el target es quien lo ejecuta
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public override void RpcRaycastShoot(RpcInfo info = default)
    {
        PlayerRef shooter = Object.InputAuthority;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, range, hitLayers))
        {
            Debug.Log("Le pegaste a: " + hit.collider.name);

            if (hit.collider.TryGetComponent(out Health health))
            {
                health.Rpc_TakeDamage(damage, shooter);
            }
            else
            {
                //Aparecer agujero de bala
                Debug.Log("No tienes componente de vida");
            }
        }
        currentAmmo--;
    }

    public override void RigidBodyShoot()
    {
        RpcPhysicalShoot(firePoint.position, firePoint.rotation);
    }

    //Un rpc se ejecuta 2 veces, la primera no hace como tal lo del metodo, sino que le manda al target la ejecucion del metodo
    // ya que el rpcTarget recibe el metodo, este lo invoca
    // ya cuando el RpcTarget invoca esto, en auto el RpcTaret rambien recibe info sobre el Rpc mismo.

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcPhysicalShoot(Vector3 pos, Quaternion rot, RpcInfo info = default)
    {
        if (bulletPrefab.IsValid)
        {
            NetworkObject bullet = Runner.Spawn(bulletPrefab, pos, rot, info.Source);

            currentAmmo--;
        }

    }

    public override void Reload()
    {
        if (!isReloaded && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadTime());
        }
    }

    private IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
        currentAmmo = maxAmmo;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCam.transform.position, playerCam.transform.forward * range);

    }
}
