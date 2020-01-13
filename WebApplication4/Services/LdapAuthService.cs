using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class LdapAuthService : IAuthenticationService
    {

        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";

        //private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthService()
        {
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
        }

        public LdapUser Login(string username, string password)
        {
            _connection.Connect("ldap.dcs.aber.ac.uk", 389);
            _connection.Bind($"cn={username},ou=People,dc=dcs,dc=aber,dc=ac,dc=uk", "");
            Debug.WriteLine("Is this connected...? -- " + _connection);

            var groups = new HashSet<string>();

            if(_connection.Bound)
            {
                Debug.WriteLine("Connection not bound");
                return null;
            }
            var searchBase = "DC=dcs,DC=aber,DC=ac,DC=uk";
            //var filter = $"(&(objectClass=group)(cn=students))";
            //var filter = string.Format("(&(objectClass=user)(uid={0}))", username);
            //var filter = $"(objectClass=*)";
            var filter = $"(&(objectclass=*)(cn={username}))";
            var search = _connection.Search(searchBase, LdapConnection.SCOPE_SUB, filter, null, false);
            Debug.WriteLine(search.Count);
            while (search.hasMore())
            {
                var nextEntry = search.next();
                groups.Add(nextEntry.DN);
            }

            foreach(var entry in groups)
            {
                Debug.WriteLine(entry);
            }

            Debug.WriteLine(groups.Count); //If greater than 0, result round
            /*var searchFilter = string.Format("(objectClass=*)", username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var user = result.next();
                if (user != null)
                {
                    _connection.Bind(user.DN, password);
                    if (_connection.Bound)
                    { }
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();*/
            return null;
        }
    }
}
