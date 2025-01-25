using UnityEngine;

public class PawnEnemy : PawnAbstract
{
    public GameObject seen_target(Vector2 direction){
        bool seen=false;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);
        if (ray.collider != null){
            seen = ray.collider.CompareTag("Player");
        }
        
        if (seen){
            Debug.DrawRay(transform.position, direction, Color.green);
            return ray.collider.gameObject;
        }else{
            
            Debug.DrawRay(transform.position, direction, Color.red);
            return null;
        }
        
    }
}
