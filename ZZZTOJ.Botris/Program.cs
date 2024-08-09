// See https://aka.ms/new-console-template for more information
using BotrisBattle.NET;
using System.Text.Json;
using ZZZTOJ.Botris;


BotSetting botSetting = new();

if (File.Exists("botconfig.json"))
{
    botSetting = JsonSerializer.Deserialize<BotSetting>(File.ReadAllText("botconfig.json"));
}
else
{
    File.WriteAllText("botconfig.json", JsonSerializer.Serialize(botSetting));
}

BotrisBot bot = new(botSetting.Token);
ZZZBot bot1 = new() { BotSetting = botSetting };
bot.RequestMove += Bot_RequestMove;
async void Bot_RequestMove(RequestMovePayload obj)
{

    //await Task.Delay(1);
    //Console.Clear();
    try
    {
        var res = bot1.GetMove(obj);
        bot.SendMove(res.moves.ToArray());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }


}

bot.Connect(botSetting.RoomKey, CancellationToken.None);

while (true)
{
    var input = Console.ReadLine();
    if (input == "q") break;
}


public class BotSetting
{
    public int NextCnt { get; set; } = 6;
    public int Level { get; set; } = 8;
    public int BPM { get; set; } = 200;

    public string Token { get; set; } = string.Empty;
    public string RoomKey { get; set; } = string.Empty;
    //public bool AutoLevel { get; set; } = true;

}