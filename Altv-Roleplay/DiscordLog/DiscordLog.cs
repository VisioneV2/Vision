using Discord.Webhook;
using Discord.Webhook.HookRequest;

namespace Altv_Roleplay.Handler
{
    class DiscordLog
    {
        internal static void SendEmbed(string type, string nickname, string text)
        {
            DiscordWebhook hook = new DiscordWebhook();

            switch (type)
            {
                //https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96
                //https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96
                //https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96
                case "adminmenu":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017349450448947/Gp2GKE5ne_lpDDvXWPG99_dEsfkKndQPCUZ-YVV1GVPnX3-6G9_TtPsuqswIJslBjKE-";
                    break;
                case "Command":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017503255564389/V6uiD0XJd2BCbza20lIkMqFbhDMheYirwapxJF7A7HyyPwLspL1ESmin1lqmyPp_1Er_";
                    break;
                case "report":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017658969128982/L6SOKf0aDboTNVO3oB4UQPWrCXtx0p6WeGW42zi75QXUrj32apOupcs1skYDxBdl6k9n";
                    break;
                case "geldlog":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017762241253376/M57U3pQ8OmUWCVZr31k1iqbGZhhOCCl0gqP7IKYZ7OvmmYB8Y2A0-rwDcdrOFbbM32od";
                    break;
                case "itemlog":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017854855700510/hlvvBH3ESIFj9V75vNXnTGx6aVgiTb1HEPKqD5H8rtsfnm3UdKc9BeQZh0iU7N4vCf8Z";
                    break;
                case "kofferraum":
                    hook.HookUrl = "https://discord.com/api/webhooks/903017953568653402/jsK-lkllG4j40nPjTlkmS8jF7LX-z2Ddks2UPEpcwVEIYpK8bxFfK_DXtANFvbbhotjU";
                    break;
                case "frakbank":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018044761186334/wW6vJFz_hnfHd_rMkX1DhXrjKLuZ0rUdogpcg6iB_4dNkMQ7m2j9uK2YoRBzaRq567Qg";
                    break;
                case "housestorage":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018161434165388/pAm_YSUxqm84YGY0jT7C80Tlgc1Gr_Xf4d12Lr0oW3P-eEZAO60TacA3tCgkOOB2S2cX";
                    break;
                case "ooc":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018287519137813/Y8jvohCg2DqHCFFL74RHTMouABRooFpeEvjHf1-ZY1rOGbmbzUuCBk0uM2JRzEpNBJWj";
                    break;
                case "death":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018385162526770/1duzGQ3MIVutj6Crelq3k5PilTBbLUdBcw1P7zIa5ZyqcRfbuvntqYwquWbqRR7XbDDA";
                    break;
                case "ban":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018480327065622/LVLKGzr1yn4aHiw9DpZNMUd01efZ54SvvJ1NkJ0IfaMmT_ull8XVHGghHFA05Yj9qnNh";
                    break;
                case "kick":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018707461214248/LRhSAnupwLiS2WI_U0sstg4tNhSOa5QfyLquKUswfPfTTi50ewMj9_5Cx0o31jWFM43n";
                    break;
                case "sus":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018781520060417/0KqYA9okfae7RsF850eQUWX7ZACUtc9XXZXpPFSw89ky82rZS0XSZaVnklP4trZGWNJV";
                    break;
                case "event":
                    hook.HookUrl = "https://discord.com/api/webhooks/903018879985549312/hwLO5eb0hXyP19mfQDK8Dp2ItGp5HWti6J7IFzHhRkaslAxfk5KtwZaDpfNFEUlLW1sp";
                    break;
                case "serverstatus":
                    hook.HookUrl = "https://discord.com/api/webhooks/918706102470848522/pepRHaHt7Lv00AB4By0kFrrVVHSGujYAmahyH-CBPcCGxzBOvZ-e2X110yPq1ABjDvhm";
                    break;
                default:
                    hook.HookUrl = "https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96";
                    break;
            }

            if (hook.HookUrl == "https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96") return; //Hier WEB_HOOK nicht ersetzen

            DiscordHookBuilder builder = DiscordHookBuilder.Create(Nickname: nickname, AvatarUrl: "https://cdn.discordapp.com/attachments/865902854652821514/866305404949757952/CGRP_-_Discord1.png");

            DiscordEmbed embed = new DiscordEmbed(
                            Title: "Visione - Logs",
                            Description: text,
                            Color: 0xf54242,
                            FooterText: "Visione - Logs",
                            FooterIconUrl: "https://cdn.discordapp.com/icons/842873686104211486/c10d4214cbd9b2d575597ab48b68ad40.png?size=96");
            builder.Embeds.Add(embed);

            DiscordHook HookMessage = builder.Build();
            hook.Hook(HookMessage);
        }
    }
}
