using System.Threading.Tasks;

namespace InteractiveApp.Services;

public interface ICameraPermissionService
{
    Task<bool> EnsurePermissionAsync();
}
