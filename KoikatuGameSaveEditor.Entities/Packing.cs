using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MsgPack;
using MsgPack.Serialization;

namespace KGSE {
    // Taken from one of my old, old projects...
    internal static class Packing {

        static Packing() {
            AnonymousFieldNamePattern = new Regex(@"<([^\>]+)>", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            AnonymousTypeNamePattern = new Regex(@"\<[^>]*\>f__AnonymousType", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        [NotNull]
        public static byte[] PackToByteArray([CanBeNull] object obj) {
            if (obj == null) {
                return Array.Empty<byte>();
            }

            using (var memoryStream = new MemoryStream()) {
                using (var packer = Packer.Create(memoryStream, PackerCompatibilityOptions.Classic)) {
                    var dictionary = ObjectToDictionary(obj);
                    packer.PackDictionary(dictionary);
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }
        }

        public static MessagePackObject PackToObject([CanBeNull] object obj) {
            if (TryGetPrimitiveMessagePackValue(obj, out var o)) {
                return o;
            }

            return new MessagePackObject(ObjectToDictionary(obj));
        }

        [ContractAnnotation("null => null; notnull => notnull")]
        [CanBeNull]
        public static MessagePackObjectDictionary PackToDictionary([CanBeNull] object obj) {
            return ObjectToDictionary(obj);
        }

        [ContractAnnotation("null => null; notnull => notnull")]
        [CanBeNull]
        private static MessagePackObjectDictionary ObjectToDictionary([CanBeNull] object obj) {
            if (ReferenceEquals(obj, null)) {
                return null;
            }

            if (obj is MessagePackObjectDictionary possibleDictionary) {
                return possibleDictionary;
            }

            var result = new MessagePackObjectDictionary();
            var t = obj.GetType();
            const BindingFlags memberSearchFlags = BindingFlags.Instance | BindingFlags.Public;

            // TODO: Cache! Cache!
            var fields = t.GetFields(memberSearchFlags);
            var properties = t.GetProperties(memberSearchFlags);
            var fieldsAndProperties = new MemberInfo[fields.Length + properties.Length];

            Array.Copy(fields, fieldsAndProperties, fields.Length);
            Array.Copy(properties, 0, fieldsAndProperties, fields.Length, properties.Length);

            var memberAttributeCount = fieldsAndProperties.Count(member => member.GetCustomAttribute<MessagePackMemberAttribute>() != null);
            var ignoreAttributeCount = fieldsAndProperties.Count(member => member.GetCustomAttribute<MessagePackIgnoreAttribute>() != null);
            var searchMode = DetectSearchMode(memberAttributeCount, ignoreAttributeCount);

            foreach (var fieldInfo in fields) {
                var name = GetMemberSerializedName(fieldInfo, searchMode, true);

                if (name == null) {
                    continue;
                }

                var value = fieldInfo.GetValue(obj);
                var b = TryGetPrimitiveAndCompositeMessagePackValue(value, out var o);
                if (b) {
                    result.Add(name, o);
                }
            }

            foreach (var propertyInfo in properties) {
                var name = GetMemberSerializedName(propertyInfo, searchMode, false);

                if (name == null) {
                    continue;
                }

                var value = propertyInfo.GetValue(obj, null);
                var b = TryGetPrimitiveAndCompositeMessagePackValue(value, out var o);
                if (b) {
                    result.Add(name, o);
                }
            }

            return result;
        }

        private static AttributeSearchMode DetectSearchMode(int memberAttributeCount, int ignoreAttributeCount) {
            if (memberAttributeCount > 0) {
                return AttributeSearchMode.OnlyMember;
            } else if (ignoreAttributeCount > 0) {
                return AttributeSearchMode.OnlyIgnore;
            } else {
                return AttributeSearchMode.Default;
            }
        }

        [CanBeNull]
        private static string GetMemberSerializedName([NotNull] MemberInfo memberInfo, AttributeSearchMode searchMode, bool skipInternalName) {
            MessagePackIgnoreAttribute ignoreAttribute;

            switch (searchMode) {
                case AttributeSearchMode.Default:
                    break;
                case AttributeSearchMode.OnlyMember: {
                    var memberAttribute = memberInfo.GetCustomAttribute<MessagePackMemberAttribute>();

                    if (memberAttribute == null) {
                        return null;
                    }

                    ignoreAttribute = memberInfo.GetCustomAttribute<MessagePackIgnoreAttribute>();

                    if (ignoreAttribute != null) {
                        return null;
                    }

                    return memberAttribute.Name ?? GetDisplayableMemberName(memberInfo.Name, out _);
                }
                case AttributeSearchMode.OnlyIgnore: {
                    ignoreAttribute = memberInfo.GetCustomAttribute<MessagePackIgnoreAttribute>();

                    if (ignoreAttribute != null) {
                        return null;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchMode), searchMode, null);
            }

            var name = GetDisplayableMemberName(memberInfo.Name, out var isInternalName);
            // Anonymous types have anonymous fields, and corresponding properties.
            // We'll retrieve values from properties only, like Visual Studio.
            return isInternalName ? (skipInternalName ? null : name) : name;
        }

        [NotNull]
        private static string GetDisplayableMemberName([NotNull] string rawName, out bool isInternalName) {
            var n1 = rawName.IndexOf('<');
            var n2 = rawName.IndexOf('>');
            // Anonymous field. Naming rule: <#real_name#>i__field.
            if (n1 >= 0 && n2 >= 0 && n2 > n1) {
                var match = AnonymousFieldNamePattern.Match(rawName);
                isInternalName = true;
                return match.Groups[1].Value;
            } else {
                isInternalName = false;
                return rawName;
            }
        }

        private static bool TryGetPrimitiveAndCompositeMessagePackValue([CanBeNull] object obj, out MessagePackObject o) {
            var b = TryGetPrimitiveMessagePackValue(obj, out o);

            if (b) {
                return true;
            }

            b = TryGetCompositeMessagePackValue(obj, out o);

            return b;
        }

        private static bool TryGetPrimitiveMessagePackValue([CanBeNull] object obj, out MessagePackObject o) {
            if (ReferenceEquals(obj, null)) {
                o = MessagePackObject.Nil;
                return true;
            } else if (obj is string) {
                o = (string)obj;
            } else if (obj is bool) {
                o = (bool)obj;
            } else if (obj is byte) {
                o = (byte)obj;
            } else if (obj is sbyte) {
                o = (sbyte)obj;
            } else if (obj is ushort) {
                o = (ushort)obj;
            } else if (obj is short) {
                o = (short)obj;
            } else if (obj is uint) {
                o = (uint)obj;
            } else if (obj is int) {
                o = (int)obj;
            } else if (obj is ulong) {
                o = (ulong)obj;
            } else if (obj is long) {
                o = (long)obj;
            } else if (obj is float) {
                o = (float)obj;
            } else if (obj is double) {
                o = (double)obj;
            } else if (obj is byte[]) {
                o = (byte[])obj;
            } else if (obj is DateTime) {
                o = MessagePackConvert.FromDateTime((DateTime)obj);
            } else if (obj is IList<MessagePackObject>) {
                o = new MessagePackObject((IList<MessagePackObject>)obj, false);
            } else if (obj is MessagePackExtendedTypeObject) {
                o = new MessagePackObject((MessagePackExtendedTypeObject)obj);
            } else if (obj is MessagePackObjectDictionary) {
                o = new MessagePackObject((MessagePackObjectDictionary)obj, false);
            } else {
                o = MessagePackObject.Nil;
                return false;
            }

            return true;
        }

        private static bool TryGetCompositeMessagePackValue([CanBeNull] object obj, out MessagePackObject o) {
            if (ReferenceEquals(obj, null)) {
                o = MessagePackObject.Nil;
                return true;
            }

            var t = obj.GetType();

            if (AnonymousTypeNamePattern.IsMatch(t.Name)) {
                // Anonymous types
                var dict = ObjectToDictionary(obj);
                o = new MessagePackObject(dict, false);
                return true;
            }

            if (t.IsArray) {
                var array = (Array)obj;
                var list = new MessagePackObject[array.Length];
                for (var i = 0; i < list.Length; ++i) {
                    var arrValue = array.GetValue(i);
                    TryGetPrimitiveAndCompositeMessagePackValue(arrValue, out var objValue);
                    list[i] = objValue;
                }

                o = new MessagePackObject(list, false);
            } else if (t == typeof(Dictionary<object, object>)) {
                // TODO: Oh my f**king god, this is messy!
                var dict = (Dictionary<object, object>)obj;
                var d = new MessagePackObjectDictionary();
                foreach (var key in dict.Keys) {
                    if (TryGetPrimitiveAndCompositeMessagePackValue(key, out var primKey)) {
                        var value = dict[key];
                        var b = TryGetPrimitiveAndCompositeMessagePackValue(value, out var primValue);
                        d[primKey] = primValue;
                    }
                }

                o = new MessagePackObject(d);
            } else {
                // Debug.Print("WARNING: Packing: getting primitive value failed on object {0}.", obj);
                // o = new MessagePackObject();
                var d = ObjectToDictionary(obj);
                o = new MessagePackObject(d);
                return true;
            }

            return true;
        }

        [NotNull]
        private static readonly Regex AnonymousFieldNamePattern;

        [NotNull]
        private static readonly Regex AnonymousTypeNamePattern;

        private enum AttributeSearchMode {

            // All public fields/properties
            Default,

            // Only explicitly marked members
            OnlyMember,

            // All members except marked ones
            OnlyIgnore

        }

    }
}
