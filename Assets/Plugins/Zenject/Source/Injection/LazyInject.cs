using Plugins.Zenject.Source.Internal;
using Plugins.Zenject.Source.Main;
using Plugins.Zenject.Source.Validation;
using Zenject;

namespace Plugins.Zenject.Source.Injection
{
    [ZenjectAllowDuringValidation]
    [NoReflectionBaking]
    public class LazyInject<T> : IValidatable
    {
        readonly DiContainer _container;
        readonly InjectContext _context;

        bool _hasValue;
        T _value;

        public LazyInject(DiContainer container, InjectContext context)
        {
            Assert.DerivesFromOrEqual<T>(context.MemberType);

            _container = container;
            _context = context;
        }

        void IValidatable.Validate()
        {
            _container.Resolve(_context);
        }

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    _value = (T)_container.Resolve(_context);
                    _hasValue = true;
                }

                return _value;
            }
        }
    }
}
