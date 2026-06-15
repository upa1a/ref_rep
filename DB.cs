using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace @ref
{
    public class DB
    {
        public readonly string connectionString = "server=localhost;uid=root;pwd=root;database=milk_factory";
        public DataTable Select(string query, Dictionary<string, object> parameters = null)
        {
            DataTable table = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }
                    }
                }
            }
            return table;
        }
        public int Execute(string query, Dictionary<string, object> parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        //SELECT DISTINCT clients.name AS 'Поставщики', COUNT(distinct products.productsname) 'Общее количество', 
        //SUM(products.price* orders_has_products.count) AS 'Итого' FROM milk_factory.orders
        //JOIN orders_has_products ON orders_has_products.orders_idorders = orders.idorders
        //JOIN products ON products.idproducts = orders_has_products.products_idproducts
        //JOIN clients ON clients.id = milk_factory.orders.clients_id
        //GROUP BY clients.name;

//SELECT distinct clients.name AS 'Покупатели', SUM(specifications.materials_quantity* materials.price* orders_has_products.count) AS 'Итого' FROM orders
//JOIN clients ON clients.id = orders.clients_id
//JOIN orders_has_products ON orders_has_products.orders_idorders = orders.idorders
//JOIN specifications ON specifications.products_idproducts = orders_has_products.products_idproducts
//JOIN materials ON materials.idmaterials = specifications.materials_idmaterials
//GROUP BY clients.name
    }
}
