using JetBrains.Annotations;

namespace Api.Models.QueryParameter
{
    public class BookQueryParameter:QueryParameter
    {
        [CanBeNull]
        public string? Name { get; set; }
        [CanBeNull]
        public string? Author { get; set; }

        public bool? IsInLibrary { get; set; }
    }
}
