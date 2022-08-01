using Serilog;

const string environment_token_variable_name = "TIAMAT_TOKEN";
const string token_file = "./token";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
#if DEBUG
    .MinimumLevel.Debug()
#endif
    .CreateLogger();

string? token = Environment.GetEnvironmentVariable(environment_token_variable_name);
if(token == null)
{
    try
    {
        token = File.ReadAllText("./token");
    }
    catch(Exception ex)
    {
        Log.Error(ex, "Token not found neither in environament variable `{EnvironmentVariable}` nor in file `{FileName}`", environment_token_variable_name, token_file);
        throw;
    }
}

await new Discord.Bot().Main(token ?? "");