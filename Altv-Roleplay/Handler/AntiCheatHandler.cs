using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Altv_Roleplay.Handler
{
    public class AntiCheatHandler : IScript
    {
        [AsyncScriptEvent(ScriptEventType.WeaponDamage)]
        public static void WeaponDamageEvent(ClassicPlayer player, ClassicPlayer target, uint weapon, ushort dmg, Position offset, BodyPart bodypart)
        {
            try
            {
                if (player == null || !player.Exists || target == null || !target.Exists) return;
                WeaponModel weaponModel = (WeaponModel)weapon;
                if (weaponModel == WeaponModel.Fist) return;

                if (Enum.IsDefined(typeof(Utils.AntiCheatOld.AntiCheatOld.forbiddenWeapons), (Utils.AntiCheatOld.AntiCheatOld.forbiddenWeapons)weaponModel) && player.AdminLevel() < 1)
                {
                    DiscordLog.SendEmbed("death", "kill", $"{Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) wurde vom AntiCheat gebannt. Waffe: {weaponModel}");
                    User.SetPlayerBanned(player, true, $"Blacklisted Weapon: {weaponModel}");
                    player.Kick("");
                    foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                    {
                        HUDHandler.SendNotification(player, 4, 2500, $"{Characters.GetCharacterName(player.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncScriptEvent(ScriptEventType.PlayerEvent)]
        public Task PlayerEventEvent(ClassicPlayer player, string name, object[] args)
        {
            if (player == null || !player.Exists || player.SentEventWarning) { return Task.CompletedTask; }
            if (player.EventCount > 180)
            {
                var accounts = User.Player.Where(p => p.socialClub == player.SocialClubId).Select(p => $"{p.playerName}[{p.playerid}]");
                DiscordLog.SendEmbed("event", "Eventspam Logs", $"AltV-ID: {player.Id} spammt Events. (Letztes: {name}) IP: {player.Ip}. Accounts mit selbem SocialClub: {string.Join(", ", accounts)}");
                player.SentEventWarning = true;
                player.Kick("event spamming");
                return Task.CompletedTask;
            }
            player.EventCount += 1;

            return Task.CompletedTask;
        }

        public static void EventSpamHandler(object o, ElapsedEventArgs e)
        {
            var pool = Alt.GetAllPlayers().Cast<ClassicPlayer>();
            foreach (var player in pool)
            {
                player.EventCount = 0;
                player.SentEventWarning = false;
            }
        }
    }
}
