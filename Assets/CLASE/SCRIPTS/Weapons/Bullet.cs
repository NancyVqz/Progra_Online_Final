using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private int lifeTime = 2;
    //[SerializeField] private int damage = 50;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * FinalSpeed());
        DespawnAfterTime();
    }

    private float FinalSpeed()
    {
        return bulletSpeed * 3;
    }

    private async void DespawnAfterTime()
    {
        await Task.Delay(lifeTime * 1000);
        if(Object != null)
        {
            Runner.Despawn(this.Object);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Runner.Despawn(this.Object);
        }
    }
}
