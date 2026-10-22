using UnityEngine;

public class WagonFollower : MonoBehaviour
{
    [Header("Chemin")]
    public Transform[] waypoints;
    public float speed = 10f;
    public float rotationSpeed = 10f;

    [Header("Décalage au démarrage")]
    public float startDelay = 0f; // Temps d'attente en secondes avant de démarrer

    private int currentWaypointIndex = 0;
    private float timer = 0f;
    private bool isMoving = false;

    void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            // Place le wagon sur le tout premier waypoint au lancement
            transform.position = waypoints[0].position;

            // Oriente immédiatement le wagon vers le deuxième waypoint si présent
            if (waypoints.Length > 1)
            {
                Vector3 initialDirection = (waypoints[1].position - transform.position).normalized;
                if (initialDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(initialDirection) * Quaternion.Euler(-90f, 0f, 0f);
                }
            }
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // Gestion du délai initial (ne s'exécute qu'une seule fois au lancement)
        if (!isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= startDelay)
            {
                isMoving = true;
            }
            else
            {
                return; // Le wagon attend sur place
            }
        }

        Transform target = waypoints[currentWaypointIndex];

        // Avancer vers le waypoint
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Orienter le wagon vers le waypoint
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90f, 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Passer au point suivant
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Boucle fluide sans réappliquer le délai
        }
    }
}