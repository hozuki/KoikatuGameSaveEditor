using JetBrains.Annotations;

namespace KGSE {
    public sealed class ErrorEntry {

        private ErrorEntry([NotNull] string firstName, [NotNull] string lastName, [NotNull] string description) {
            FirstName = firstName;
            LastName = lastName;
            Description = description;
        }

        [NotNull]
        public static ErrorEntry New([NotNull] string firstName, [NotNull] string lastName, [NotNull] string description) {
            return new ErrorEntry(firstName, lastName, description);
        }

        [NotNull]
        public string FirstName { get; }

        [NotNull]
        public string LastName { get; }

        [NotNull]
        public string Description { get; }

        public override string ToString() {
            return $"{FirstName} {LastName}: {Description}";
        }

    }
}
