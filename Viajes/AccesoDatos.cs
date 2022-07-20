using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TUP_PI_Recu_Viajes
{
    internal class AccesoDatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;

        string cadenaConexion;

        public AccesoDatos()
        {
            cadenaConexion = @"Data Source=Tomas;Initial Catalog=AgenciaViaje;Integrated Security=True";
            conexion = new SqlConnection(cadenaConexion);
            comando = new SqlCommand();
        }
        public void conectar()
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;

        }
        public void desconectar()
        {
            conexion.Close();
        }

        public DataTable ConsultaBD(string consultaSQL)
        {

            DataTable table = new DataTable();
            conectar();
            comando.CommandText = consultaSQL;

            table.Load(comando.ExecuteReader());


            desconectar();

            return table;

        }
        public int ActualizarBD(string consultaSQL, List<Parametro>lparametro)
        {
            int filas;
            conectar();
            comando.CommandText = consultaSQL;
            foreach (Parametro P in lparametro)
            {
                comando.Parameters.AddWithValue(P.Nombre, P.Valor);

            }
            filas = comando.ExecuteNonQuery();
            desconectar();
            return filas;
        }

    }
}
