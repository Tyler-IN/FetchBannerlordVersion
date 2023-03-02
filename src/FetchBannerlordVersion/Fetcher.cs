using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml;

namespace FetchBannerlordVersion
{
    public static class Fetcher
    {
        private static CustomAttribute? GetVirtualFileAttributeV1(MetadataReader mdReader)
        {
            var vfType = mdReader.TypeDefinitions
                .GetType(mdReader, "TaleWorlds.Library", "VirtualFolders");

            return vfType
                .GetNestedType(mdReader, "Win64_Shipping_Client")
                .GetNestedType(mdReader, "bin")
                .GetNestedType(mdReader, "Parameters")
                .GetField(mdReader, "Version")
                .GetCustomAttributes()
                .Select(mdReader.GetCustomAttribute)
                .FirstOrDefault(a =>
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
        private static CustomAttribute? GetVirtualFileAttributeV2(MetadataReader mdReader)
        {
            var vfType = mdReader.TypeDefinitions
                .GetType(mdReader, "TaleWorlds.Library", "VirtualFolders");

            return vfType
                .GetNestedType(mdReader, "Shipping_Client")
                .GetNestedType(mdReader, "bin")
                .GetNestedType(mdReader, "Parameters")
                .GetField(mdReader, "Version")
                .GetCustomAttributes()
                .Select(mdReader.GetCustomAttribute)
                .FirstOrDefault(a =>
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


        public static int GetChangeSet(string gameFolderPath, string libAssembly)
        {
            try
            {
                var libFolderPath = Path.GetFullPath(Path.Combine(gameFolderPath, "bin", "Win64_Shipping_Client"));
                switch (GetVersionType(gameFolderPath, libAssembly))
                {
                    case VersionType.Unknown:
                        return 0;
                    case VersionType.V1:
                        return 0;
                    case VersionType.V2:
                    case VersionType.V3:
                    case VersionType.V5:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, libAssembly));
                        using var peReader = new PEReader(fs);
                        var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                        var changeSetHandle = mdReader.TypeDefinitions
                            .GetType(mdReader, "TaleWorlds.Library", "ApplicationVersion")
                            .GetField(mdReader, "DefaultChangeSet")
                            .GetDefaultValue();
                        var changeSetBlob = mdReader.GetConstant(changeSetHandle).Value;

                        return mdReader.GetBlobReader(changeSetBlob).ReadInt32();
                    }
                    case VersionType.V4:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, "Version.xml"));
                        using var xmlReader = XmlReader.Create(fs);
                        xmlReader.Read();
                        xmlReader.ReadToDescendant("Singleplayer");
                        xmlReader.MoveToAttribute("Value");
                        var split = xmlReader.Value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        return int.Parse(split.Last());
                    }
                    default:
                        return 0;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception($"gameFolderPath: {gameFolderPath}, libAssembly: {libAssembly}", ex);
            }
        }

