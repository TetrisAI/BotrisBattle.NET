// See https://aka.ms/new-console-template for more information
using BotrisBattle.NET;
using System.Text.Json;
using ZZZTOJ.Botris;
using System.Runtime.InteropServices;




try
{
    nint v = ZZZTOJCore.AIName(8);

    string name = Marshal.PtrToStringAnsi(v);
    Console.WriteLine("DLL AIName = {0}", name);
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}

BotSetting botSetting = new();

if (File.Exists("botconfig.json"))
{
    botSetting = JsonSerializer.Deserialize<BotSetting>(File.ReadAllText("botconfig.json"));
}
else
{
    File.WriteAllText("botconfig.json", JsonSerializer.Serialize(botSetting));
}

if (args.Contains("--quiet")) {

    botSetting.Quiet = true;
}

BotrisBot bot = new(botSetting.Token);
ZZZBot bot1 = new() { BotSetting = botSetting };

bot.RequestMove += BotRequestMove;
bot.UpdateConfig += BotUpdateConfig;

bot.Connect(botSetting.RoomKey, CancellationToken.None);

while (true)
{
    var input = Console.ReadLine();
    if (input == "q") break;
}

return;

void BotRequestMove(RequestMovePayload obj)
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

void BotUpdateConfig(UpdateConfigPayload payload)
{
    try
    {
        bot1.BotSetting.Duration = payload.Duration;
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}


public class BotSetting
{
    public int NextCnt { get; set; } = 6;
    public int Duration { get; set; } = 100;
    public string Token { get; set; } = string.Empty;
    public string RoomKey { get; set; } = string.Empty;
    public bool Quiet {get;set;} = false;
    //public bool AutoLevel { get; set; } = true;

}