using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTTV_Host.Models;

namespace TTTV_Host.ServiceClass
{
    public static class Converter
    {
        
        public static LoginDTO ConvertLoginToDTO(TTTV__Users obj)
        {
            LoginDTO objdto = new LoginDTO();
            objdto.Username = obj.Username;
            objdto.Password = obj.Password;
            objdto.ForgotPassAnswer = obj.ForgotPassAnswer;
            objdto.ForgotPassQuestion = obj.ForgotPassQuestion;
            objdto.Email = obj.Email;
            objdto.RoleId = obj.RoleId;
            return objdto;

        }
        public static TTTV__Users ConvertDTOToImage(LoginDTO obj)
        {
            TTTV__Users objdto = new TTTV__Users();
            objdto.Username = obj.Username;
            objdto.Password = obj.Password;
            objdto.ForgotPassAnswer = obj.ForgotPassAnswer;
            objdto.ForgotPassQuestion = obj.ForgotPassQuestion;
            objdto.Email = obj.Email;
            objdto.RoleId = obj.RoleId;
            return objdto;

        }
    }
}