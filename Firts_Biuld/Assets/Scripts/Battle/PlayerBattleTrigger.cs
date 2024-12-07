using UnityEngine;

namespace Game
{
    public class PlayerBattleTrigger : MonoBehaviour
    {
        [SerializeField] private float battleRange = 2.0f; // Радиус обнаружения врага
        private Player player; // Ссылка на игрока
        private BattleManager battleManager;

        void Start()
        {
            battleManager = FindObjectOfType<BattleManager>(); // Находим BattleManager в сцене
            player = GetComponent<Player>();
        }

        void Update()
        {
            CheckForEnemies();
        }

        private void CheckForEnemies()
        {
            if (!battleManager.canFight) return; // Если нельзя сражаться, не запускаем бой

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, battleRange);
            foreach (var hit in hitEnemies)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        battleManager.StartBattle(player, enemy);
                        return;
                    }
                }
            }
        }
    }
}