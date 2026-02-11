using System.Threading.Tasks;

namespace InteractiveApp.Services;

public interface ICameraService
{
    Task<string?> TakePhotoAsync();
}