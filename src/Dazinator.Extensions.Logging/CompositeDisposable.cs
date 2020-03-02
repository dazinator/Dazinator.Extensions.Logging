using System;

namespace Microsoft.Extensions.Logging
{
    public class CompositeDisposable : IDisposable
    {
        private readonly IDisposable[] _disposables;

        public CompositeDisposable(params IDisposable[] disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item?.Dispose();
            }
        }
    }
}