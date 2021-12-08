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


namespace Altv_Roleplay.Handler
{
    class DeathHandler : IScript
    {
        [AsyncScriptEvent(ScriptEventType.PlayerDead)]
        public async Task OnPlayerDeath(ClassicPlayer player, IEntity killer, uint weapon)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                if (Characters.IsCharacterUnconscious(charId)) return;
                if (Characters.IsCharacterInJail(charId))
                {
                    player.Spawn(new Position(1691.4594f, 2565.7056f, 45.556763f), 0);
                    player.Position = new Position(1691.4594f, 2565.7056f, 45.556763f);
                    return;
                }
              

                openDeathscreen(player);
                Characters.SetCharacterUnconscious(charId, true, 10); // Von 15 auf 10 geändert.
                ServerFactions.createFactionDispatch(player, 4, $"HandyNotruf", $"Eine Verletzte Person wurde gemeldet");/*
                Characters.SetCharacterHealth(charId, 0);
                Characters.SetCharacterArmor(charId, 0);*/
                player.EmitLocked("Client:HUD:UpdateDesire", Characters.GetCharacterArmor(charId), Characters.GetCharacterHealth(charId), Characters.GetCharacterHunger(charId), Characters.GetCharacterThirst(charId)); //HUD updaten

                Alt.Emit("Server:Smartphone:leaveRadioFrequence", player);

