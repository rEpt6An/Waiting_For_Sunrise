using UnityEngine;
using TMPro;
using Assets.C_.player.player;
using System.Text;
using Assets.C_.player;

public class PlayerStateDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stateDisplay;

    private IPlayerState playerState;

    void Start()
    {
        PlayerCharacter playerCharacter = UnityEngine.Object.FindObjectOfType<PlayerCharacter>();

        if (playerCharacter != null)
        {
            playerState = Player.GetInstance().PlayerState;
        }
        else
        {
            UnityEngine.Debug.LogError("PlayerStateDisplay: Could not find PlayerCharacter instance!");
        }
    }

    void Update()
    {
        if (playerState != null)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Blood: {playerState.Blood}");
            sb.AppendLine($"EXP: {playerState.Experience}");
            sb.AppendLine($"Damage Multiplier: {playerState.DamageMultipler:F2}");
            sb.AppendLine($"Max HP: {playerState.MaxHP}");
            sb.AppendLine($"Defense: {playerState.DefensivePower}");
            sb.AppendLine($"Dodge Rate: {playerState.Dodge:P1}");
            sb.AppendLine($"Attack Speed: {playerState.AttackSpeed:F2}");
            sb.AppendLine($"Hit Rate: {playerState.HitRate:P1}");
            sb.AppendLine($"Scope: {playerState.Scope}");
            sb.AppendLine($"Movement Speed: {playerState.Speed}");
            sb.AppendLine($"Crit Chance: {playerState.CriticalChance:P1}");
            sb.AppendLine($"Crit Damage: {playerState.CriticalDamage:F2}x");
            sb.AppendLine($"Lifesteal: {playerState.Leech:P1}");
            sb.AppendLine($"Self Healing: {playerState.SelfHealing}");
            sb.AppendLine($"Luck: {playerState.Lucky}");
            sb.AppendLine($"Harvest: {playerState.Harvest}");
            sb.AppendLine($"Melee Attack: {playerState.MeleeAttack}");
            sb.AppendLine($"Ranged Attack: {playerState.RangedAttack}");

            stateDisplay.text = sb.ToString();
        }
    }
}