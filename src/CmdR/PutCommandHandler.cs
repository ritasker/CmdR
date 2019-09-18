namespace CmdR
{
    public abstract class PutCommandHandler<TCommand> : CommandHandler<TCommand> {
        protected PutCommandHandler(string path = "/") : base(path){}
    }
}