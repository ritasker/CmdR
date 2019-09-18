namespace CmdR
{
    public abstract class PostCommandHandler<TCommand> : CommandHandler<TCommand>
    {
        public PostCommandHandler(string path = "/") : base(path){}
    }
}