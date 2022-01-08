using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var token = File.ReadAllText("./token");

await new Discord.Bot().Main(token);