using UnityEngine;

public class PawnEnemy : MonoBehaviour
{
    public ControllerEnemy Controller;
    public MoverAbstract Mover;
    public AudioSource hitSource;
    public AudioClip jumpscare;
    
    public float sight_range=25;
    public float chase_timeout = 5;
    public float retarget_timeout=3;
    private float retarget_timer=0;
    public float chase_timer=0;
    private float random_wait;
    public Vector2 random_wait_range=new Vector2(1, 3);
    public Vector2 direction = Vector2.zero;

    public void OnTriggerEnter2D(Collider2D other){
        hit_state(other);
    }

    
    public virtual void default_state(){}

    public virtual void target_state(){}

    public virtual void hit_state(Collider2D other){}


    public GameObject seen_target(Vector2 direction){
        retarget_timer-=Time.deltaTime;

        bool seen=false;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);
        if (ray.collider != null){
            seen = ray.collider.CompareTag("Player");
        }
        
        if (seen && retarget_timer<0){
            Debug.DrawRay(transform.position, direction, Color.green);
            return ray.collider.gameObject;
        }else{
            
            Debug.DrawRay(transform.position, direction, Color.red);
            return null;
        }   
    }
    public void set_target(GameObject new_target){
        Controller.target=new_target;
    }
    public void alert_target(Collider2D other){
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
		ControllerPlayer player = human.Controller as ControllerPlayer;
		if (player == null){
            random_wait=0;
            return;
        }
        var player_pawn=player.Pawn.gameObject;
        
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach ( GameObject enemy in enemies){
            var enemy_game_object = enemy.gameObject;
            
            var pawn_enemy = enemy_game_object.GetComponent<PawnEnemy>();
            pawn_enemy.set_target(player_pawn);
        }
        clear_target();
    }
    public void chase_target(){

        Controller.agent.SetDestination(Controller.target.transform.position);

        chase_timer+=Time.deltaTime;

        if(chase_timeout<chase_timer){
            clear_target();
        }
    }

    public void lunge_target(){
        Mover.Move(direction.normalized);
        chase_timer+=Time.deltaTime;

        if(chase_timeout<chase_timer){
            clear_target();
        }
    }
    public void kill_target(Collider2D other){
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
		ControllerPlayer player = human.Controller as ControllerPlayer;
		if (player == null){
            random_wait=0;
            return;
        }

        clear_target();
		player.Die(Controller.rarity);
        hitSource.PlayOneShot(jumpscare);
        hitSource.Play(0);
    }

    public void steal_from_target(Collider2D other){
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
		ControllerPlayer player = human.Controller as ControllerPlayer;
		if (player == null){
            random_wait=0;
            return;
        }
        
        clear_target();
        if (player.Coins>0){
            player.RemoveCoins(1);
            direction=player.transform.position-transform.position;
            hitSource.Play(0);
        }else{
            player.Die(Controller.rarity);
        }
        
		
    }
    public void clear_target(){
        Controller.target=null;
        chase_timer=0;
        retarget_timer=3;
    }

    public void wander(){
        random_wait-=Time.deltaTime;
        if (random_wait<=0){
            direction = Random.insideUnitCircle.normalized;
            random_wait=Random.Range(random_wait_range[0], random_wait_range[0]); 
        }
        Mover.Move(direction);

        //look for player
        Controller.target=seen_target(direction*sight_range);       
    }


    public void idle(){
        Controller.target=seen_target(direction*sight_range);       
    }

    public void debug_sight(){
        if (Controller.target){
            Debug.DrawRay(transform.position, direction, Color.green);
        }else{
            Debug.DrawRay(transform.position, direction, Color.red);
        }
    }
}
