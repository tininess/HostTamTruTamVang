using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using TTTV_Host.Classes;
using TTTV_Host.Models;
using TTTV_Host.ServiceClass;

using System.Data.Services.Client;

namespace TTTV_Host.Contract
{
    public class Level2Service : ILevel2Service
    {
        UserDataAccessConsolidator access = new UserDataAccessConsolidator();
        TTTVEntities db = new TTTVEntities();
        #region Membership
        public bool chkLogin(string username, string password)    // Kiem Tra Dang Nhap
        {

            var user = access.getUser(username);

            if (string.IsNullOrEmpty(password.Trim())) return false;
            if (user == null) return false;

            //string hash = EncryptPassword(password);
            var pass = user.Password;
            if (pass == password)
            {
                return true;
            }
            else return false;
        }
        public void InsertUser(string username, string password, string email, int provinceID, int districtID, int detailprovinceID, int roleID, string question, string answear, string companyname, string businesscode, string phone, string mobile, string CMND, string name, int age, string address, string subaddress, out MembershipCreateStatus status) // Tao user    
        {
            try
            {

                TTTV__Users user = new TTTV__Users();
                TTTV__UserInformation info = new TTTV__UserInformation();

                user.Username = username;
                user.Password = password;
                user.Email = email;
                user.RoleId = roleID;
                user.ForgotPassQuestion = question;
                user.ForgotPassAnswer = answear;
                info.Username = username;
                info.ProvinceID = provinceID;
                info.DistrictID = districtID;
                info.ProvinceDetailID = detailprovinceID;
                info.CompanyName = companyname;
                info.BusinessCode = businesscode;
                info.Phone = phone;
                info.Mobile = mobile;
                info.PersonCode = CMND;
                info.UserFullName = name;
                info.Age = age;
                info.Address = address;
                info.SubAddress = subaddress;
                db.TTTV__Users.AddObject(user);
                db.TTTV__UserInformation.AddObject(info);
                db.SaveChanges();
                status = MembershipCreateStatus.Success;

            }
            catch (Exception ex)
            {
                CustomException cex = new CustomException();
                cex.Details = ex.Message;
                cex.Issue = "Trùng tên đăng nhập";
                //status = MembershipCreateStatus.ProviderError;
                throw new FaultException<CustomException>(cex, new FaultReason(cex.Issue));

            }

        }
        #endregion
        #region Register Field
        public IEnumerable<TTTV__Provinces> getAllProvince()
        {
            var data = db.TTTV__Provinces.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__Districts> getAllDistrict()
        {
            var data = db.TTTV__Districts.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__DetailProvinces> getAllDetailProvince()
        {
            var data = db.TTTV__DetailProvinces.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }

        public IEnumerable<TTTV__Districts> GetDistrictNameById(int ProvinceId)
        {
            return db.TTTV__Districts.Where(e => e.ProvinceID == ProvinceId).Select(e => e).ToList();
        }
        public IEnumerable<TTTV__DetailProvinces> GetProvinceDetailNameById(int ProvinceId, int DistrictID)
        {
            return db.TTTV__DetailProvinces.Where(e => e.DistrictID == DistrictID && e.ProvinceID == ProvinceId).Select(e => e).ToList();
        }
        public bool checkUsernameExist(string username)
        {
            var check = db.TTTV__Users.Where(e => e.Username == username).Select(e => e);
            if (check.Count() > 0)
                return true;
            else
                return false;
        }
        public bool checkEmailExist(string email)
        {
            var check = db.TTTV__Users.Where(e => e.Email == email).Select(e => e);
            if (check.Count() > 0)
                return true;
            else
                return false;
        }
        #endregion
        #region Roles
        public IList<string> GetRolesForUser(string username)
        {

            var result = (
                         from role in db.TTTV__Roles
                         join user in db.TTTV__Users on role.RoleId equals user.RoleId
                         where user.Username == username
                         select role.RoleName).ToList();

            return result;
        }
        #endregion
        #region Tam Tru Contract
        public IList<TTTV__TamTru> getAllTamTru()                 // Lay tat ca gia tri cua tam tru theo list
        {
            var tttv__tamtru = db.TTTV__TamTru.Include("TTTV__GiayTo").Include("TTTV__LiDo").Include("TTTV__QuocTich").Include("TTTV__Status").Include("TTTV__Type").Include("TTTV__Users").ToList();
            return tttv__tamtru;
        }
        public string Tinh(int? provinceID)
        {
            return db.TTTV__Provinces.Where(e => e.ProvinceID == provinceID).Select(e => e.ProvinceName).Single();
        }

        public string Quan(int? provinceID, int? districtID)
        {
            return db.TTTV__Districts.Where(e => e.ProvinceID == provinceID && e.DistrictID == districtID).Select(e => e.DistrictName).Single();
        }

        public string Phuong(int? provinceID, int? districtID, int? detailID)
        {
            return db.TTTV__DetailProvinces.Where(e => e.ProvinceID == provinceID && e.DistrictID == districtID && e.DetailID == detailID).Select(e => e.DetailName).Single();
        }

        public IEnumerable<TTTV__LiDo> getAllLiDo()  // Lay tat ca cac li do
        {
            var data = db.TTTV__LiDo.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__QuocTich> getAllQuocTich()     // Lay tat ca quoc tich
        {
            var data = db.TTTV__QuocTich.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__GiayTo> getAllGiayTo()                                //Lay tat ca giay to
        {
            var data = db.TTTV__GiayTo.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__Status> getAllStatus()        //status
        {
            var data = db.TTTV__Status.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
        public IEnumerable<TTTV__Type> getAllType()  //type
        {
            var data = db.TTTV__Type.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }

        public TTTV__TamTru getTamTruById(int id)
        {
            TTTV__TamTru model = db.TTTV__TamTru.Single(t => t.ID_TamTru == id);
                return model;
        }
        #endregion
        #region Insert,update,delete LinQ Command
        //public IQueryable<TTTVEntities> tamtru {
             
        //  return   TTTVEntities.SaveChanges();
        //}
      //  public void OnChangeBackups(TTTVEntities t, UpdateOperations a) {
        public void SaveChanges()// Save database change
        {

            db.SaveChanges();
        }
        public void AddObject(TTTV__TamTru model)
        {
            db.TTTV__TamTru.AddObject(model);
            
        }
        public void  DeleteObject(TTTV__TamTru model)
             {
         db.TTTV__TamTru.DeleteObject(model);
         
            }
        public void Dispose()
        {
            db.Dispose();
        }
           
        #endregion
        public void Updatetamtru(int MaTamTru, int? SelectedTypeValue, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, int? SelectedStatusValue, string TT_FullName, string TT_DiaChiThuongTru, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user)
        {

            try
            {
                TTTV__TamTru model = getTamTruById(MaTamTru);
                model.ID_Type = SelectedTypeValue;
                model.TT_FullName = TT_FullName;
                model.TT_NgayDen = TT_NgayDen;
                model.TT_NgayDi = TT_NgayDi;
                model.TT_DiaChiThuongTru = TT_DiaChiThuongTru;
                model.TT_DiaChiTamTru = TT_DiaChiTamTru;
                model.TT_Room = TT_Room;
                model.TT_GiayTo = TT_GiayTo;
                model.ID_GiayTo = SelectedGiayToValue;
                model.ID_LiDo = SelectedLiDoValue;
                model.ID_QuocTich = SelectedQuocTichValue;
                model.ID_Status = SelectedStatusValue;
                model.TT_LiDoKhac = TT_LiDoKhac;
                model.TaiKhoanDangKi = user;
                db.SaveChanges();
            }
            catch (OutOfMemoryException ex)
            {
                FaultOutOfMemory fault = new FaultOutOfMemory();
                fault.Details = ex.Message;
                fault.Issue = "Quá Tải";
                //status = MembershipCreateStatus.ProviderError;
                throw new FaultException<FaultOutOfMemory>(fault, new FaultReason(fault.Issue));
            }
        }
        public void deleteTamTru(int id)
        {
            TTTV__TamTru item = getTamTruById(id);
            db.DeleteObject(item);
            db.SaveChanges();

        }
        public void insertAtPolice(int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, int? SelectedProvinceValue, int? SelectedDistrictValue, int? SelectedDetailProvincesValue, string TT_FullName, string TT_DiaChiThuongTru, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user)
        {
            TTTV__TamTru model = new TTTV__TamTru();
            model.ID_Type = 3;
            model.ID_GiayTo = SelectedGiayToValue;
            model.ID_LiDo = SelectedLiDoValue;
            model.ID_QuocTich = SelectedQuocTichValue;
            model.ID_Status = 1;
            model.TT_FullName = TT_FullName;
            model.TT_NgayDen = TT_NgayDen;
            model.TT_NgayDi = TT_NgayDi;
            model.TT_DiaChiThuongTru = TT_DiaChiThuongTru;
            model.TT_DiaChiTamTru = TT_DiaChiTamTru;
            model.TT_Room = TT_Room;
            model.TT_GiayTo = TT_GiayTo;
            model.TT_LiDoKhac = TT_LiDoKhac;
            model.TaiKhoanDangKi = user;
            model.ProvinceID = SelectedProvinceValue;
            model.DistrictID = SelectedDistrictValue;
            model.DetailProvinceID = SelectedDetailProvincesValue;
            db.TTTV__TamTru.AddObject(model);
            db.SaveChanges();
        }

        #region Don Vi Service
        public bool chkLoginClient(string username, string password)
        {
            var user = access.getUser(username);

            if (string.IsNullOrEmpty(password.Trim())) return false;
            if (user == null) return false;

            //string hash = EncryptPassword(password);
            var pass = user.Password;
            var role = user.RoleId;
            if (pass == password && role==2)
            {
                return true;
            }
            else return false;
        }
        public bool changePassword(string username, string oldpass, string newpass)
        {
            var user = access.getUser(username);
            var pass = user.Password;
            if (pass == oldpass)
            {
                access.ChangePass(username, newpass);
                return true;
            }
            else
                return false;
        }
        public void insertbyUser(string user, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac)
        {
            TTTV__TamTru model = new TTTV__TamTru();
            var info = db.TTTV__UserInformation.Where(e => e.Username == user);
            model.ID_Type = 2;
            model.ID_GiayTo = SelectedGiayToValue;
            model.ID_LiDo = SelectedLiDoValue;
            model.ID_QuocTich = SelectedQuocTichValue;
            model.ID_Status = 3;
            model.TT_FullName = info.Select(e => e.UserFullName).Single();
            model.TT_NgayDen = TT_NgayDen;
            model.TT_NgayDi = TT_NgayDi;
            model.TT_DiaChiThuongTru = info.Select(e => e.Address).Single();
            model.TT_DiaChiTamTru = TT_DiaChiTamTru;
            model.TT_Room = TT_Room;
            model.TT_GiayTo = TT_GiayTo;
            model.TT_LiDoKhac = TT_LiDoKhac;
            model.TaiKhoanDangKi = user;
            model.ProvinceID = info.Select(e => e.ProvinceID).Single();
            model.DistrictID = info.Select(e => e.DistrictID).Single();
            model.DetailProvinceID = info.Select(e => e.ProvinceDetailID).Single();
            db.TTTV__TamTru.AddObject(model);
            db.SaveChanges();
        }

        public void Duyet(int id)
        {
            TTTV__TamTru model = db.TTTV__TamTru.Single(t => t.ID_TamTru == id);
            model.ID_Status = 1;
            db.SaveChanges();

        }

        public string Email(string user)
        {
            string email = db.TTTV__Users.Where(e => e.Username == user).Select(e => e.Email).Single();
            return email;
        }
        public int? RoleID(string user)
        {
            int? role = db.TTTV__Users.Where(e => e.Username == user).Select(e => e.RoleId).Single();
            return role;
        }
        public IList<TTTV__TamTru> getHistory(string username)
        {
            IList<TTTV__TamTru> list = db.TTTV__TamTru.Where(e => e.TaiKhoanDangKi == username).ToList();
            return list.Count() > 0 ? list : null;

        }
        #endregion

        #region EDIT USER

        public IList<TTTV__UserInformation> getUserList()
        {
            IList<TTTV__UserInformation> list = db.TTTV__UserInformation.ToList();
            return list.Count() > 0 ? list : null;
        }
        public TTTV__UserInformation getUserInfoByID(string user)
        {
            TTTV__UserInformation item = db.TTTV__UserInformation.Where(e => e.Username == user).Select(e => e).Single();
            if (item != null) return item;
            return null;
        }
        public TTTV__Users getUserLogin(string username)  // Lấy thông tin đăng nhập
        {
            var data = db.TTTV__Users.Where(e => e.Username == username);
            return data.Count() > 0 ? data.Single() : null;
        }
        public IEnumerable<TTTV__Roles> getAllRoles()
        {
            var data = db.TTTV__Roles.Select(e => e).ToList();
            return data.Count() > 0 ? data : null;
        }
      
        public void updateUserInfo(string UserName, string Companyname, string BusinessCode, string Phone, string Moblie, string CMND, string Name, int? Age, string Address, int province, int district, int detail, string email, int? role)
        {
            TTTV__UserInformation user = db.TTTV__UserInformation.Single(e => e.Username == UserName);
            TTTV__Users login = db.TTTV__Users.Single(e => e.Username == UserName);
            user.CompanyName = Companyname;
            user.BusinessCode = BusinessCode;
            user.Phone = Phone;
            user.Mobile = Moblie;
            user.PersonCode = CMND;
            user.UserFullName = Name;
            user.Age = Age;
            login.RoleId = role;
            login.Email = email;
            db.SaveChanges();
        }
        #endregion
        #region HOTEL
        public void insertbyHotel(string user, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue,string TT_Fullname, string TT_DiaChiThuongTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac)
        {
            TTTV__TamTru model = new TTTV__TamTru();
            var info = db.TTTV__UserInformation.Where(e => e.Username == user);
            model.ID_Type = 1;
            model.ID_GiayTo = SelectedGiayToValue;
            model.ID_LiDo = SelectedLiDoValue;
            model.ID_QuocTich = SelectedQuocTichValue;
            model.ID_Status = 1;
            model.TT_FullName = TT_Fullname;
            model.TT_NgayDen = TT_NgayDen;
            model.TT_NgayDi = TT_NgayDi;
            model.TT_DiaChiThuongTru = TT_DiaChiThuongTru;
            model.TT_DiaChiTamTru = info.Select(e => e.Address).Single();
            model.TT_Room = TT_Room;
            model.TT_GiayTo = TT_GiayTo;
            model.TT_LiDoKhac = TT_LiDoKhac;
            model.TaiKhoanDangKi = user;
            model.ProvinceID = info.Select(e => e.ProvinceID).Single();
            model.DistrictID = info.Select(e => e.DistrictID).Single();
            model.DetailProvinceID = info.Select(e => e.ProvinceDetailID).Single();
            db.TTTV__TamTru.AddObject(model);
            db.SaveChanges();
        }
        public void UpdatetamtruByHotel(int MaTamTru, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, string TT_FullName, string TT_DiaChiThuongTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user)
        {
            TTTV__TamTru model = getTamTruById(MaTamTru);
        
            model.TT_FullName = TT_FullName;
            model.TT_NgayDen = TT_NgayDen;
            model.TT_NgayDi = TT_NgayDi;
            model.TT_DiaChiThuongTru = TT_DiaChiThuongTru; 
            model.TT_Room = TT_Room;
            model.TT_GiayTo = TT_GiayTo;
            model.ID_GiayTo = SelectedGiayToValue;
            model.ID_LiDo = SelectedLiDoValue;
            model.ID_QuocTich = SelectedQuocTichValue;
            model.TT_LiDoKhac = TT_LiDoKhac;
            model.TaiKhoanDangKi = user;
            db.SaveChanges();
        }
        #endregion
    }
}