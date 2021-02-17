using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using dotnetpostgre.Entities;

namespace dotnetpostgre
{
    public class DataLayer
    {

        private string connString;
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();

        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
        }


        public DataTable getdata()
        {
            connString = GetConnectionString();
            string sql = "SELECT * FROM public.\"User\"";
            using (var connection = new NpgsqlConnection(connString))
            {
                var users = connection.Query(sql).FirstOrDefault();

            }
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // data adapter making request from our connection
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                // i always reset DataSet before i do
                // something with it.... i don't know why :-)
                ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                dt = ds.Tables[0];
                // connect grid to DataTable
                //dataGridView1.DataSource = dt;
                // since we only showing the result we don't need connection anymore
                conn.Close();
            }

            return dt;
        }


        public IEnumerable<blog> getblogs()
        {
            IEnumerable<blog> blog;
            connString = GetConnectionString();
            string sql = "SELECT id, title, createdon, blogdetails, author, description FROM public.Blog order by createdon desc";
            using (var connection = new NpgsqlConnection(connString))
            {
                blog = connection.Query<blog>(sql).ToList();

            }

            return blog;
        }


        public blog getblog(int id)
        {
            blog blog;
            connString = GetConnectionString();
            string sql = "SELECT id, title, createdon, blogdetails, author, description FROM public.Blog where id =" + id;
            using (var connection = new NpgsqlConnection(connString))
            {
                blog = connection.Query<blog>(sql).FirstOrDefault();

            }

            return blog;
        }

        public void saveBlog(blog b)
        {
            try
            {
                connString = GetConnectionString();
                using (var conn = new NpgsqlConnection(connString))
                {
                    string query = string.Empty;
                    if (b.id == 0)
                    {
                        query = "insert into public.blog(title, blogdetails, author, description) values(@title, @blogdetails, @author, @description)";
                        int id = conn.Execute(query, b);
                    }
                    else
                    {
                        query = "update public.blog set blogdetails=@blogdetails, title=@title, author=@author, description=@description where id=@id";
                        int updatedRows = conn.Execute(query, b);
                    }

                }
            }
            catch (Exception ex) {
            }
        }

        public void deleteBlog(int id)
        {
            try
            {
                string query = string.Empty;
                connString = GetConnectionString();
                using (var conn = new NpgsqlConnection(connString))
                {
                   
                        query = "delete from public.blog where id = @id";
                        conn.Execute(query, id);

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
