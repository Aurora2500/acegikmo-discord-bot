using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AcegikmoDiscordBot
{
    internal class DeleteEcho
    {
        private readonly Log _log;
        private readonly Config _config;

        public DeleteEcho(Log log, Config config)
        {
            _log = log;
            _config = config;
        }

        public async Task MessageDeletedAsync(Cacheable<IMessage, ulong> messageId, ISocketMessageChannel socket)
        {
            if (_log.TryGetMessage(messageId.Id, out var message) && socket is IGuildChannel socketGuild && socketGuild.GuildId == _config.server)
            {
                var after = _log.TryGetPreviousMessage(messageId.Id, socket.Id, out var previous)
                    ? $" after <https://discordapp.com/channels/{socketGuild.GuildId}/{previous.ChannelId}/{previous.MessageId}>"
                    : "";
                var toSend = $"Message by {MentionUtils.MentionUser(message.AuthorId)} deleted in {MentionUtils.MentionChannel(message.ChannelId)}{after}:\n{message.Message}";
                Console.WriteLine(toSend);
                var modchannel = await socketGuild.Guild.GetTextChannelAsync(_config.channel);
                await modchannel.SendMessageAsync(toSend);
            }
        }
    }
}
