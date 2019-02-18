using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public interface ICommandHandler
    {
        string Path { get; }
        Task HandleRequest(HttpContext context);
    }
}