        public static string GetVersion(string gameFolderPath, string libAssembly)
        {
            try
            {
                var libFolderPath = Path.GetFullPath(Path.Combine(gameFolderPath, "bin", "Win64_Shipping_Client"));
                switch (GetVersionType(gameFolderPath, libAssembly))
                {
                    case VersionType.Unknown:
                        return "";
                    case VersionType.V1:
                    case VersionType.V2:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, libAssembly));
                        using var peReader = new PEReader(fs);
                        var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                        var versionVirtualFileAttribute = GetVirtualFileAttributeV1(mdReader)!.Value;
                        var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);
                        attributeReader.ReadByte();
                        attributeReader.ReadByte();
                        attributeReader.ReadSerializedString();
                        var xml = attributeReader.ReadSerializedString() ?? string.Empty;
                        using var xmlReader = new XmlTextReader(new StringReader(xml));
                        xmlReader.Read();
                        xmlReader.MoveToAttribute("Value");
                        return xmlReader.Value;
                    }
                    case VersionType.V3:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, libAssembly));
                        using var peReader = new PEReader(fs);
                        var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                        var versionVirtualFileAttribute = GetVirtualFileAttributeV1(mdReader)!.Value;
                        var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);
                        attributeReader.ReadByte();
                        attributeReader.ReadByte();
                        attributeReader.ReadSerializedString();
                        var xml = attributeReader.ReadSerializedString() ?? string.Empty;
                        using var xmlReader = XmlReader.Create(new StringReader(xml));
                        xmlReader.Read();
                        xmlReader.ReadToDescendant("Singleplayer");
                        xmlReader.MoveToAttribute("Value");
                        return xmlReader.Value;
                    }
                    case VersionType.V4:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, "Version.xml"));
                        using var xmlReader = XmlReader.Create(new StreamReader(fs));
                        xmlReader.Read();
                        xmlReader.ReadToDescendant("Singleplayer");
                        xmlReader.MoveToAttribute("Value");
                        var split = xmlReader.Value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        return string.Join(".", split.Take(split.Length - 1));
                    }
                    case VersionType.V5:
                    {
                        using var fs = File.OpenRead(Path.Combine(libFolderPath, libAssembly));
                        using var peReader = new PEReader(fs);
                        var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);
                        var versionVirtualFileAttribute = GetVirtualFileAttributeV2(mdReader)!.Value;
                        var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttribute.Value);
                        attributeReader.ReadByte();
                        attributeReader.ReadByte();
                        attributeReader.ReadSerializedString();
                        var xml = attributeReader.ReadSerializedString() ?? string.Empty;
                        using var xmlReader = XmlReader.Create(new StringReader(xml));
                        xmlReader.Read();
                        xmlReader.ReadToDescendant("Singleplayer");
                        xmlReader.MoveToAttribute("Value");
                        return xmlReader.Value;
                    }
                    default:
                        return "";
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception($"gameFolderPath: {gameFolderPath}, libAssembly: {libAssembly}", ex);
            }
        }

        public static VersionType GetVersionType(string gameFolderPath, string libAssembly)
        {
            try
            {
                var libFolderPath = Path.GetFullPath(Path.Combine(gameFolderPath, "bin", "Win64_Shipping_Client"));
                if (File.Exists(Path.Combine(libFolderPath, "Version.xml")))
                    return VersionType.V4;

                if (!File.Exists(Path.Combine(libFolderPath, libAssembly)))
                    return VersionType.Unknown;

                using var fs = File.OpenRead(Path.Combine(libFolderPath, libAssembly));
                using var peReader = new PEReader(fs);
                var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);


                var applicationVersionDef = mdReader.TypeDefinitions
                    .GetType(mdReader, "TaleWorlds.Library", "ApplicationVersion");
                var hasChangeSet = applicationVersionDef.GetFields()
                    .Any(f => mdReader.GetString(mdReader.GetFieldDefinition(f).Name) == "DefaultChangeSet");


                var versionVirtualFileAttributeV1 = GetVirtualFileAttributeV1(mdReader);
                if (versionVirtualFileAttributeV1 is null)
                {
                    if (GetVirtualFileAttributeV2(mdReader) is not null)
                        return VersionType.V5;
                    
                    return VersionType.Unknown;
                }

                var attributeReader = mdReader.GetBlobReader(versionVirtualFileAttributeV1.Value.Value);

                if (attributeReader.ReadByte() != 0x01 || attributeReader.ReadByte() != 0x00)
                    throw new NotSupportedException("Custom Attribute prolog is invalid.");

                var fn = attributeReader.ReadSerializedString();

                if (fn != "Version.xml")
                    throw new NotSupportedException("Version field doesn't have Version.xml attribute.");

                var xml = attributeReader.ReadSerializedString() ?? string.Empty;
                using var xmlReader = XmlReader.Create(new StringReader(xml));

                if (!xmlReader.Read())
                    throw new NotSupportedException("Can't parse XML content of Version.xml");

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
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception($"gameFolderPath: {gameFolderPath}, libAssembly: {libAssembly}", ex);
            }
        }
    }
}