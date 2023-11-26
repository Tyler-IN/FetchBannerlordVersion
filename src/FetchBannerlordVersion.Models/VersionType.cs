namespace FetchBannerlordVersion
{
    public enum VersionType
    {
        /// <summary>
        /// Something new or invalid file provided
        /// </summary>
        Unknown,

        /// <summary>
        /// ChangeSet not available, Version.xml has single Version
        /// </summary>
        V1,

        /// <summary>
        /// const int DefaultChangeSet in ApplicationVersion, Version.xml has single Version
        /// </summary>
        V2,

        /// <summary>
        /// const int DefaultChangeSet in ApplicationVersion, Version.xml has Version for singleplayer and multiplayer
        /// </summary>
        V3,

        /// <summary>
        /// ChangeSet and singleplayer/multiplayer in Version.xml
        /// </summary>
        V4,
        
        /// <summary>
        /// const int DefaultChangeSet in ApplicationVersion. VirtualFolders with Shipping_Client
        /// </summary>     
        V5
    }
}