using System.Threading.Tasks;

namespace InteractiveApp.Services;

public interface IMicrophonePermissionService
{
    Task<bool> EnsurePermissionAsync();
}
