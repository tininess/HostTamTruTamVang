using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TTTV_Host.Models;

namespace TTTV_Host.Classes
{
    public class UserDataAccessConsolidator

    {
        TTTVEntities db = new TTTVEntities();
        public TTTV__Users getUser(string username)  // Lấy thông tin đăng nhập
        {
            var data = db.TTTV__Users.Where(e => e.Username == username);
            return data.Count() > 0 ? data.Single() : null;
        }

        public void ChangePass(string username,string newpass)
        {
            TTTV__Users user = db.TTTV__Users.Single(e=>e.Username==username);
            user.Password = newpass;
            db.SaveChanges();
        }

        // Insert User vào Database.
        public void InsertUser(string username, string password, string email, int provinceID, int districtID, int detailprovinceID,int roleID,out MembershipCreateStatus status)
        {
            try
            {
                TTTV__Users user = new TTTV__Users();
                TTTV__UserInformation info = new TTTV__UserInformation();

                user.Username = username;
                user.Password = password;
                user.Email = email;
                user.RoleId = roleID;
                info.Username = username;
                info.ProvinceID = provinceID;
                info.DistrictID = districtID;
                info.ProvinceDetailID = detailprovinceID;
                db.TTTV__Users.AddObject(user);
                db.TTTV__UserInformation.AddObject(info);
                db.SaveChanges();
                status = MembershipCreateStatus.Success;
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                throw ex;
            }

        }
        // 2 Hàm lấy dữ liệu từ Dropdownlist
        public IEnumerable<TTTV__Districts> GetDistrictNameById(int ProvinceId)
        {
            return db.TTTV__Districts.Where(e=>e.ProvinceID==ProvinceId).Select(e => e).ToList();
        }
        public IEnumerable<TTTV__DetailProvinces> GetProvinceDetailNameById(int ProvinceId,int DistrictID)
        {
            return db.TTTV__DetailProvinces.Where(e=>e.DistrictID==DistrictID && e.ProvinceID==ProvinceId).Select(e => e).ToList();
        }
        /////////////////////////////////////

    }
}