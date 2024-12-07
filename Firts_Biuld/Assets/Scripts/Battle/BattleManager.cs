using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // For coroutines

namespace Game
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private GameObject battleUI; // Battle UI (panel with buttons)
        [SerializeField] private Text battleLog; // Battle log
        [SerializeField] private Button attackButton; // Attack button
        [SerializeField] private Button runButton; // Run button
        [SerializeField] private Player player; // Player
        [SerializeField] private Enemy enemy; // Enemy

        private bool isPlayerTurn = true; // Is it the player's turn?
        public bool canFight = true; // Flag that controls whether fighting is allowed

        void Start()
        {
            battleUI.SetActive(false); // Hide battle UI by default
        }

        // Initialize battle
        public void StartBattle(Player currentPlayer, Enemy currentEnemy)
        {
            if (canFight)
            {
                player = currentPlayer;
                enemy = currentEnemy;

                canFight = false;

                battleUI.SetActive(true); // Show the battle UI
                battleLog.text = $"Battle started! Enemy: {enemy.name}\n"; // Log the battle start
                Time.timeScale = 0; // Pause the game

                // Activate attack and run buttons
                attackButton.interactable = true;
                runButton.interactable = true;
            }
        }

        // Player attack method
        public void PlayerAttack()
        {
            if (!isPlayerTurn)
            {
                return;
            }

            // Player attacks the enemy
            enemy.TakeDamage(player.damageAmount);

            battleLog.text = " ";

            battleLog.text += $"Player attacked the enemy for {player.damageAmount} damage. Enemy health {enemy.Health}\n";

            if (enemy.Health <= 0)
            {
                EndBattle(true); // End battle if enemy is defeated
                return;
            }

            isPlayerTurn = false;

            StartCoroutine(EnemyTurn()); // Call enemy's turn after player's action
        }

        // Player run method
        public void PlayerRun()
        {
            battleLog.text += "Player escaped the battle!\n";
            EndBattle(false); // End battle when player runs
        }

        public void PlayerGuard()
        {
            battleLog.text = " ";

            battleLog.text += $"Player guarded from attack {enemy.name}\n";

            player.defenceAttribute += 10;

            StartCoroutine(EnemyTurn());

            player.defenceAttribute -= 10;
        }

        // Enemy turn method (called via coroutine to handle actions)
        private IEnumerator EnemyTurn()
        {
            if (enemy.Health > 0)
            {
                // Enemy attacks the player
                player.TakeDamage(enemy.Damage);

                battleLog.text += $"Enemy attacked the player for {enemy.Damage} damage.\nHis resist is {player.defenceAttribute} --> Damage decrease to {enemy.Damage - player.defenceAttribute}.\nPlayer Health {player.CurrentHealth}\n";

                if (player.Health <= 0)
                {
                    EndBattle(false); // End battle if the player is defeated
                    yield break;
                }
            }

            isPlayerTurn = true; // Transfer turn back to the player
        }

        // End the battle
        private void EndBattle(bool playerWon)
        {
            if (playerWon)
            {
                battleLog.text += "Player won the battle!\n";
            }
            else
            {
                battleLog.text += "Player lost the battle.\n";
            }

            // Disable the buttons and UI
            attackButton.interactable = false;
            runButton.interactable = false;
            battleUI.SetActive(false); // Hide battle UI

            Time.timeScale = 1; // Resume game time

            StartCoroutine(canFightTimer());
        }

        private IEnumerator canFightTimer()
        {
            canFight = false;
            yield return new WaitForSeconds(3);
            canFight = true;
        }
    }
}