                ClassicPlayer killerPlayer = (ClassicPlayer)killer;
                if (killerPlayer == null || !killerPlayer.Exists)
                {
                    WeaponModel weaponModel = (WeaponModel)weapon;
                    DiscordLog.SendEmbed("death", "kill", $"{Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) hat {Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) getötet. Waffe: {weaponModel}");
                    foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && (int)x.GetCharacterMetaId() > 0 && x.AdminLevel() > 1))
                    {
                        HUDHandler.SendNotification(p, 4, 7500, $"{Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) hat {Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) getötet. Waffe: {weaponModel}");
                    }
                    if (Enum.IsDefined(typeof(AntiCheat.forbiddenWeapons), (Utils.AntiCheat.forbiddenWeapons)weaponModel) && player.AdminLevel() < 8)
                    {
                        User.SetPlayerBanned(player, true, $"Waffen Hack[2]: {weaponModel}");
                        player.Kick("");
                        player.Health = 200;
                        foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                        {
                            HUDHandler.SendNotification(p, 4, 2500, $"{Characters.GetCharacterName(player.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                        }
                        return;
                    }
                    string weaponName = WeaponHandler.GetWeaponItemNameByWeaponModel(weaponModel);
                    if ((weaponName == "" && !WeaponHandler.IsMeleeKill(weaponModel) && player.AdminLevel() < 8) || ((!WeaponHandler.IsMeleeKill(weaponModel) && player.AdminLevel() < 8 && !CharactersInventory.ExistCharacterItem((int)player.GetCharacterMetaId(), weaponName, "inventory") && !CharactersInventory.ExistCharacterItem((int)player.GetCharacterMetaId(), weaponName, "backpack"))))
                    {
                        if (weaponModel == WeaponModel.Fist) return;
                        User.SetPlayerBanned(player, true, $"Waffen Hack[2]: {weaponModel}");
                        player.Kick("");
                        player.Health = 200;
                        foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                        {
                            HUDHandler.SendNotification(p, 4, 2500, $"{Characters.GetCharacterName(player.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                        }
                        return;
                    }

                }
                else
                {
                    WeaponModel weaponModel = (WeaponModel)weapon;
                    DiscordLog.SendEmbed("death", "kill", $"{Characters.GetCharacterName(killerPlayer.CharacterId)} ({killerPlayer.CharacterId}) hat {Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) getötet. Waffe: {weaponModel}");
                    HUDHandler.SendNotification(player, 4, 7500, $"Der Spieler mit der CharakterID {killerPlayer.CharacterId} hat dich getötet. Waffe: {weaponModel}");
                    foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                    {
                        HUDHandler.SendNotification(p, 4, 7500, $"{Characters.GetCharacterName(killerPlayer.CharacterId)} ({killerPlayer.CharacterId}) hat {Characters.GetCharacterName(player.CharacterId)} ({player.CharacterId}) getötet. Waffe: {weaponModel}");
                    }
                    if (Enum.IsDefined(typeof(AntiCheat.forbiddenWeapons), (Utils.AntiCheat.forbiddenWeapons)weaponModel) && killerPlayer.AdminLevel() < 8)
                    {
                        User.SetPlayerBanned(killerPlayer, true, $"Waffen Hack[2]: {weaponModel}");
                        killerPlayer.Kick("");
                        player.Health = 200;
                        foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                        {
                            HUDHandler.SendNotification(p, 4, 2500, $"{Characters.GetCharacterName(killerPlayer.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                        }
                        return;
                    }

                    string weaponName = WeaponHandler.GetWeaponItemNameByWeaponModel(weaponModel);
                    if ((weaponName == "" && !WeaponHandler.IsMeleeKill(weaponModel) && killerPlayer.AdminLevel() < 8) || ((!WeaponHandler.IsMeleeKill(weaponModel) && killerPlayer.AdminLevel() < 8 && !CharactersInventory.ExistCharacterItem((int)killerPlayer.GetCharacterMetaId(), weaponName, "inventory") && !CharactersInventory.ExistCharacterItem((int)killerPlayer.GetCharacterMetaId(), weaponName, "backpack"))))
                    {
                        if (weaponModel == WeaponModel.Fist) return;
                        User.SetPlayerBanned(killerPlayer, true, $"Waffen Hack[2]: {weaponModel}");
                        killerPlayer.Kick("");
                        killerPlayer.Health = 200;
                        foreach (IPlayer p in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                        {
                            HUDHandler.SendNotification(p, 4, 2500, $"{Characters.GetCharacterName(killerPlayer.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                        }
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void openDeathscreen(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                Position pos = new Position(player.Position.X, player.Position.Y, player.Position.Z + 1);
                player.Spawn(pos);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", true, 0); //Ragdoll setzen
                player.EmitLocked("Client:Deathscreen:openCEF"); // Deathscreen öffnen
                player.SetPlayerIsUnconscious(true);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void closeDeathscreen(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.EmitLocked("Client:Deathscreen:closeCEF");
                player.SetPlayerIsUnconscious(false);
                player.SetPlayerIsFastFarm(false);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                Characters.SetCharacterUnconscious(charId, false, 0);
                Characters.SetCharacterFastFarm(charId, false, 0);
                player.EmitLocked("Client:Inventory:StopEffect", "DrugsMichaelAliensFight");

                foreach (var item in CharactersInventory.CharactersInventory_.ToList().Where(x => x.charId == charId))
                {
                    if (item.itemName.Contains("EC Karte") || item.itemName.Contains("Ausweis") || item.itemName.Contains("Fahrzeugschluessel") || item.itemName.Contains("Bargeld") || item.itemLocation.Contains("clothes") || ServerItems.GetItemType(ServerItems.ReturnNormalItemName(item.itemName)) == "clothes") continue;
                    CharactersInventory.RemoveCharacterItem(charId, item.itemName, item.itemLocation);
                }

                Characters.SetCharacterWeapon(player, "PrimaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "PrimaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "SecondaryWeapon2", "None");
                Characters.SetCharacterWeapon(player, "SecondaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "SecondaryAmmo2", 0);
                Characters.SetCharacterWeapon(player, "SecondaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "FistWeapon", "None");
                Characters.SetCharacterWeapon(player, "FistWeaponAmmo", 0);
                player.EmitLocked("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId));
                Characters.SetCharacterPhoneEquipped(charId, false);
                player.RemoveAllWeaponsAsync();
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        internal static void revive(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.EmitLocked("Client:Deathscreen:closeCEF");
                player.SetPlayerIsUnconscious(false);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                Characters.SetCharacterUnconscious(charId, false, 0);
                ServerFactions.SetFactionBankMoney(3, ServerFactions.GetFactionBankMoney(4) + 1500); //ToDo: Preis anpassen
                player.EmitLocked("Client:HUD:UpdateDesire", Characters.GetCharacterArmor(charId), Characters.GetCharacterHealth(charId), Characters.GetCharacterHunger(charId), Characters.GetCharacterThirst(charId)); //HUD updaten
                /*Characters.SetCharacterHealth(charId, 0);
                Characters.SetCharacterArmor(charId, 0);*/
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
