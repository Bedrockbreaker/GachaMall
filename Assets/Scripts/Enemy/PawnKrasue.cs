using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PawnKrasue : PawnEnemy
{
    public void Start(){
        direction=transform.up*sight_range;
    }
    public override void default_state(){
        idle();
    }
    public override void target_state(){
        lunge_target();
        
    }

    public override void hit_state(Collider2D other){
        kill_target(other);
        direction=-direction;
    }
}
