using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    private float machineCont;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.shoot && ScoreManager.instance.canShoot)
            {
                if (currentWeapon.shootMode == ShootMode.Physical)
                {
                    currentWeapon.RigidBodyShoot();
                }
                else
                {
                    currentWeapon.RpcRaycastShoot();
                }
            }
        }


        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentWeapon.Reload();
        }

    }
}
