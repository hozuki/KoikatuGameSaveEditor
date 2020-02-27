using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace KGSE.Entities {
    public abstract class ValuePropertyBag<T>
        where T : unmanaged {

        private protected ValuePropertyBag() {
        }

        [NotNull, ItemNotNull]
        public string[] Keys {
            get {
                EnsurePropertiesLoaded();

                if (_keys is null) {
                    Debug.Assert(_properties != null, nameof(_properties) + " != null");
                    _keys = _properties.Select(p => p.Key).ToArray();
                }

                return _keys;
            }
        }

        [NotNull]
        public ValueProperty<T> GetProperty([NotNull] string key) {
            var index = Array.IndexOf(Keys, key);

            if (index < 0) {
                throw new KeyNotFoundException($"Key missing: '{key}'.");
            }

            Debug.Assert(_properties != null, nameof(_properties) + " != null");
            return _properties[index];
        }

        public T Get([NotNull] string key) {
            return GetProperty(key).Value;
        }

        public void Set([NotNull] string key, T value) {
            GetProperty(key).Value = value;
        }

        [NotNull, ItemNotNull]
        public abstract ValueProperty<T>[] GetAllProperties();

        private void EnsurePropertiesLoaded() {
            if (_properties is null) {
                _properties = GetAllProperties();
            }
        }

        [CanBeNull, ItemNotNull]
        private ValueProperty<T>[] _properties;

        [CanBeNull, ItemNotNull]
        private string[] _keys;

    }
}
