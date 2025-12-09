using UnityEngine;
using Fusion;

public abstract class Weapon : NetworkBehaviour
{
    public ShootMode shootMode = ShootMode.Raycast;
    [SerializeField] protected NetworkPrefabRef bulletPrefab;
    [SerializeField] protected NetworkRunner runner;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Camera playerCam;
    [SerializeField] protected LayerMask hitLayers;

    [SerializeField] protected int damage;
    [SerializeField] protected float range = 10f;
    [SerializeField] protected int currentAmmo = 30;
    [SerializeField] protected int maxAmmo = 30;
    [SerializeField] protected float fireRate = 0.1f;
    [SerializeField] protected float reloadTime = 2f;

    public bool isReloaded;

    private void Start()
    {
        runner = FindAnyObjectByType<NetworkRunner>();
    }

    public bool CanShoot()
    {
        return currentAmmo > 0;
    }

    public abstract void RpcRaycastShoot(RpcInfo info = default);
    public abstract void RigidBodyShoot();
    public abstract void Reload();

}
