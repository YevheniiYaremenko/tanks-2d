public interface IDamageHandler
{
	float Health { get; }
	void DealDamage(float damage);
	void Death();
    event System.Action onDeath;
}
