using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] public int gems = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.GemsCollected(gems);
                Destroy(this.gameObject);
            }
        }
    }
}
