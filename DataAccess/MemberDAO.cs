using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO : BaseDAL
    {
        private static MemberDAO instance = null;
        private static readonly Object instanceLock = new object();
        private MemberDAO(){ }
        public static MemberDAO Instance
        {
            get {
                lock (instanceLock)
                {
                    if (instance == null) instance = new MemberDAO();
                }
                return instance; 
            }
        }

        public IEnumerable<MemberObject> GetMemberList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT MemberID, CompanyName, Email, Password, City, Country, IsAdmin FROM Member";
            List<MemberObject> list = new List<MemberObject> ();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    MemberObject member = new MemberObject
                    {
                        MemberID = dataReader.GetInt32(0),
                        MemberName = dataReader.GetString(1),
                        Email = dataReader.GetString(2),
                        Password = dataReader.GetString(3),
                        City = dataReader.GetString(4),
                        Country = dataReader.GetString(5),
                        Admin = dataReader.GetBoolean(6)
                    };
                    list.Add(member);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            } 
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        public MemberObject GetMemberByID(int id)
        {
            MemberObject member = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT CompanyName, Email, Password, City, Country, IsAdmin"
                             + " FROM Member WHERE MemberID = @MemberID";
            try
            {
                var param = dataProvider.CreateParameter("@MemberID", 4, id, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    member = new MemberObject
                    {
                        MemberID = id,
                        MemberName = dataReader.GetString(0),
                        Email = dataReader.GetString(1),
                        Password = dataReader.GetString(2),
                        City = dataReader.GetString(3),
                        Country = dataReader.GetString(4),
                        Admin = dataReader.GetBoolean(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                CloseConnection();
            }
            return member;
        }
        public int GetSeed()
        {          
            return GetSeed("Member");
        }
        public void AddMember(MemberObject member)
        {
            try
            {
                string SQLUpdate = "INSERT Member VALUES (@Email, @CompanyName, @City, @Country, @Password, @IsAdmin)";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@Email", 100, member.Email, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@CompanyName", 40, member.MemberName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@City", 15, member.City, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Country", 15, member.Country, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Password", 30, member.Password, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@IsAdmin", 1, member.Admin, DbType.Boolean));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void UpdateMember(MemberObject member)
        {
            try
            {
                string SQLUpdate = "UPDATE Member SET Email = @Email, CompanyName = @CompanyName," 
                                 + " City = @City, Country = @Country, Password = @Password WHERE MemberId = @MemberID";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@Email", 100, member.Email, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@CompanyName", 40, member.MemberName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@City", 15, member.City, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Country", 15, member.Country, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Password", 30, member.Password, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@MemberID", 4, member.MemberID, DbType.Int32));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void RemoveMember(int MemberID)
        {
            try
            {
                string SQLUpdate = "DELETE Member WHERE MemberId = @MemberID";
                var parameter = dataProvider.CreateParameter("@MemberID", 4, MemberID, DbType.Int32);
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
