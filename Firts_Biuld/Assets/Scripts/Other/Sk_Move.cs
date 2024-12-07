using UnityEngine;
using Pathfinding;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Sk_Move : MonoBehaviour
{
    private float range;
    [SerializeField] private GameObject target; // ���� (�����)
    private Seeker seeker; // ��������� ��� ������� ����
    private Path path; // ������� ����
    private int currentWaypoint = 0; // ������� ����� �� ����
    public float minDistance = 2.0f; // ����������� ���������� ��� ������ ������ ������

    public float speed = 2f; // �������� �������� �����
    public float nextWaypointDistance = 1f; // ���������� ��� �������� � ��������� �����
    public float repathRate = 0.5f; // ������� ��������� ���� (� ��������)
    private float lastRepathTime; // ����� ���������� ��������� ����

    private bool isPlayerInRange; // ����, �����������, ��������� �� ����� � ���� ������
    private bool isWaiting = false; // ����, �����������, ���������� �� ����

    public float stopDuration = 0.5f; // ����� �������� � ��������

    private Vector3 initialPosition; // �������� ������� �����
    private Animator animator;
    private Vector3 previousPosition;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        target = GameObject.FindWithTag("Player"); // ���� ������ �� ����
        seeker = GetComponent<Seeker>();
        initialPosition = transform.position; // ��������� ��������� �������

        previousPosition = transform.position;
        // ���������, ������� �� ����, � ������������� ����, ���� ��� � ����
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
            // ���������� ������������ �����������
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // �������������� �������� ����������
                if (direction.x > 0)
                    animator.SetInteger("direction", 2); // Move_Right
                else
                    animator.SetInteger("direction", 4); // Move_Left
            }
            else
            {
                // ������������ �������� ����������
                if (direction.y > 0)
                    animator.SetInteger("direction", 1);
                else
                    animator.SetInteger("direction", 3);
            }
        }

        // ��������� ���������� �������
        previousPosition = transform.position;


        if (isWaiting) return; // ���� ���� ����, �� ��������� ��������

        // ��������� ���������� �� ������
        if (target != null)
        {
            isPlayerInRange = IsPlayerInRange();

            if (isPlayerInRange)
            {
                // ������������� ���� � �������� ��������
                if (Time.time - lastRepathTime > repathRate)
                {
                    RecalculatePath();
                    lastRepathTime = Time.time;
                }

                FollowPath(); // ������� �� �������
            }
            else
            {
                StopFollowing();
                // ������������� ��������
            }
        }
    }

    private void FollowPath()
    {
        // ���� ���� ���, �������
        if (path == null) return;

        // ���� �������� ����� ����
        if (currentWaypoint >= path.vectorPath.Count)
        {
            StartCoroutine(WaitAtDestination()); // ��������������� �� �����
            return;
        }

        // ������� ����� �� ����
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // ��������� ���������� �� ��������� �����
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void StopFollowing()
    {

        //path = null; // ���������� ����, ����� ���������� ��������
        // ���� ������������ �� �������� �������
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
        // ��������� ���������� �� ������
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
        isWaiting = true; // ������������� ���� ��������
        yield return new WaitForSeconds(stopDuration); // ���� ��������� �����
        isWaiting = false; // ������� ���� ��������
    }
}
