// See https://aka.ms/new-console-template for more information
using BotrisBattle.NET;

BotrisBot bot = new("token");
bot.RequestMove += Bot_RequestMove;

void Bot_RequestMove(RequestMovePayload obj)
{
    bot.SendMove([Command.hold, Command.drop]);
}

bot.Connect("roomkey", CancellationToken.None);

Console.ReadLine();