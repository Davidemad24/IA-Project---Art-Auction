namespace ArtAuction.Application.Interfaces.Repositories;

public interface ISaveChanges
{
    Task<bool> Save();
}