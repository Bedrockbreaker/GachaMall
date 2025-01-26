using UnityEngine;

public class PawnMothman : PawnEnemy
{
    public override void default_state(){
        if (!GameManager.Instance.Player.CollectedRarities.Contains(GachaRarities.Legendary)) return;
        Controller.target=GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public override void target_state(){
        direction = Controller.target.transform.position - transform.position;
        Mover.Move(direction.normalized);
    }

    public override void hit_state(Collider2D other){
        kill_target(other);
    }
}
