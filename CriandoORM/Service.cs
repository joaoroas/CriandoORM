using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

                string table = $"{this.cType.GetType().Name.ToLower()}s";

                TableAttribute[] tableAttributes = (TableAttribute[])this.cType.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                if (tableAttributes != null && tableAttributes.Length > 0)
                {
                    table = tableAttributes[0].Name;
                }


                string sql = string.Empty;
                if(this.cType.Id == 0)
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

                    foreach(var col in cols)
                    {
                        colsUpdate.Add($"{col} = @{col}");
                    }
                    sql += string.Join(",", colsUpdate);
                    string pkName = this.cType.Id.GetType().GetCustomAttributes<TableAttribute>().P
                    sql += "where"

                }




                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                for (var i = 0; i < cols.Count; i++)
                {
                    var value = values[i];
                    var col = cols[i];

                    if (value.GetType() == typeof(int))
                    {
                        cmd.Parameters.Add($"@{col}", SqlDbType.Int);

                    }
                    else if (value.GetType() == typeof(string))
                    {
                        cmd.Parameters.Add($"@{col}", SqlDbType.VarChar);

                    }
                    else if (value.GetType() == typeof(double))
                    {
                        cmd.Parameters.Add($"@{col}", SqlDbType.Money);

                    }
                    else if (value.GetType() == typeof(DateTime))
                    {
                        cmd.Parameters.Add($"@{col}", SqlDbType.DateTime);

                    }

                    cmd.Parameters[$"@{col}"].Value = values[i];
                }

                private  SqlDbType GetDbType(object value)
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
                    catch (Exception)
                    {

                        throw;
                    }
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



        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public static T All<T>()
        {
            throw new NotImplementedException();
        }
    }
}
