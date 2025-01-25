using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PawnMothman : PawnEnemy
{
    public override void default_state(){
        wander();      
    }

    public override void target_state(){
        chase_target();
    }

    public override void hit_state(Collider2D other){
        kill_target(other);
    }
}
