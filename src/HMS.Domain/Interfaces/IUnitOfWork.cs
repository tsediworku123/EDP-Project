using System;

namespace HMS.Core.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
    }
}
