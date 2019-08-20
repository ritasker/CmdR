

namespace CmdR
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface ICommandHandler
    {
        string Path { get; }
        Task HandleRequest(HttpContext context);
    }
}