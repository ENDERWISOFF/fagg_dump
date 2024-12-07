using UnityEngine;
using Pathfinding;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Enemy_Move : MonoBehaviour
{
    private float range;
    [SerializeField] private GameObject target; // Цель (игрок)
    private Seeker seeker; // Компонент для расчета пути
    private Path path; // Текущий путь
    private int currentWaypoint = 0; // Текущая точка на пути
    public float minDistance = 2.0f; // Минимальное расстояние для начала поиска игрока

    public float speed = 2f; // Скорость движения врага
    public float nextWaypointDistance = 1f; // Расстояние для перехода к следующей точке
    public float repathRate = 0.5f; // Частота пересчета пути (в секундах)
    private float lastRepathTime; // Время последнего пересчета пути

    private bool isPlayerInRange; // Флаг, указывающий, находится ли игрок в зоне поиска
    private bool isWaiting = false; // Флаг, указывающий, остановлен ли враг

    public float stopDuration = 0.5f; // Время задержки в секундах

    private Vector3 initialPosition; // Исходная позиция врага
    private Animator animator;
    private Vector3 previousPosition;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        target = GameObject.FindWithTag("Player"); // Ищем игрока по тегу
        seeker = GetComponent<Seeker>();
        initialPosition = transform.position; // Сохраняем начальную позицию
        
        previousPosition = transform.position;
        // Проверяем, найдена ли цель, и пересчитываем путь, если она в зоне
        if (target != null && IsPlayerInRange())
        {
            isPlayerInRange = true;
            RecalculatePath();
        }

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 direction = (transform.position - previousPosition).normalized;

        if (direction.magnitude < 0.01f)
        {
            animator.SetInteger("direction", -1); // Idle
        }
        else
        {
            // Определяем доминирующее направление
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Горизонтальное движение доминирует
                if (direction.x > 0)
                    animator.SetInteger("direction", 2); // Move_Right
                else
                    animator.SetInteger("direction", 1); // Move_Left
            }
            else
            {
                // Вертикальное движение доминирует
                if (direction.y > 0)
                    animator.SetInteger("direction", 0);
                else
                    animator.SetInteger("direction", 2);
            }
        }

        // Обновляем предыдущую позицию
        previousPosition = transform.position;


        if (isWaiting) return; // Если враг ждет, не выполняем движение

        // Проверяем расстояние до игрока
        if (target != null)
        {
            isPlayerInRange = IsPlayerInRange();

            if (isPlayerInRange)
            {
                // Пересчитываем путь с заданной частотой
                if (Time.time - lastRepathTime > repathRate)
                {
                    RecalculatePath();
                    lastRepathTime = Time.time;
                }

                FollowPath(); // Следуем за игроком
            }
            else
            {
                StopFollowing();
                // Останавливаем движение
            }
        }
    }

    private void FollowPath()
    {
        // Если пути нет, выходим
        if (path == null) return;

        // Если достигли конца пути
        if (currentWaypoint >= path.vectorPath.Count)
        {
            StartCoroutine(WaitAtDestination()); // Останавливаемся на время
            return;
        }

        // Двигаем врага по пути
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Проверяем расстояние до следующей точки
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void StopFollowing()
    {
        
        //path = null; // Сбрасываем путь, чтобы остановить движение
                     // Враг возвращается на исходную позицию
                     //if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
                     //{
                     //    Vector3 direction = (initialPosition - transform.position).normalized;
                     //    transform.position += direction * speed * Time.deltaTime;
                     //}
        if (Time.time - lastRepathTime > repathRate)
        {
            if (seeker != null && target != null)
            {
                seeker.StartPath(transform.position, initialPosition, OnPathComplete);
            }
            lastRepathTime = Time.time;
        }

        FollowPath();

    }

    private bool IsPlayerInRange()
    {
        // Проверяем расстояние до игрока
        range = Vector2.Distance(transform.position, target.transform.position);
        return range < minDistance;
    }

    private void RecalculatePath()
    {
        if (seeker != null && target != null)
        {
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private System.Collections.IEnumerator WaitAtDestination()
    {
        isWaiting = true; // Устанавливаем флаг ожидания
        yield return new WaitForSeconds(stopDuration); // Ждем указанное время
        isWaiting = false; // Снимаем флаг ожидания
    }
}
