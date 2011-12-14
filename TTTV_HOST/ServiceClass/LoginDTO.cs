using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace TTTV_Host.ServiceClass
{
   // [DataContract]
    public class LoginDTO
    {
   // [DataMember]
    public string  Username {get;set;}
    //[DataMember]
	public string Password {get;set;}
    //[DataMember]
	public string ForgotPassQuestion{get;set;}
   // [DataMember]
	public string ForgotPassAnswer {get;set;}
    //[DataMember]
	public string Email {get;set;}
   // [DataMember]
	public int?  RoleId {get;set;}
    }

  
}