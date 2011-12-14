using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using TTTV_Host.ServiceClass;
using TTTV_Host.Models;
namespace TTTV_Host.Contract
{

     [ServiceContract]
    public interface ILevel2Service
    {
        #region Membership Provider
        //[OperationContract]
        [OperationContract]
         bool chkLogin(string username, string password);
        [OperationContract]
        bool chkLoginClient(string username, string password);
        [OperationContract]
        bool changePassword(string username, string oldpass, string newpass);
      
        
         [OperationContract]
         [FaultContract(typeof(CustomException))]
        void InsertUser(string username, string password, string email, int provinceID, int districtID, int detailprovinceID, int roleID, string question, string answear, string companyname, string businesscode, string phone, string mobile, string CMND, string name, int age, string address, string subaddress, out MembershipCreateStatus status);
        #endregion
        #region Register User
         [OperationContract]
        IEnumerable<TTTV__Provinces> getAllProvince();
         [OperationContract]
         IEnumerable<TTTV__Districts> getAllDistrict();
         [OperationContract]
         IEnumerable<TTTV__DetailProvinces> getAllDetailProvince();
         [OperationContract]
         IEnumerable<TTTV__Districts> GetDistrictNameById(int ProvinceId);
         [OperationContract]
         IEnumerable<TTTV__DetailProvinces> GetProvinceDetailNameById(int ProvinceId, int DistrictID);
         [OperationContract]
         bool checkUsernameExist(string username);
         [OperationContract]
         bool checkEmailExist(string email);

         [OperationContract]
         IList<string> GetRolesForUser(string username);

        #endregion
        #region TamTru Class
         [OperationContract]
         IList<TTTV__TamTru> getAllTamTru();
         [OperationContract]
         string Tinh(int? provinceID);
         [OperationContract]
         string Quan(int? provinceID, int? districtID);
         [OperationContract]
         string Phuong(int? provinceID, int? districtID, int? detailID);
                [OperationContract]
           IEnumerable<TTTV__LiDo> getAllLiDo();  // Lay tat ca cac li do
                  [OperationContract]
         IEnumerable<TTTV__QuocTich> getAllQuocTich();     // Lay tat ca quoc tich
                   [OperationContract]
        IEnumerable<TTTV__GiayTo> getAllGiayTo();                                //Lay tat ca giay to
                   [OperationContract]
      IEnumerable<TTTV__Status> getAllStatus();        //status
                   [OperationContract]
                   IEnumerable<TTTV__Type> getAllType();  //type
                   [OperationContract]
                   IEnumerable<TTTV__Roles> getAllRoles();// Roles
         [OperationContract]
                   TTTV__TamTru getTamTruById(int id);
         [OperationContract]
         [FaultContract(typeof(FaultOutOfMemory))]
         void Updatetamtru(int MaTamTru, int? SelectedTypeValue, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, int? SelectedStatusValue, string TT_FullName, string TT_DiaChiThuongTru, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user);
         [OperationContract]
         void UpdatetamtruByHotel(int MaTamTru,  int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, string TT_FullName, string TT_DiaChiThuongTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user);
         [OperationContract]
         void SaveChanges();
         [OperationContract]
         void AddObject(TTTV__TamTru model);
         [OperationContract]
         void DeleteObject(TTTV__TamTru model);
         [OperationContract]
         void Dispose();
         [OperationContract]
         void deleteTamTru(int id);
         [OperationContract]
         void insertAtPolice(int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, int? SelectedProvinceValue, int? SelectedDistrictValue, int? SelectedDetailProvincesValue, string TT_FullName, string TT_DiaChiThuongTru, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac, string user);
         #region USER/////////////////////////////////////////////////
         [OperationContract]
         void insertbyUser(string user, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue, string TT_DiaChiTamTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac);
         [OperationContract]
         void Duyet(int id);
         [OperationContract]
         string Email(string user);
         [OperationContract]
         int? RoleID(string user);
         [OperationContract]
         IList<TTTV__TamTru> getHistory(string username);
#endregion
         #region HOTEL
         [OperationContract]
         void insertbyHotel(string user, int? SelectedGiayToValue, int? SelectedLiDoValue, int? SelectedQuocTichValue,string TT_Fullname, string TT_DiaChiThuongTru, DateTime? TT_NgayDen, DateTime? TT_NgayDi, string TT_Room, string TT_GiayTo, string TT_LiDoKhac);
#endregion
        #endregion
         #region UserList
         [OperationContract]
         IList<TTTV__UserInformation> getUserList();
         [OperationContract]
         TTTV__UserInformation getUserInfoByID(string user);
         [OperationContract]
         TTTV__Users getUserLogin(string username);
         [OperationContract]
         void updateUserInfo(string UserName, string Companyname, string BusinessCode, string Phone, string Moblie, string CMND, string Name, int? Age, string Address, int province, int district, int detail, string email, int? role);
        #endregion





    }
     [DataContract]
     public class CustomException
     {

         [DataMember]
         public string Issue { get; set; }
         [DataMember]
         public string Details { get; set; }
     }
}
[DataContract]
public class FaultOutOfMemory
{

    [DataMember]
    public string Issue { get; set; }
    [DataMember]
    public string Details { get; set; }
}