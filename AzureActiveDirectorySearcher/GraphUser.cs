using System.Net;

namespace AzureActiveDirectorySearcher
{
    public class GraphQueryResult
    {
        public GraphUser[] Value { get; set; }
    }

    public class GraphUser
    {
        public string Mail { get; set; }
        public string Kerberos => Extension_6ea18d073af64cdaa5d7c4ff39fe5ca4_extensionAttribute13;
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string TelephoneNumber { get; set; }
        public string JobTitle { get; set; }

        /// <summary>
        /// Name of the extension attribute which holds kerberos
        /// </summary>
        public string Extension_6ea18d073af64cdaa5d7c4ff39fe5ca4_extensionAttribute13 { get; set; }

        public static string GetExtensionForKerberos()
        {
            return "extension_6ea18d073af64cdaa5d7c4ff39fe5ca4_extensionAttribute13";
        }
    }
}
