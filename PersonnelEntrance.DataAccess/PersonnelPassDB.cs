using Dapper;
using PersonnelEntrance.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PersonnelEntrance.DataAccess
{
    public class PersonnelPassDB : IPersonnelPassDB
    {
        private readonly IDbConnection _dbConnection;

        public PersonnelPassDB(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Delete(Guid id)
        {
            string SqlDeleteCommand= "DELETE from PersonnelPass where PassId = @id;";
            int rowCount;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                rowCount = connection.Execute(
                    SqlDeleteCommand, new
                    {
                        id= id
                    });
            }
            return rowCount;
        }

        public int Insert(PersonnelPass personnelPass)
        {
            string SqlInsertCommand = "INSERT INTO PersonnelPass " +
                " (PassId, EmployeeName, PassTime, PassType)" +
                " Values (@PassId, @EmployeeName, @PassTime, @PassType);";
            int rowCount;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                rowCount = connection.Execute(
                    SqlInsertCommand, new { 
                        PassId = personnelPass.passId,
                        EmployeeName = personnelPass.employeeName,
                        PassTime = personnelPass.passTime,
                        PassType = (int)personnelPass.passType
                });
            }
            return rowCount;
        }

        public PersonnelPass Select(Guid id)
        {
            string SqlSelectCommand = "SELECT * from PersonnelPass where PassId = @PassId;";
            PersonnelPass personnelPass;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                personnelPass = connection.QuerySingleOrDefault<PersonnelPass>(
                    SqlSelectCommand, new { PassId = id });
            }
            return personnelPass;
        }

        public IEnumerable<PersonnelPass> SelectAll()
        {
            string SqlSelectAllCommand = "SELECT * FROM PersonnelPass";
            IEnumerable<PersonnelPass> personnelPass;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                personnelPass = connection.Query<PersonnelPass>(
                    SqlSelectAllCommand);
            }
            return personnelPass;
        }

        public int Update(Guid id, PersonnelPass personnelPass)
        {
            string SqlUpdateCommand = "UPDATE PersonnelPass" +
                " SET EmployeeName = @EmployeeName, " +
                " PassTime= @PassTime, PassType = @PassType" +
                " WHERE PassId = @Id";
            int rowCount;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                rowCount = connection.Execute(
                    SqlUpdateCommand, new
                    {
                        Id = id,
                        EmployeeName = personnelPass.employeeName,
                        PassTime = personnelPass.passTime,
                        PassType = (int)personnelPass.passType
                    });
            }
            return rowCount;
        }

        public int GetCount(String EmployeeName)
        {
            string SqlSelectCountCommand = "SELECT COUNT(EmployeeName) FROM PersonnelPass" +
                " WHERE EmployeeName in (@EmployeeName)";
            int repCount;

            using (var connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                repCount = connection.ExecuteScalar<int>(
                    SqlSelectCountCommand, new
                    {
                        EmployeeName = EmployeeName
                    });
            }
            return repCount;
        }
    }
}
