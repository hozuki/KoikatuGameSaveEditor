using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace KGSE.Entities {
    [DebuggerDisplay("{UnderlyingTypeName} Property: [\"{Key}\" = {Value}]")]
    public abstract class ValueProperty<T>
        where T : unmanaged {

        protected ValueProperty([NotNull] string key, T value) {
            Key = key;
            _value = value;
        }

        public event EventHandler<ValueChangingEventArgs<T>> ValueChanging;

        public event EventHandler<EventArgs> ValueChanged;

        public T Value {
            get => _value;
            set {
                if (EqualityComparer<T>.Default.Equals(value, _value)) {
                    return;
                }

                ValueChanging?.Invoke(this, new ValueChangingEventArgs<T>(value));
                _value = value;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [NotNull]
        public string Key { get; internal set; }

        [NotNull]
        private static string UnderlyingTypeName {
            [DebuggerStepThrough]
            get => typeof(T).Name;
        }

        private T _value;

    }
}
