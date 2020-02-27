using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace KGSE.Localization {
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class Translation {

        [JsonConstructor]
        private Translation() {
        }

        internal Translation([NotNull] string displayName, [NotNull] string code) {
            DisplayName = displayName;
            Code = code;
            _map = new Dictionary<string, string>();
        }

        internal Translation([NotNull] string displayName, [NotNull] string code, [NotNull] IReadOnlyDictionary<string, string> map)
            : this(displayName, code) {
            foreach (var kv in map) {
                _map.Add(kv.Key, kv.Value);
            }
        }

        [JsonProperty("display_name")]
        [NotNull]
        public string DisplayName { get; private set; }

        [JsonProperty("code")]
        [NotNull]
        public string Code { get; private set; }

        [NotNull]
        public string Get([NotNull] string key) {
            return Get(key, string.Empty);
        }

        [NotNull]
        public string Get([NotNull] string key, [NotNull] string defaultValue) {
            var b = _map.TryGetValue(key, out var value);
            return b ? value : defaultValue;
        }

        public string this[[NotNull] string key] {
            get => Get(key);
        }

        [JsonProperty("map")]
        [NotNull]
        private readonly Dictionary<string, string> _map;

    }
}
