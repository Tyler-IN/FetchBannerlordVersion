using System.Linq;
using System.Reflection.Metadata;

namespace FetchBannerlordVersion
{
    public static class MetadataHelpers
    {
        public static TypeDefinition GetType(this TypeDefinitionHandleCollection typeDefHandles, MetadataReader reader,
            string ns, string typeName) => typeDefHandles
            .Select(reader.GetTypeDefinition)
            .First(t => reader.GetString(t.Name) == typeName && reader.GetString(t.Namespace) == ns);

        public static TypeDefinition GetNestedType(this TypeDefinition t, MetadataReader reader, string nestedTypeName) => t
            .GetNestedTypes()
            .Select(reader.GetTypeDefinition)
            .First(nt => reader.GetString(nt.Name) == nestedTypeName);

        public static FieldDefinition GetField(this TypeDefinition t, MetadataReader reader, string nestedTypeName) => t
            .GetFields()
            .Select(reader.GetFieldDefinition)
            .First(nt => reader.GetString(nt.Name) == nestedTypeName);
    }
}