using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Teste_Wakke
{
    public partial class frm_inicio : Form
    {
        string path = "BancoWakke.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\BancoWakke.db";
        int id = 0;

        public frm_inicio()
        {
            InitializeComponent();
            create_db();
        }

        public frm_inicio(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void frm_inicio_Load(object sender, EventArgs e)
        {
            listar_tudo();
        }

        private void btn_cadastro_Click(object sender, EventArgs e)
        {
            frm_cadastro cadastro = new frm_cadastro(0);

            cadastro.ShowDialog();
            listar_tudo();
        }

        private void btn_editar_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(Dt_formulario.Rows[Dt_formulario.CurrentCell.RowIndex].Cells[0].Value);
            frm_cadastro cadastro = new frm_cadastro(id);

            cadastro.ShowDialog();
            listar_tudo();
        }

        private void Btn_excluir_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(Dt_formulario.Rows[Dt_formulario.CurrentCell.RowIndex].Cells[0].Value);
            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                try
                { 
                    con.Open();
                    var sql = "DELETE FROM usuario WHERE ID=" + id;
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    listar_tudo();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Erro", MessageBoxButtons.OK);
                    return;
                }
                finally
                {
                    con.Close();
                }
            }
            listar_tudo();
        }

        private void listar_tudo()
        {

            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=" + path))
            {
                try
                {
                    con.Open();
                    var sql = "SELECT id, nome, ativo, sobrenome, altura, datanascimento  FROM usuario";
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            Dt_formulario.DataSource = dt;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Erro", MessageBoxButtons.OK);
                    return;
                }
                finally 
                { 
                    con?.Close();
                }
            }
        }

        private void create_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "CREATE TABLE IF NOT EXISTS usuario (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ativo BOOLEAN NOT NULL CHECK (true||false), nome STRING NOT NULL, sobrenome STRING NOT NULL, datanascimento DATE NOT NULL, altura DECIMAL NOT NULL)";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                    sqlite.Close();
                }

            }
            else
            {
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "CREATE TABLE IF NOT EXISTS usuario (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ativo BOOLEAN NOT NULL CHECK (true||false), nome STRING NOT NULL, sobrenome STRING NOT NULL, datanascimento DATE NOT NULL, altura DECIMAL NOT NULL)";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                    sqlite.Close();
                }
                Console.WriteLine("Database cannot create");
                return;
            }
        }

    }
}
