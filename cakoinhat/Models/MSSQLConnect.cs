using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace cakoinhat.Models
{
    public class MSSQLConnect
    {
        public static string ___SQLConnect = ConfigurationManager.ConnectionStrings["MSSQL_ConnectionString"] != null ? ConfigurationManager.ConnectionStrings["MSSQL_ConnectionString"].ConnectionString : "";
    }
    public class MSSQLDataHelper : IDisposable
    {
        void IDisposable.Dispose()
        {

        }

        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataAdapter _dap;
        private DataTable _dt;

        /// <summary>
        /// Contructor lấy về các kết nối
        /// </summary>
        public MSSQLDataHelper(CommandType _Type, int TimeOut = 30)
        {
            _conn = new SqlConnection(MSSQLConnect.___SQLConnect);
            _cmd = new SqlCommand()
            {
                CommandType = _Type,
                Connection = _conn,
                CommandText = string.Empty,
                CommandTimeout = TimeOut
            };
        }

        /// <summary>
        /// Contructor set kết nối 
        /// </summary>
        public MSSQLDataHelper(string _SQL, CommandType _Type, int TimeOut = 30)
        {

            _conn = new SqlConnection(MSSQLConnect.___SQLConnect);
            _cmd = new SqlCommand()
            {
                CommandType = _Type,
                Connection = _conn,
                CommandText = _SQL,
                CommandTimeout = TimeOut
            };
        }        

        /// <summary>
        /// Contructor set connect mssql
        /// </summary>
        /// <param name="_SQL">SQL Query</param>
        /// <param name="_Type">Type Query</param>
        /// <param name="_Conn">String connect</param>
        /// <param name="TimeOut">Time out</param>
        public MSSQLDataHelper(string _SQL, CommandType _Type, string _Conn, int TimeOut = 30)
        {
            _conn = new SqlConnection(_Conn);
            _cmd = new SqlCommand()
            {
                CommandType = _Type,
                Connection = _conn,
                CommandText = _SQL,
                CommandTimeout = TimeOut
            };
        }

        /// <summary>
        /// Contructor set kết nối transaction
        /// </summary>
        public MSSQLDataHelper(SqlTransaction _tranSaction, string _SQL, CommandType _Type, int TimeOut = 30)
        {
            _cmd = new SqlCommand()
            {
                CommandType = _Type,
                Connection = _tranSaction.Connection,
                Transaction = _tranSaction,
                CommandText = _SQL,
                CommandTimeout = TimeOut
            };
        }

        /// <summary>
        /// Contructor lấy về các kết nối
        /// </summary>
        public MSSQLDataHelper(string _Server, string _User, string _pass, bool _trust, string _DBName, CommandType _Type, int TimeOut = 30)
        {
            _conn = new SqlConnection("Server=" + _Server.Trim() + ";Database=" + _DBName + ";User Id=" + _User.Trim() + ";Password=" + _pass.Trim() + ";Trusted_Connection=" + _trust.ToString() + ";Connect Timeout=" + TimeOut + ";");
            _cmd = new SqlCommand()
            {
                CommandType = _Type,
                Connection = _conn,
                CommandText = string.Empty,
                CommandTimeout = TimeOut
            };
        }

        public MSSQLDataHelper() { }

        /// <summary>
        /// Khởi tạo transaction
        /// </summary>
        /// <returns></returns>
        public SqlTransaction BeginTransaction()
        {
            _conn = new SqlConnection(MSSQLConnect.___SQLConnect);
            _conn.Open();
            return _conn.BeginTransaction();
        }

        /// <summary>
        /// Khởi tạo transaction
        /// </summary>
        /// <returns></returns>
        public SqlTransaction BeginTransaction(SqlConnection connect)
        {
            _conn = connect;
            _conn.Open();
            return _conn.BeginTransaction();
        }

        /// <summary>
        /// Hàm hoàn thành tran
        /// </summary>
        /// <param name="_tran"></param>
        public void CommitTran(SqlTransaction _tran)
        {
            _tran.Commit();
            //_tran.Connection.Close();
            _tran.Dispose();
        }

        /// <summary>
        /// hàm Rollback tran
        /// </summary>
        /// <param name="_tran"></param>
        public void RollbackTran(SqlTransaction _tran)
        {
            _tran.Rollback();
            //_tran.Connection.Close();
            _tran.Dispose();
        }

        /// <summary>
        /// mở kết nối sql
        /// </summary>
        public void OpenConnect()
        {
            try
            {
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();
            }
            catch (Exception e)
            {

                throw new Exception("Lỗi kết nối dữ liệu" + e.Message);
            }

        }

        /// <summary>
        /// Hủy kết nối sql
        /// </summary>
        public void CloseConnect()
        {
            try
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            catch (Exception e)
            {

                throw new Exception("Lỗi kết nối dữ liệu" + e.Message);
            }
        }

        /// <summary>
        /// Khởi tạo Parameter
        /// </summary>
        /// <param name="ParaName">Tên biến sql</param>
        /// <param name="Type">kiểu dữ liệu</param>
        /// <param name="value">giá trị truyền vào</param>
        public void SetParameter(string ParaName, SqlDbType Type, object value)
        {
            _cmd.Parameters.Add(ParaName, Type).Value = value;

        }
        /// <summary>
        /// Xóa toàn bộ param trên cmd
        /// </summary>
        public void ClearParameter()
        {
            _cmd.Parameters.Clear();
        }

        /// <summary>
        /// Khởi tạo Parameter
        /// </summary>
        /// <param name="ParaName">Tên biến sql</param>
        /// <param name="Type">kiểu dữ liệu</param>
        /// <param name="value">giá trị truyền vào</param>
        public void SetParameterWith(string ParaName, SqlDbType Type, object value)
        {
            _cmd.Parameters.AddWithValue(ParaName, Type).Value = value;
        }

        /// <summary>
        /// Gán câu lệnh xử lý trong sql
        /// </summary>
        /// <param name="_CommandText">Hàm cần gọi trong sql</param>
        public void SetComandText(string _CommandText)
        {
            _cmd.CommandText = _CommandText;
        }

        /// <summary>
        /// Xóa các Param
        /// </summary>
        public void ClearParam()
        {
            _cmd.Parameters.Clear();
            _cmd.Dispose();
        }

        /// <summary>
        /// Xử lý dự liệu không trả về giá trị
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonquery()
        {
            try
            {
                OpenConnect();
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Clear();
            }
        }

        /// <summary>
        /// Hàm thực thi lệnh dùng cho transaction
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonqueryTran()
        {
            try
            {
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm xử lý sql trả về 1 SqlDataReader
        /// </summary>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader()
        {
            try
            {
                OpenConnect();
                return _cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Clear();
            }
        }

        public DataTable ExecuteDataTable()
        {
            try
            {
                OpenConnect();
                _dap = new SqlDataAdapter(_cmd);
                _dap.SelectCommand = _cmd;
                _dt = new DataTable();
                _dap.Fill(_dt);
                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Clear();
            }
        }

        public string ExcuteString()
        {
            try
            {
                OpenConnect();
                return _cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Clear();
            }
        }

        /// <summary>
        /// Hàm clear xóa bỏ toàn 
        /// bộ các config trước đó
        /// </summary> 
        public void Clear()
        {
            try
            {
                CloseConnect();
                ClearParam();
                _conn.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}