using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for GetPlayer
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class GetPlayer : System.Web.Services.WebService {

    public GetPlayer () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); s
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    
}
