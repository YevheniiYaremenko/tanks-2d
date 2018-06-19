using UnityEngine;

namespace Game.AI
{
    [RequireComponent(typeof(NavMeshAgent2D))]
    public abstract class Enemy : DamagingObject, IDamaging
    {
        [Header("Enemy")]
		[SerializeField] float damage = 100;
		[SerializeField] int killBonus = 10;
		Transform target;

        NavMeshAgent2D navigationAgent;
        Vector2 lastPosition = Vector2.zero;

		public int KillBonus { get { return killBonus; } }

        protected override void Awake()
        {
            base.Awake();
            navigationAgent = GetComponent<NavMeshAgent2D>();
        }

        public void SetData(Transform target)
        {
            this.target = target;
        }

        void Update()
		{
			if (target != null)
			{
                navigationAgent.SetDestination(target.position);
			}

            transform.up = ((Vector2)transform.position - lastPosition).normalized;
            lastPosition = transform.position;
		}

		void OnCollisionEnter2D(Collision2D col)
		{
            if (col.transform == target)
            {
                var damageHandler = col.transform.GetComponent<IDamaging>();
                if (damageHandler != null)
                {
                    damageHandler.DealDamage(damage);
                }

                Death();
            }
		}

        public override void Death()
        {
            navigationAgent.ResetPath();
            navigationAgent.enabled = false;
            base.Death();
        }
    }
}
