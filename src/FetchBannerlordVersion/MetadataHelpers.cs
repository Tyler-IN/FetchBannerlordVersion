using System.Linq;
using System.Reflection.Metadata;

namespace FetchBannerlordVersion
{
    public static class MetadataHelpers
    {
        public static TypeDefinition GetType(this TypeDefinitionHandleCollection tdh, MetadataReader reader, string ns, string typeName)
        {
            return tdh
                .Where(x => !x.IsNil)
                .Select(reader.GetTypeDefinition)
                .FirstOrDefault(x => reader.GetString(x.Name) == typeName && reader.GetString(x.Namespace) == ns);
        }

        public static TypeDefinition GetNestedType(this TypeDefinition td, MetadataReader reader, string nestedTypeName)
        {
            if (td.Equals(default(TypeDefinition)))
                return default;
            
            return td
                .GetNestedTypes()
                .Where(x => !x.IsNil)
                .Select(reader.GetTypeDefinition)
                .FirstOrDefault(x => reader.GetString(x.Name) == nestedTypeName);
        }

        public static FieldDefinition GetField(this TypeDefinition td, MetadataReader reader, string nestedTypeName)
        {
            if (td.Equals(default(TypeDefinition)))
                return default;
            
            return td
                .GetFields()
                .Where(x => !x.IsNil)
                .Select(reader.GetFieldDefinition)
                .FirstOrDefault(x => reader.GetString(x.Name) == nestedTypeName);
        }
    }
}