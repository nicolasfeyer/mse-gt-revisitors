using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;

    public Transform leftBoundary;
    public Transform rightBoundary;

    public SpriteRenderer graphics;
    private Transform target;
    private int destPoint = 0;
    private PlayerCtrl player;

    private bool isFollowingPlayer = false;

    public float minDist = 5;
    // Start is called before the first frame update
    void Start()
    {
        target = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if(!isFollowingPlayer)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                destPoint = (destPoint + 1) % waypoints.Length;
                target = waypoints[destPoint];
                graphics.flipX =! graphics.flipX;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            
        }
    }
    private void FollowPlayer()
    {
        if(player==null)
        {
            player = FindObjectOfType<PlayerCtrl>();
            if(player==null){
                return;
            }
        }
        float distance = Vector3.Distance(player.gameObject.transform.position, this.transform.position);
        if(distance < minDist)
        {
            isFollowingPlayer = true;
            float distX = player.gameObject.transform.position.x - this.transform.position.x;
            float minDist;
            if(distX > 0)
            {
                minDist = Mathf.Min(distX, speed * Time.deltaTime * 2f);
                minDist = Mathf.Min(minDist, rightBoundary.position.x - transform.position.x);
            } else
            {
                minDist = Mathf.Max(distX, -speed * Time.deltaTime * 2f);
                minDist = Mathf.Max(minDist, leftBoundary.position.x - transform.position.x);
            }
            graphics.flipX = distX < 0;
            transform.Translate(Vector3.right*minDist, Space.World);
        }
        else 
        {
            isFollowingPlayer = false;
        }
    }
}
