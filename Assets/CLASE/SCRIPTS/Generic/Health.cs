using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int health; //Tendremos una variable local de vida

    [Networked] public int _health { get; set; }

    public override void Spawned()
    {
        _health = health;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)] //Con esto cualquiera puede recibir daño pero solo el host lo ejecuta porque si en el target ponga All, entonces el objetivo recibira daño de todos lados.
    public void Rpc_TakeDamage(int damage, PlayerRef shooter)
    {
        ScoreManager.instance.Rpc_UpdateScore(shooter);

        //_health -= damage;
        //Debug.Log($"{name} recibio daño de {shooter}. Vida actual: {_health}");

        //if (_health <= 0)
        //{
        //    ScoreManager.instance.Rpc_UpdateScore(shooter);
        //}
    }

    private void OnDeath()
    {
        this.gameObject.SetActive(false);
    }
}
