using CmdR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCmdR(typeof(Program));

var app = builder.Build();

app.UseCmdR();

app.Run();