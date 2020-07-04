using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml;

namespace FetchBannerlordVersion
{
    public static partial class Program
    {
        private static CustomAttribute GetVirtualFileAttribute(MetadataReader mdReader)
        {
            var vfType = mdReader.TypeDefinitions
                .GetType(mdReader, "TaleWorlds.Library", "VirtualFolders");

            return vfType
                .GetNestedType(mdReader, "Win64_Shipping_Client")
                .GetNestedType(mdReader, "bin")
                .GetNestedType(mdReader, "Parameters")
                .GetField(mdReader, "Version")
                .GetCustomAttributes()
                .Select(ah => mdReader.GetCustomAttribute(ah))
                .First(a =>
                {
                    var ctor = a.Constructor;
                    var ctorKind = ctor.Kind;
                    if (ctorKind != HandleKind.MethodDefinition)
                        return false;

                    var attributeDth = mdReader.GetMethodDefinition((MethodDefinitionHandle) ctor).GetDeclaringType();
                    var attributeDt = mdReader.GetTypeDefinition(attributeDth);
                    var name = mdReader.GetString(attributeDt.Name);
                    return name == "VirtualFileAttribute";
                });
        }


        public static int GetChangeSet(string libPath)
        {
            switch (GetVersionType(libPath))
            {
                case VersionType.Unknown:
                    return 0;
                case VersionType.V1:
                    return 0;
                case VersionType.V2:
                case VersionType.V3:
                {
                    using var fs = File.OpenRead(libPath);
                    using var peReader = new System.Reflection.PortableExecutable.PEReader(fs);
                    var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                    var changeSetHandle = mdReader.TypeDefinitions
                        .GetType(mdReader, "TaleWorlds.Library", "ApplicationVersion")
                        .GetField(mdReader, "DefaultChangeSet")
                        .GetDefaultValue();
                    var changeSetBlob = mdReader.GetConstant(changeSetHandle).Value;

                    return mdReader.GetBlobReader(changeSetBlob).ReadInt32();
                }
                default:
                    return 0;
            }
        }

        public static string GetVersion(string libPath)
        {
            switch (GetVersionType(libPath))
            {
                case VersionType.Unknown:
                    return "";
                case VersionType.V1:
                case VersionType.V2:
                {
                    using var fs = File.OpenRead(libPath);
                    using var peReader = new System.Reflection.PortableExecutable.PEReader(fs);
                    var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                    var versionVirtualFileAttribute = GetVirtualFileAttribute(mdReader);
                    var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);
                    attributeReader.ReadByte();
                    attributeReader.ReadByte();
                    attributeReader.ReadSerializedString();
                    var xml = attributeReader.ReadSerializedString();
                    using var xmlReader = new XmlTextReader(new StringReader(xml));
                    xmlReader.Read();
                    xmlReader.MoveToAttribute("Value");
                    return xmlReader.Value;
                }
                case VersionType.V3:
                {
                    using var fs = File.OpenRead(libPath);
                    using var peReader = new System.Reflection.PortableExecutable.PEReader(fs);
                    var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                    var versionVirtualFileAttribute = GetVirtualFileAttribute(mdReader);
                    var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);
                    attributeReader.ReadByte();
                    attributeReader.ReadByte();
                    attributeReader.ReadSerializedString();
                    var xml = attributeReader.ReadSerializedString();
                    using var xmlReader = new XmlTextReader(new StringReader(xml));
                    xmlReader.Read();
                    xmlReader.ReadToDescendant("Singleplayer");
                    xmlReader.MoveToAttribute("Value");
                    return xmlReader.Value;
                }
                default:
                    return "";
            }
        }

        public static VersionType GetVersionType(string libPath)
        {
            using var fs = File.OpenRead(libPath);
            using var peReader = new System.Reflection.PortableExecutable.PEReader(fs);
            var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);


            var applicationVersionDef = mdReader.TypeDefinitions
                .GetType(mdReader, "TaleWorlds.Library", "ApplicationVersion");
            var hasChangeSet = applicationVersionDef.GetFields()
                .Any(f => mdReader.GetString(mdReader.GetFieldDefinition(f).Name) == "DefaultChangeSet");


            var versionVirtualFileAttribute = GetVirtualFileAttribute(mdReader);

            var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);

            if (attributeReader.ReadByte() != 0x01 || attributeReader.ReadByte() != 0x00)
                throw new NotImplementedException("Custom Attribute prolog is invalid.");

            var fn = attributeReader.ReadSerializedString();

            if (fn != "Version.xml")
                throw new NotImplementedException("Version field doesn't have Version.xml attribute.");

            var xml = attributeReader.ReadSerializedString();
            using var xmlReader = new XmlTextReader(new StringReader(xml));

            if (!xmlReader.Read())
                throw new NotImplementedException("Can't parse XML content of Version.xml");

            if (hasChangeSet)
            {
                if (!xmlReader.MoveToAttribute("Value"))
                {
                    if (!xmlReader.ReadToDescendant("Singleplayer"))
                        return VersionType.Unknown;
                    return VersionType.V3;
                }
                return VersionType.V2;
            }
            else
            {
                if (!xmlReader.MoveToAttribute("Value"))
                    return VersionType.Unknown;
                return VersionType.V1;
            }
        }
    }
}