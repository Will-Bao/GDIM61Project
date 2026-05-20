using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter/Steps/Player Anim")]
public class PlayerAnimStep : EncounterStep
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _animClip;
    [SerializeField] private float _duration;
    public override IEnumerator Execute(EncounterContext context)
    {
        Player player = context.Player.GetComponentInChildren<Player>();
        Animator playerAnim = context.Player.GetComponentInChildren<Animator>();
        Debug.Log("Triggered anim");
        player.SetPlayerStunned(true);
        playerAnim.Play(_animClip.name);
        yield return new WaitForSeconds(Mathf.Max(_animClip.length, _duration));
        player.SetPlayerStunned(false);
    }
}
