using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Dapper.SqlMapper;

namespace ShopSWE.Service
{
    public class GenericService<T> where T : class
    {
        private string connectionString = string.Empty;
        private static string DBO = string.Empty;
        public GenericService()
        {
            //connectionString = ConfigurationManager.ConnectionStrings["AppConnection"].ConnectionString;
            connectionString = "Data Source=.;Initial Catalog=SHOPSWE;Integrated Security=True;MultipleActiveResultSets=True";

        }

        //Lấy ra 1 dòng dữ liệu (Get By Id)
        public virtual T ExcuteSingle(string storeName, DynamicParameters dyParam)
        {
            T result = null;
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        result = SqlMapper.QuerySingleOrDefault<T>(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return result;
        }
        public virtual async Task<T> ExcuteSingleAsync(string storeName, DynamicParameters dyParam)
        {
            T result = null;
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        result = await SqlMapper.QuerySingleOrDefaultAsync<T>(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return result;
        }
        //Trả về 1 giá trị (VD: Count=1)
        public virtual object ExcuteScalar(string storeName, DynamicParameters dyParam)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        return SqlMapper.ExecuteScalar(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return null;
        }
        public virtual async Task<object> ExcuteScalarAsync(string storeName, DynamicParameters dyParam)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        return await SqlMapper.ExecuteScalarAsync(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return null;
        }
        //Trả về 1 bảng dữ liệu (duy nhất chỉ 1)
        public virtual List<T> ExcuteMany(string storeName, DynamicParameters dyParam)
        {
            List<T> result = new List<T>();
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        var data = SqlMapper.Query<T>(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                        if (data != null)
                        {
                            result = data.ToList();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return result;
        }
        public virtual async Task<IEnumerable<T>> ExcuteManyAsync(string storeName, DynamicParameters dyParam)
        {
            List<T> result = new List<T>();
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        var data = await SqlMapper.QueryAsync<T>(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                        if (data != null)
                        {
                            result = data.ToList();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return result;
        }
        //Không trả về cái gì (thưc hiện việc thêm , sửa , xóa)
        public virtual int ExcuteNoneQuery(string storeName, DynamicParameters dyParam)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        int result = SqlMapper.Execute(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                        return result;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return 0;
        }
        public virtual async Task<int> ExcuteNoneQueryAsync(string storeName, DynamicParameters dyParam)
        {
            int result = -1;
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        result = await SqlMapper.ExecuteAsync(conn, storeName, param: dyParam, commandType: CommandType.StoredProcedure);
                        return result;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }
            return result;
        }
        //Trả về nhiều bảng dữ liệu (1 hoặc nhiều)
        public virtual void ExcuteMultiple(string storeName, DynamicParameters dyParam, Action<GridReader> action)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        using (var multi = conn.QueryMultiple(storeName, dyParam, null, null, CommandType.StoredProcedure))
                        {
                            action(multi);
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }

        }
        public virtual async Task ExcuteMultipleAsync(string storeName, DynamicParameters dyParam, Action<GridReader> action)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        using (var multi = await conn.QueryMultipleAsync(storeName, dyParam, null, null, CommandType.StoredProcedure))
                        {
                            action(multi);
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                throw new SqlDataException(ex.Message);
            }

        }
        public IDbConnection GetConnection()
        {
            if (connectionString != null)
            {
                var conn = new SqlConnection(connectionString);
                return conn;
            }
            return null;
        }
    }
    public class SqlDataException : Exception
    {
        public SqlDataException(string message) : base(message)
        {

        }
    }
}