using System.Linq;
using System.Reflection.Metadata;

namespace FetchBannerlordVersion
{
    public static class MetadataHelpers
    {
        public static TypeDefinition GetType(this TypeDefinitionHandleCollection tdh, MetadataReader reader, string ns, string typeName) => tdh
            .Select(reader.GetTypeDefinition)
            .First(x => reader.GetString(x.Name) == typeName && reader.GetString(x.Namespace) == ns);

        public static TypeDefinition GetNestedType(this TypeDefinition td, MetadataReader reader, string nestedTypeName) => td
            .GetNestedTypes()
            .Select(reader.GetTypeDefinition)
            .First(x => reader.GetString(x.Name) == nestedTypeName);

        public static FieldDefinition GetField(this TypeDefinition td, MetadataReader reader, string nestedTypeName) => td
            .GetFields()
            .Select(reader.GetFieldDefinition)
            .First(x => reader.GetString(x.Name) == nestedTypeName);
    }
}