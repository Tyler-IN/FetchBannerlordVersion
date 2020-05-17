using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml;

namespace FetchBannerlordVersion {

  public static partial class Program {

    public static string ExtractVersionFromTaleWorldsLibrary(string libPath, out int changeSet) {
      using var fs = File.OpenRead(libPath);
      using var peReader = new System.Reflection.PortableExecutable.PEReader(fs);
      var mdReader = peReader.GetMetadataReader(MetadataReaderOptions.None);

      var vfType = mdReader.TypeDefinitions
        .GetType(mdReader, "TaleWorlds.Library", "VirtualFolders");
      
      var versionVirtualFileAttrib = vfType
        .GetNestedType(mdReader, "Win64_Shipping_Client")
        .GetNestedType(mdReader, "bin")
        .GetNestedType(mdReader, "Parameters")
        .GetField(mdReader, "Version")
        .GetCustomAttributes()
        .Select(ah => mdReader.GetCustomAttribute(ah))
        .First(a => {
          var ctor = a.Constructor;
          var ctorKind = ctor.Kind;
          if (ctorKind != HandleKind.MethodDefinition)
            return false;

          var attribDth = mdReader.GetMethodDefinition((MethodDefinitionHandle) ctor).GetDeclaringType();
          var attribDt = mdReader.GetTypeDefinition(attribDth);
          var name = mdReader.GetString(attribDt.Name);
          return name == "VirtualFileAttribute";
        });

      var attribReader = mdReader.GetBlobReader(versionVirtualFileAttrib.Value);

      if (attribReader.ReadByte() != 0x01 || attribReader.ReadByte() != 0x00)
        throw new NotImplementedException("Custom Attribute prolog is invalid.");

      var fn = attribReader.ReadSerializedString();

      if (fn != "Version.xml")
        throw new NotImplementedException("Version field doesn't have Version.xml attribute.");

      var xml = attribReader.ReadSerializedString();

      using var xmlReader = new XmlTextReader(new StringReader(xml));
      if (!xmlReader.Read())
        throw new NotImplementedException("Can't parse XML content of Version.xml");
      if (xmlReader.LocalName != "Version")
        throw new NotImplementedException("Unexpected document element of Version.xml");
      if (!xmlReader.MoveToAttribute("Value"))
        throw new NotImplementedException("Version.xml Version element missing Value attribute.");

      var appVersion = xmlReader.Value;

      var changeSetHandle = mdReader.TypeDefinitions
        .GetType(mdReader, "TaleWorlds.Library", "ApplicationVersion")
        .GetField(mdReader, "DefaultChangeSet")
        .GetDefaultValue();
      var changeSetBlob = mdReader.GetConstant(changeSetHandle).Value;

      changeSet = mdReader.GetBlobReader(changeSetBlob).ReadInt32();
      return appVersion;
    }

  }

}