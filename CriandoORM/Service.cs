using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
namespace CriandoORM
{
    public sealed class Service
    {
        private CType cType;

        public Service(CType _cType)
        {
            this.cType = _cType;
        }

        public void Save()
        {
            using (SqlConnection conn = new SqlConnection(this.cType.ConnectionString))
            {
                List<string> cols = new List<string>();
                List<object> values = new List<object>();

                foreach (var p in this.cType.GetType().GetProperties())
                {
                    if (p.GetValue(this.cType) == null) continue;

                    TableAttribute[] propertiesAttributes = (TableAttribute[])p.GetCustomAttributes(typeof(TableAttribute), false);
                    if (propertiesAttributes != null && propertiesAttributes.Length > 0)
                    {
                        if (!propertiesAttributes[0].IsNotOnDatabase && string.IsNullOrEmpty(propertiesAttributes[0].PrimaryKey))
                        {
                            cols.Add(p.Name);
                            values.Add(p.GetValue(this.cType));
                        }

                    }
                    else
                    {
                        cols.Add(p.Name);
                        values.Add(p.GetValue(this.cType));
                    }
                }

                string table = this.GetTableName();


                string sql = string.Empty;
                if (this.cType.Id == 0)
                {
                    sql = $"insert into {table} (";
                    sql += string.Join(',', cols);
                    sql += ") values (";
                    sql += "@" + string.Join(", @", cols);
                    sql += " )";
                }
                else
                {
                    sql = $"update {table} set";

                    var colsUpdate = new List<string>();

                    foreach (var col in cols)
                    {
                        colsUpdate.Add($"{col} = @${col}");
                    }
                    sql += string.Join(",", colsUpdate);
                    sql += $"where {this.GetPkName()} = {this.cType.Id}";

                }




                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                for (var i = 0; i < cols.Count; i++)
                {
                    var value = values[i];
                    var col = cols[i];

                    cmd.Parameters.Add($"@{col}", GetDbType(value));

                    cmd.Parameters[$"@{col}"].Value = value;
                }


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }

        private string GetTableName()
        {
            var table = $"{this.cType.GetType().Name.ToLower()}s";

            TableAttribute[] tableAtributes = (TableAttribute[])this.cType.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            if (tableAtributes != null && tableAtributes.Length > 0)
            {
                table = tableAtributes[0].Name;
            }
            return table;
        }

        private SqlDbType GetDbType(object value)
        {
            var result = SqlDbType.VarChar;
            try
            {
                Type type = value.GetType();

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Object:
                        result = SqlDbType.Variant;
                        break;
                    case TypeCode.Boolean:
                        result = SqlDbType.Bit;
                        break;
                    case TypeCode.Char:
                        result = SqlDbType.NChar;
                        break;
                    case TypeCode.SByte:
                        result = SqlDbType.SmallInt;
                        break;
                    case TypeCode.Byte:
                        result = SqlDbType.TinyInt;
                        break;
                    case TypeCode.Int16:
                        result = SqlDbType.SmallInt;
                        break;
                    case TypeCode.UInt16:
                        result = SqlDbType.Int;
                        break;
                    case TypeCode.Int32:
                        result = SqlDbType.Int;
                        break;
                    case TypeCode.UInt32:
                        result = SqlDbType.BigInt;
                        break;
                    case TypeCode.Int64:
                        result = SqlDbType.BigInt;
                        break;
                    case TypeCode.UInt64:
                        result = SqlDbType.Decimal;
                        break;
                    case TypeCode.Single:
                        result = SqlDbType.Real;
                        break;
                    case TypeCode.Double:
                        result = SqlDbType.Float;
                        break;
                    case TypeCode.Decimal:
                        result = SqlDbType.Money;
                        break;
                    case TypeCode.DateTime:
                        result = SqlDbType.DateTime;
                        break;
                    case TypeCode.String:
                        result = SqlDbType.VarChar;
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }




        public void Destroy()
        {
            using (SqlConnection conn = new SqlConnection(this.cType.ConnectionString))
            {
                string sql = $"delete * from {this.GetTableName()} where {this.GetPkName()} = {this.cType.Id}";

                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }


            }
        }

        public void Get()
        {
            using (SqlConnection conn = new SqlConnection(this.cType.ConnectionString))
            {
                string sql = $"select * from {this.GetTableName()} where {this.GetPkName()} = {this.cType.Id}";

                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {


                            this.cType.Id = Convert.ToInt32(dr[this.GetPkName()]);
                            foreach (var p in this.cType.GetType().GetProperties())
                            {
                                TableAttribute[] propertiesAttributes = (TableAttribute[])p.GetCustomAttributes(typeof(TableAttribute), false);
                                if (propertiesAttributes != null && propertiesAttributes.Length > 0)
                                {
                                    if (!propertiesAttributes[0].IsNotOnDatabase && string.IsNullOrEmpty(propertiesAttributes[0].PrimaryKey))
                                    {
                                        p.SetValue(this.cType, dr[p.Name]);
                                    }

                                }
                                else
                                {
                                    p.SetValue(this.cType, dr[p.Name]);

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }


            }
        }

        private object GetPkName()
        {
          /*  TableAttribute[] propertiesAttributes = (TableAttribute[])this.cType.GetType().GetProperty("Id").GetCustomAttributes(typeof(TableAttribute), false);
            if (propertiesAttributes != null && propertiesAttributes.Length > 0 && !string.IsNullOrEmpty(propertiesAttributes[0].PrimaryKey)) ;
            {
                return propertiesAttributes[0].PrimaryKey;
                //return this.cType.Id.GetType().GetCustomAttribute<TableAttribute>().PrimaryKey;
            }
            else
            {
                return "id";
            }*/

            return this.cType.GetType().GetProperty("Id").GetCustomAttribute<TableAttribute>().PrimaryKey;

        }

        public static T All<T>()
        {
            throw new NotImplementedException();
        }
    }
}
