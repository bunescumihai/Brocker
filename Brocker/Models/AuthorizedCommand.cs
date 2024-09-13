using System.Net;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Brocker.Models;

public class AuthorizedCommand<T>: Command<T>
{
    public ICredentials Credentials { get; set; }

    public AuthorizedCommand(string name, T content) : base(name, content)
    {
    }
}