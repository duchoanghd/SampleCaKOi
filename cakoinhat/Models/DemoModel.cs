using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace cakoinhat.Models
{
    public class DemoModel : BaseModel
    {
        public void demo()
        {
            using(var data = new MSSQLDataHelper("ten store", CommandType.StoredProcedure))
            {
                //set tham so truyen vao store
                data.SetParameter("@tham so", SqlDbType.Int, 11);
                //thuc thi store co 4 kieu thuc thi 
                data.ExcuteString(); // tra ve 1 tham so
                data.ExecuteDataTable();// tra ve du lieu kieu datatables
                data.ExecuteNonquery();// khong tra ve gi het
            }
        }
    }
}