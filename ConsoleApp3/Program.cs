using System;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
namespace Addcolumn
{
    class connectspo
    {
        static void Main(string[] args)
        {
            string userName = "rangasad@bms.com";
            Console.WriteLine("Enter your password.");
            SecureString password = getpassword();
            using (var ctx = new ClientContext("https://sites.bms.com/sites/miketest"))
            {
                ctx.Credentials = new SharePointOnlineCredentials(userName, password);
                Web subsite = ctx.Web;
                ctx.Load(subsite);
                ctx.ExecuteQuery();
                List userlist = subsite.Lists.GetByTitle("Clinical");
                ctx.Load(subsite);
                ctx.ExecuteQuery();
                ContentType ct = userlist.ContentTypes.GetById("0x010047A9C2CCA715F64E96F1FCA3603F845A");
                ctx.Load(ct);
                ctx.ExecuteQuery();
                Field column = userlist.Fields.GetByInternalNameOrTitle("console4");
                ctx.Load(column);
                ctx.ExecuteQuery();
                Console.WriteLine("Title: " + subsite.Title + "; URL: " + subsite.Url + "; column: " + column.InternalName);
                Console.ReadLine();
                FieldLinkCollection addcolumntoct = ct.FieldLinks;
                ctx.Load(addcolumntoct);
                ctx.ExecuteQuery();
                foreach (var item in addcolumntoct)
                {
                    if (item.Name == "console4")
                        return;
                    }
                FieldLinkCreationInformation link = new FieldLinkCreationInformation();
                link.Field = column;
                addcolumntoct.Add(link);
                ct.Update(false);
                ctx.ExecuteQuery();
            }
            }

        private static SecureString getpassword()
        {
            ConsoleKeyInfo info;
            SecureString securepassword = new SecureString();
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    securepassword.AppendChar(info.KeyChar);
                }

            }
            while (info.Key != ConsoleKey.Enter);
            return securepassword;
        }
    }
}
