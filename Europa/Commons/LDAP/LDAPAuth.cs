using System.DirectoryServices;

namespace Europa.Commons.LDAP
{
    public class LDAPAuth
    {
        private string _path;

        public LDAPAuth(string path)
        {
            _path = path;
        }

        public LDAPModel Autenticate(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(&(objectClass=user)(anr=" + username + "))";
                // specify which property values to return in the search
                search.PropertiesToLoad.Add("givenName");   // first name
                search.PropertiesToLoad.Add("sn");          // last name
                search.PropertiesToLoad.Add("mail");        // smtp mail address

                // perform the search
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return null;
                }

                LDAPModel model = new LDAPModel();
                if (result.Properties["givenName"].Count > 0)
                {
                    model.Nome = (string)result.Properties["givenName"][0];
                }
                if (result.Properties["sn"].Count > 0)
                {
                    model.Sobrenome = (string)result.Properties["sn"][0];
                }
                if (result.Properties["mail"].Count > 0)
                {
                    model.Email = (string)result.Properties["mail"][0];
                }
                model.Login = username;

                return model;
            }
            catch (DirectoryServicesCOMException)
            {
                return null;
            }
        }
    }
}
