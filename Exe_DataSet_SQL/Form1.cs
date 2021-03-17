using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exe_DataSet_SQL
{
    public partial class update_DB_btn : Form
    {
        public update_DB_btn()
        {
            InitializeComponent();
        }

        string strCon = @"Data Source=DESKTOP-FTT4EST\SQLEXPRESS;Initial Catalog=SoccerDB;Integrated Security=True";
        SqlConnection con;
        SqlDataAdapter adptr;
        DataSet dsTeams;
        DataTable dtTeams;
        static int count = 1;
        private void Form1_Load(object sender, EventArgs e)
        {
            dsTeams = new DataSet();

            con = new SqlConnection(strCon);
            adptr = new SqlDataAdapter(
                " SELECT * " +
                " FROM Teams ", con);
            ShowDataFromDB();
        }
        private void select_btn_Click(object sender, EventArgs e)
        {
            ShowDataFromDB();
        }

        private void ShowDataFromDB()
        {
            dsTeams.Clear();
            adptr.Fill(dsTeams, "TeamsTemp");
            dtTeams = dsTeams.Tables["TeamsTemp"];

            dataGridView1.DataSource = dtTeams;
        }

        private void btnInsertTeam_Click(object sender, EventArgs e)
        {
            int win = int.Parse(InputWin.Text);
            int draw = int.Parse(InputDraw.Text);
            DataRow dr = dtTeams.NewRow();
            dr["id_team"] = count++;
            dr["club_name"] = InputClub.Text;
            dr["coach_name"] = InputCoach.Text;
            dr["wins"] = InputWin.Text;
            dr["draws"] = InputDraw.Text;
            dr["loses"] = InputLose.Text;
            dr["points"] = win * 3 + draw;
            dtTeams.Rows.Add(dr);
            MessageBox.Show("Row Added", "Add", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void del_team_btn_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in dtTeams.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["id_team"].ToString() == InputDel.Text)
                {
                    row.Delete();
                    MessageBox.Show("Row Deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


        }

        private void update_team_btn_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in dtTeams.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["club_name"].ToString() == InputClub.Text)
                {
                    if (InputClub.Text == "" || InputCoach.Text == "" || InputWin.Text == "" || InputDraw.Text == "" || InputLose.Text == "")
                    {
                        MessageBox.Show("Please Fill all the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    row["id_team"] = count;
                    row["club_name"] = InputClub.Text;
                    row["coach_name"] = InputCoach.Text;
                    row["wins"] = InputWin.Text;
                    row["draws"] = InputDraw.Text;
                    row["loses"] = InputLose.Text;
                    row["points"] = (int.Parse(InputWin.Text) * 3) + int.Parse(InputDraw.Text);
                    MessageBox.Show("Team Updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void update_db_Click(object sender, EventArgs e)
        {
            new SqlCommandBuilder(adptr);
            adptr.Update(dtTeams);
            MessageBox.Show("DataBase Updated", "Update DB", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            SqlCommand comm = new SqlCommand("SearchTeamsTable", con);
            comm.CommandType = CommandType.StoredProcedure;

            SqlParameter parID = new SqlParameter("Points",InputId.Text);
            parID.Direction = ParameterDirection.Input;
            comm.Parameters.Add(parID);

            SqlDataAdapter aptr2 = new SqlDataAdapter(comm);
            DataSet ds2 = new DataSet();
            aptr2.Fill(ds2, "T1");

            dataGridView1.DataSource = ds2.Tables["T1"];
        }

    }
}

