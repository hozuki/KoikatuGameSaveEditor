using System;

namespace KGSE.Entities {
    public sealed class ValueChangingEventArgs<T> : EventArgs
        where T : unmanaged {

        public ValueChangingEventArgs(T newValue) {
            NewValue = newValue;
        }

        public T NewValue { get; }

    }
}
