using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace sci_res_inst
{
    enum Pages
    {
        Main = 0,
        Autorisation,
        Registration,
        Cab,
        Publication,
        Project,
        Person,
        Reserching,
        Department,
        Laboratory,
        Equipment
    }

    enum User_role
    {
        admin = 1,
        guest,
        student,
        employer,
        manager
    }

    public partial class Main_page : Form
    {

        List<Panel> list_panel = new List<Panel>();
        Pages current_page = Pages.Main;

        int page_number = -1;
        int prev_page = -1;

        bool autorisation_flag = false;
        int user_id = -1;
        int user_role = (int)User_role.guest;

        int count_page_pb = 0;
        int current_page_pb = 1;
        int public_first_id = 0;
        int public_last_id = 0;

        int pb_next_id = 0;
        int pb_prev_id = 0;

        int current_first_id_page = 0;

        bool half_page_f = false;

        int cnt_unic_rec_BD = 0;
        int cnt_unic_rec_app = 0;

        int public_id_tmp = 0;
        int tmp = 0;

        private static string connectionString = "Server=localhost;Port=5432;Database=sci_res_inst;User Id=postgres;Password=admin";
        NpgsqlConnection conn = null;


        public Main_page()
        {
            InitializeComponent();

            Conntion_to_DB();
            DisConntion_to_DB();
        }



        //private void button5_Click(object sender, EventArgs e)  //test bt down
        //{
        //    if(page_number > 0)
        //    {
        //        list_panel[--page_number].BringToFront();
        //    }
        //}

        //private void button6_Click(object sender, EventArgs e)  //test bt up
        //{
        //    if (page_number < list_panel.Count - 1)
        //    {
        //        list_panel[++page_number].BringToFront();
        //    }
        //}

        private void Main_page_Load(object sender, EventArgs e)
        {

            list_panel.Add(pn_main_body);
            list_panel.Add(pn_autorisation);
            list_panel.Add(pn_registration);
            list_panel.Add(pn_cab);
            list_panel.Add(pn_my_public);
            list_panel.Add(pn_proj);
            list_panel.Add(pn_personal);
            list_panel.Add(pn_researching);
            list_panel.Add(pn_department);
            list_panel.Add(pn_labs);
            list_panel.Add(pn_equipment);

            //current_page = Pages.Main;
            //page_number = (int)current_page;
            //list_panel[(int)current_page].BringToFront();

            bt_public_Click(sender,e);

            get_publications();
        }

        private void lb_input_Click(object sender, EventArgs e)
        {
            current_page = Pages.Autorisation;

            if (autorisation_flag == true)
            {
                autorisation_flag = false;
                user_id = -1;

                lb_input.Text = "Войти";

                lb_registration.Cursor = Cursors.Hand;
                lb_registration.ForeColor = Color.Blue;

                lb_ac_nm_hd.Text = "Гость";

                bt_my_public.Visible = false;
                pb_my_public.Visible = false;

                bt_proj.Visible = false;
                pb_proj.Visible = false;

                bt_research.Visible = false;
                pb_research.Visible = false;

                bt_department.Visible = false;
                pb_dep.Visible = false;

                bt_lab.Visible = false;
                pb_lab.Visible = false;

                bt_eq.Visible = false;
                pb_eq.Visible = false;

                bt_pers.Visible = false;
                pb_pers.Visible = false;

                current_page = Pages.Main;
            }
               
            if ((int)current_page != page_number)
            {
                page_number = (int)current_page;
                list_panel[(int)current_page].BringToFront();
            }
        }

        private void lb_registration_Click(object sender, EventArgs e)
        {
            if(autorisation_flag == false)
            {
                current_page = Pages.Registration;

                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();
                }
               
                lb_err_empty_dt_r.Visible = false;
                lb_scc_reg_r.Visible = false;
                lb_err_pss_r.Visible = false;

                //to do вынести в отдельную функцию сделать ее универсальной
                tb_srnm_r.Text = "";
                tb_nm_r.Text = "";
                tb_md_nm_r.Text = "";
                tb_lg_r.Text = "";
                tb_pss_r.Text = "";
                tb_pss_ch_r.Text = "";
                tb_eml_r.Text = "";
            }
            else
            {
                lb_registration.Cursor = Cursors.Arrow;
                lb_registration.ForeColor = Color.Gray;
            }
        }

        private void bt_public_Click(object sender, EventArgs e)
        {
            current_page = Pages.Main;

            if ((int)current_page != page_number)
            {

                reset_color_bt(prev_page);

                //bt_public.BackColor = Color.FromArgb(255, 128, 0);
                //pb_public.BackColor = Color.FromArgb(255, 128, 0);

                //bt_public.Enabled = false;
                //pb_public.Enabled = false;
                //функция перехода на другую страницу, где бы бло бы перекрашивание кнопки

                page_number = (int)current_page;
                prev_page = page_number;


                list_panel[(int)current_page].BringToFront();
            }
        }

        private void bt_cab_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Cab;
                if ((int)current_page != page_number)
                {
                    //bt_cab.BackColor = Color.FromArgb(255, 192, 128);
                    //reset_color_bt(prev_page);

                    //bt_cab.BackColor = Color.FromArgb(255, 128, 0);
                    //pb_cab.BackColor = Color.FromArgb(255, 128, 0);

                    //bt_cab.Enabled = false;
                    //pb_cab.Enabled = false;

                    page_number = (int)current_page;
                    prev_page = page_number;

                    list_panel[(int)current_page].BringToFront();
                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }


// --------------------------------------------------------------------------------------------------------------------------------------------------
        private void bt_proj_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Project;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();

                    if (user_role == (int)User_role.admin || user_role == (int)User_role.manager)
                    {

                        bt_pj_create.Visible = true;
                        bt_pj_create.Enabled = true;

                        bt_pj_ed.Visible = true;
                        bt_pj_ed.Enabled = true;

                        if(dgv_pj_all.Width != 730)
                            dgv_pj_all.Width = 730;

                        //bt_pj_sv.Visible = true;
                        //bt_pj_sv.Enabled = true;

                        //bt_pj_cl.Visible = true;
                        //bt_pj_cl.Enabled = true;

                    }
                    else
                    {
                        bt_pj_create.Visible = false;
                        bt_pj_create.Enabled = false;

                        bt_pj_ed.Visible = false;
                        bt_pj_ed.Enabled = false;

                        dgv_pj_all.Width = 930;

                        //930


                    }

                    get_all_project();



                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }


        private void bt_pj_create_Click(object sender, EventArgs e)
        {
            bt_pj_ed.Enabled = false;

            bt_pj_sv.Enabled = true;
            bt_pj_sv.Visible = true;

            bt_pj_cl.Enabled = true;
            bt_pj_cl.Visible = true;

            prj_name_tb.Enabled = true;
            prj_desc_tb.Enabled = true;
            prj_st_cb.Enabled = true;


        }

        private void bt_pj_ed_Click(object sender, EventArgs e)
        {
            bt_pj_create.Enabled = false;

            bt_pj_sv.Enabled = true;
            bt_pj_sv.Visible = true;

            bt_pj_cl.Enabled = true;
            bt_pj_cl.Visible = true;

            bt_pj_dl.Enabled = true;
            bt_pj_dl.Visible = true;




        }

        private void bt_pj_sv_Click(object sender, EventArgs e)
        {
            if (bt_pj_create.Enabled == false)
            {

            }
            else
            {

            }

            bt_pj_cl_Click(sender,e);
        }

        private void bt_pj_cl_Click(object sender, EventArgs e)
        {
            bt_pj_create.Enabled = true;
            bt_pj_ed.Enabled = true;

            bt_pj_sv.Enabled = false;
            bt_pj_sv.Visible = false;

            bt_pj_cl.Enabled = false;
            bt_pj_cl.Visible = false;

            bt_pj_dl.Enabled = false;
            bt_pj_dl.Visible = false;

            prj_name_tb.Text = "";
            prj_desc_tb.Text = "";

            prj_name_tb.Enabled = false;
            prj_desc_tb.Enabled = false;
            prj_st_cb.Enabled = false;

            get_all_project();
        }

        private void bt_pj_dl_Click(object sender, EventArgs e)
        {

        }



 // --------------------------------------------------------------------------------------------------------------------------------------------------


        private void bt_my_public_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Publication;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();
                }


                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM get_publications_by_user(@p_usr_id)";

                    cmd.Parameters.AddWithValue("p_usr_id", user_id);

                    DataTable dt = new DataTable();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(dt);


                    dataGridView1.DataSource = dt;


                    DisConntion_to_DB();
                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        private void bt_pers_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Person;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();

                    if(user_role == (int)User_role.admin || user_role == (int)User_role.manager)
                    {
                        bt_ed_prs.Visible = true;

                        lb_nm_prs.Visible =true;
                        lb_level_rl_prs.Visible =true;
                        cb_level_rl_prs.Visible = true;


                        cb_level_rl_prs.SelectedIndex = 0;
                    }
                    else
                    {
                        bt_ed_prs.Visible = false;
                        bt_save_ed_prs.Visible = false;
                        bt_cancel_ed_prs.Visible = false;

                        lb_nm_prs.Visible = false;
                        lb_level_rl_prs.Visible = false;
                        cb_level_rl_prs.Visible = false;
                    }

                    get_all_users();

                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        private void bt_research_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Reserching;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();


                    get_all_research();
                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        private void bt_department_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Department;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();
                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        private void bt_lab_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Laboratory;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();


                    get_all_lab();



                }
            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        private void bt_eq_Click(object sender, EventArgs e)
        {
            if (autorisation_flag)
            {
                current_page = Pages.Equipment;
                if ((int)current_page != page_number)
                {
                    page_number = (int)current_page;
                    list_panel[(int)current_page].BringToFront();
                }


                get_all_eq() ;


            }
            else
            {
                lb_input_Click(sender, e);
            }
        }

        public void reset_color_bt(int inx_pg)
        {
            switch(inx_pg)
            {
                case 0:
                    bt_public.BackColor = Color.Gainsboro;
                    pb_public.BackColor = Color.Gainsboro;

                    bt_public.Enabled = true;
                    pb_public.Enabled = true;

                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    bt_cab.BackColor = Color.Gainsboro;
                    pb_cab.BackColor = Color.Gainsboro;

                    bt_cab.Enabled = true;
                    pb_cab.Enabled = true;
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 6:
                    break;
            }
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------

        public void get_all_eq()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_eq()";

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_all_eq.DataSource = dt;

                DisConntion_to_DB();
            }
        }

        public void get_name_lab()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_name_lab()";

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_lab_eq.DataSource = dt;

                DisConntion_to_DB();
            }
        }

        public void update_eq(int id)
        {
            if (eq_name_tb.Text != "" && eq_desc_tb.Text != "" && eq_lab_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL update_eq(@p_eq_id, @p_name, @p_text, @p_lab_name, NULL)";

                    cmd.Parameters.AddWithValue("p_eq_id", id);
                    cmd.Parameters.AddWithValue("p_name", eq_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_text", eq_desc_tb.Text);
                    cmd.Parameters.AddWithValue("p_lab_name", eq_lab_tb.Text);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }
        }

        public void add_eq()
        {
            if (eq_name_tb.Text != "" && eq_desc_tb.Text != "" && eq_lab_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL add_eq(@p_name, @p_text, @p_lab_name)";

                    cmd.Parameters.AddWithValue("p_name",   eq_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_text",   eq_desc_tb.Text);
                    cmd.Parameters.AddWithValue("p_lab_name", eq_lab_tb.Text);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }
        }

        public void delete_eq(int id)
        {
            if (eq_name_tb.Text != "" && eq_desc_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL delete_eq(@p_eq_id, NULL)";

                    cmd.Parameters.AddWithValue("p_eq_id", id);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(rtrn.Value) == 1)
                    {
                        MessageBox.Show("Запись удалена", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка", "Не удача");
                    }

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись, которую желаете удалить", "Ошибка, запись не выбрана");
            }
        }
        private void dgv_all_eq_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_all_eq.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_all_eq.SelectedRows[0];

                eq_name_tb.Text = selectedRow.Cells["eq_name"].Value.ToString();
                eq_desc_tb.Text = selectedRow.Cells["eq_text"].Value.ToString();
                eq_lab_tb.Text = selectedRow.Cells["lb_name"].Value.ToString();

                tmp = Convert.ToInt32(selectedRow.Cells["eq_id"].Value);

            }
        }

        private void dgv_lab_eq_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_lab_eq.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_lab_eq.SelectedRows[0];

                eq_lab_tb.Text = selectedRow.Cells["lb_name"].Value.ToString();
            }
        }

        private void bt_cr_eq_Click(object sender, EventArgs e)
        {
            bt_ed_eq.Enabled = false;

            bt_sv_eq.Enabled = true;
            bt_sv_eq.Visible = true;

            bt_cl_eq.Enabled = true;
            bt_cl_eq.Visible = true;

            eq_name_tb.Enabled = true;
            eq_desc_tb.Enabled = true;

            get_name_lab();

        }

        private void bt_ed_eq_Click(object sender, EventArgs e)
        {
            bt_cr_eq.Enabled = false;

            bt_sv_eq.Enabled = true;
            bt_sv_eq.Visible = true;

            bt_cl_eq.Enabled = true;
            bt_cl_eq.Visible = true;

            bt_del_eq.Enabled = true;
            bt_del_eq.Visible = true;

            eq_name_tb.Enabled = true;
            eq_desc_tb.Enabled = true;

            get_name_lab();

        }

        private void bt_sv_eq_Click(object sender, EventArgs e)
        {

            if (bt_cr_eq.Enabled == false)
            {
                update_eq(tmp);
            }
            else
            {
                add_eq();
            }

            bt_cl_eq_Click(sender, e);
        }

        private void bt_cl_eq_Click(object sender, EventArgs e)
        {
            bt_ed_eq.Enabled = true;
            bt_cr_eq.Enabled = true;

            bt_sv_eq.Enabled = false;
            bt_sv_eq.Visible = false;

            bt_cl_eq.Enabled = false;
            bt_cl_eq.Visible = false;

            bt_del_eq.Enabled = false;
            bt_del_eq.Visible = false;


            eq_name_tb.Text = "";
            eq_desc_tb.Text = "";
            eq_lab_tb.Text = "";

            eq_name_tb.Enabled = false;
            eq_desc_tb.Enabled = false;

            dgv_lab_eq.DataSource = null;

            get_all_eq();
        }

        private void bt_del_eq_Click(object sender, EventArgs e)
        {

            delete_eq(tmp);
            bt_cl_eq_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        public void get_all_users()
        {
            if (Conntion_to_DB())
            {

                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_users()";

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_persons.DataSource = dt;

                DisConntion_to_DB();
            }
        }


        public void get_all_project()
        {

            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_proj()";

                //cmd.Parameters.AddWithValue("p_usr_id", user_id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_pj_all.DataSource = dt;


                DisConntion_to_DB();
            }


        }


        public void get_all_research()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_research()";

                //cmd.Parameters.AddWithValue("p_usr_id", user_id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_all_rst.DataSource = dt;


                DisConntion_to_DB();
            }
        }

        public void get_all_lab()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_lab()";

                //cmd.Parameters.AddWithValue("p_usr_id", user_id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_all_lab.DataSource = dt;


                DisConntion_to_DB();
            }
        }



        private void bt_burger_Click(object sender, EventArgs e)
        {
            if (pn_sidebar.Width == 234)
                pn_sidebar.Width = 49;
            else
                pn_sidebar.Width = 234;
            
        }

        private void Main_page_Resize(object sender, EventArgs e)
        {

            //MessageBox.Show("Форма восстановлена до нормального размера.");
            //if (this.WindowState == FormWindowState.Maximized)
            //{
            //    //CenterControls(list_panel[page_number]);
            //    //list_panel[page_number].Dock = DockStyle.Fill;
            //}
            //else if (this.WindowState == FormWindowState.Normal)
            //{
            //    //list_panel[page_number].Dock = DockStyle.None;
            //}

        }

        private void bt_send_a_Click(object sender, EventArgs e)
        {
            string login = tb_lg_a.Text;
            string pass = tb_pass_a.Text;

            bool lg_ad_ps_notEmprty = true;

            lb_lgEmpty_a.Visible = false;
            lb_psEmpty_a.Visible = false;
            lb_err_a.Visible = false;

            if (String.IsNullOrEmpty(login) == true)
            {
                lb_lgEmpty_a.Visible = true;
                lg_ad_ps_notEmprty = false;
            }

            if (String.IsNullOrEmpty(pass) == true)
            {
                lb_psEmpty_a.Visible = true;
                lg_ad_ps_notEmprty = false;
            }

            if (lg_ad_ps_notEmprty)
            {
                if (Conntion_to_DB())
                {

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL compare_accounts(@p_login, @p_pass, NULL ,NULL)";

                    // Добавляем параметры
                    cmd.Parameters.AddWithValue("p_login", login);
                    cmd.Parameters.AddWithValue("p_pass", pass);

                    // Определяем параметр OUT
                    var accIdParam = new NpgsqlParameter("p_acc_id", NpgsqlTypes.NpgsqlDbType.Integer)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(accIdParam);

                    var usr_role = new NpgsqlParameter("p_acc_id", NpgsqlTypes.NpgsqlDbType.Integer)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(usr_role);

                    // Выполняем команду
                    cmd.ExecuteNonQuery();

                    user_role = (int)usr_role.Value;
                    user_id = (int)accIdParam.Value;

                    DisConntion_to_DB();
                }

                if (user_id != -1)
                {
                    autorisation_flag = true;
                    tb_pass_a.Text = "";
                    lb_input.Text = "Выйти";

                    get_full_data_about_user();

                    if (user_role == (int)User_role.guest)
                    {
                        bt_my_public.Visible = false;
                        pb_my_public.Visible = false;

                        bt_proj.Visible = false;
                        pb_proj.Visible = false;  

                        bt_research.Visible = false;
                        pb_research.Visible = false;

                        bt_department.Visible = false;
                        pb_dep.Visible = false;

                        bt_lab.Visible = false;
                        pb_lab.Visible = false;

                        bt_eq.Visible = false;
                        pb_eq.Visible = false;

                        bt_pers.Visible = false;
                        pb_pers.Visible = false;
                    }

                    if (user_role == (int)User_role.student)
                    {
                        bt_my_public.Visible = false;
                        pb_my_public.Visible = false;

                        bt_proj.Visible = false;
                        pb_proj.Visible = false;

                        bt_research.Visible = false;
                        pb_research.Visible = false;

                        bt_department.Visible = false;
                        pb_dep.Visible = false;

                        bt_lab.Visible = false;
                        pb_lab.Visible = false;

                        bt_eq.Visible = false;
                        pb_eq.Visible = false;

                        bt_pers.Visible = true;
                        pb_pers.Visible = true;
                    }

                    if (user_role == (int)User_role.employer)
                    {
                        bt_my_public.Visible = true;
                        pb_my_public.Visible = true;

                        bt_proj.Visible = true;
                        pb_proj.Visible = true;

                        bt_research.Visible = true;
                        pb_research.Visible = true;

                        bt_department.Visible = true;
                        pb_dep.Visible = true;

                        bt_lab.Visible = true;
                        pb_lab.Visible = true;

                        bt_eq.Visible = false;
                        pb_eq.Visible = false;

                        bt_pers.Visible = true;
                        pb_pers.Visible = true;
                    }

                    if (user_role == (int)User_role.manager)
                    {
                        bt_my_public.Visible = true;
                        pb_my_public.Visible = true;

                        bt_proj.Visible = true;
                        pb_proj.Visible = true;

                        bt_research.Visible = true;
                        pb_research.Visible = true;

                        bt_department.Visible = true;
                        pb_dep.Visible = true;

                        bt_lab.Visible = true;
                        pb_lab.Visible = true;

                        bt_eq.Visible = true;
                        pb_eq.Visible = true;

                        bt_pers.Visible = true;
                        pb_pers.Visible = true;
                    }

                    if (user_role == (int)User_role.admin)
                    {
                        bt_my_public.Visible = true;
                        pb_my_public.Visible = true;

                        bt_proj.Visible = true;
                        pb_proj.Visible = true;

                        bt_research.Visible = true;
                        pb_research.Visible = true;

                        bt_department.Visible = true;
                        pb_dep.Visible = true;

                        bt_lab.Visible = true;
                        pb_lab.Visible = true;

                        bt_eq.Visible = true;
                        pb_eq.Visible = true;

                        bt_pers.Visible = true;
                        pb_pers.Visible = true;
                    }

                    bt_public_Click(sender, e);
                }
                else
                {
                    lb_err_a.Visible = true;
                    autorisation_flag = false;
                }
            }
        }
        public bool Conntion_to_DB()
        {
            bool succes = true;


            if (conn == null)
                conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=sci_res_inst;User Id=postgres;Password=admin");

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключится к базе данных!\n" + ex.Message);
                succes =  false;
            }

            return succes; 
        }

        public void DisConntion_to_DB()
        {

            if (conn != null)
            {
                conn.Close();
                //conn.Dispose(); // Освобождаем ресурсы
            }

        }

        public void get_full_data_about_user()
        {
            if (Conntion_to_DB())
            {
                if (autorisation_flag == true)
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL get_full_data_about_user(@p_usr_id, NULL, NULL, NULL, NULL, NULL, NULL)";

                    // Добавляем параметры
                    cmd.Parameters.AddWithValue("p_usr_id", user_id);

                    // Определяем параметр OUT

                    var srnm_parm = new NpgsqlParameter("p_surname", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(srnm_parm);

                    var nm_parm = new NpgsqlParameter("p_name", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(nm_parm);

                    var md_nm_parm = new NpgsqlParameter("p_md_name", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(md_nm_parm);

                    var em_p = new NpgsqlParameter("p_email", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(em_p);

                    var role_p = new NpgsqlParameter("p_role", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(role_p);

                    var log_p = new NpgsqlParameter("p_log", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(log_p);

                    // Выполняем команду
                    cmd.ExecuteNonQuery();

                    if ((string)srnm_parm.Value != "non")
                    {
                        tb_sr_n_cb.Text = (string)srnm_parm.Value;
                        tb_n_cb.Text = (string)nm_parm.Value;
                        tb_md_n_cb.Text = (string)md_nm_parm.Value;
                        tb_em_cb.Text = (string)em_p.Value;
                        tb_rl_cb.Text = Convert.ToString(role_p.Value);
                        tb_lg_cb.Text = (string)log_p.Value;

                        lb_ac_nm_hd.Text = (string)nm_parm.Value + " " + (string)md_nm_parm.Value;
                    }
                }
                else
                {
                    lb_ac_nm_hd.Text = "Гость";
                }

                DisConntion_to_DB();
            }
        }

        public void clear_pub_one()
        {
            tb_pub_one.Text = "";
            lb_title_pub_one.Text = "";
            lb_auth_pub_one.Text = "Автор:";
        }
        public void clear_pub_two()
        {
            tb_pub_two.Text = "";
            lb_title_pub_two.Text = "";
            lb_auth_pub_two.Text = "Автор:";
        }

        public void get_publications()
        {
            if (Conntion_to_DB())
            {
                clear_pub_one();
                clear_pub_two();

                cnt_unic_rec_BD = count_unique_records("tb_publication", "pb_id");
                count_page_pb = DivideUp(cnt_unic_rec_BD);

                lb_all_rec_m.Text = Convert.ToString(count_page_pb);
                lb_curr_pg_p.Text = Convert.ToString(current_page_pb);

                pb_prev_id = get_last_id("tb_publication", "pb_id");
                pb_next_id = get_next_id_pb(pb_prev_id);

                int tmp = -1;
                if(pb_prev_id != -1)
                    tmp = get_date_pub(pb_prev_id, 1);

                if (pb_next_id != -1)
                    tmp = get_date_pub(pb_next_id, 2);


                DisConntion_to_DB();
            }
        }

        public int get_next_id_pb(int id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL get_next_id_pb(@p_in_id, NULL)";

            cmd.Parameters.AddWithValue("p_in_id", id);

            var p_out_id = new NpgsqlParameter("p_out_id", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_out_id);

            cmd.ExecuteNonQuery();


            if (p_out_id.Value == DBNull.Value)
                return -1;

            return Convert.ToInt32(p_out_id.Value);
        }

        public int get_prev_id_pb(int id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL get_prev_id_pb(@p_in_id, NULL)";

            cmd.Parameters.AddWithValue("p_in_id", id);

            var p_out_id = new NpgsqlParameter("p_out_id", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_out_id);

            cmd.ExecuteNonQuery();

            if (p_out_id.Value == DBNull.Value)
                return -1;

            return Convert.ToInt32(p_out_id.Value);
        }

        public int count_unique_records(string tb_nm, string clm_nm)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL count_unique_records(@tb_name, @clmnn_name, NULL)";

            cmd.Parameters.AddWithValue("tb_name", tb_nm);
            cmd.Parameters.AddWithValue("clmnn_name", clm_nm);

            var p_result = new NpgsqlParameter("unique_count", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_result);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(p_result.Value);
           // return Convert.ToString(p_result.Value);
        }

        public int get_first_id(string tb_nm, string clm_nm)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL get_first_id(@tb_name, @clmnn_name, NULL)";

            cmd.Parameters.AddWithValue("tb_name", tb_nm);
            cmd.Parameters.AddWithValue("clmnn_name", clm_nm);

            var p_result = new NpgsqlParameter("unique_id", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_result);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(p_result.Value);
        }

        public int get_last_id(string tb_nm, string clm_nm)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL get_last_id(@tb_name, @clmnn_name, NULL)";

            cmd.Parameters.AddWithValue("tb_name", tb_nm);
            cmd.Parameters.AddWithValue("clmnn_name", clm_nm);

            var p_result = new NpgsqlParameter("unique_id", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_result);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(p_result.Value);
        }

        public int get_date_pub(int id, int num_tb)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CALL get_date_pub(@p_id, NULL, NULL, NULL, NULL)";

            cmd.Parameters.AddWithValue("p_id", id);


            var p_text = new NpgsqlParameter("p_text", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_text);

            var p_capture = new NpgsqlParameter("p_capture", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_capture);

            var p_name = new NpgsqlParameter("p_name", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_name);

            var p_md_nm = new NpgsqlParameter("p_md_nm", NpgsqlTypes.NpgsqlDbType.Text)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(p_md_nm);

            cmd.ExecuteNonQuery();

            if(Convert.ToString(p_text.Value) != "non")
            {
                if (num_tb == 1)
                {
                    tb_pub_one.Text = Convert.ToString(p_text.Value);
                    lb_title_pub_one.Text = Convert.ToString(p_capture.Value);
                    lb_auth_pub_one.Text = "Автор: " + Convert.ToString(p_md_nm.Value) + " " + Convert.ToString(p_name.Value);
                }
                else if (num_tb == 2)
                {
                    tb_pub_two.Text = Convert.ToString(p_text.Value);
                    lb_title_pub_two.Text = Convert.ToString(p_capture.Value);
                    lb_auth_pub_two.Text = "Автор: " + Convert.ToString(p_md_nm.Value) + " " + Convert.ToString(p_name.Value);

                }
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private void bt_next_page_pb_Click(object sender, EventArgs e)
        {

            current_page_pb++;

            if(current_page_pb <= count_page_pb)
            {
                if (Conntion_to_DB())
                {
                    
                    int tmp_p = get_next_id_pb(pb_next_id);
                    int tmp_n = get_next_id_pb(tmp_p);
                   
                    if(tmp_n == -1)
                    {
                        get_date_pub(tmp_p, 1);
                        clear_pub_two();
                        half_page_f = true;
                    }
                    else
                    {
                        half_page_f = false;
                        pb_prev_id = tmp_p;
                        pb_next_id = tmp_n;

                        get_date_pub(pb_prev_id, 1);
                        get_date_pub(pb_next_id, 2);
                    }
                    
                    DisConntion_to_DB();
                }
            }
            else
            {
                current_page_pb = count_page_pb;
            }

            lb_curr_pg_p.Text = Convert.ToString(current_page_pb);
        }

        private void bt_prev_page_pb_Click(object sender, EventArgs e)
        {
            current_page_pb--;

            if (current_page_pb >= 1)
            {
                if (Conntion_to_DB())
                {
                    if(half_page_f == false)
                    {
                        pb_next_id = get_prev_id_pb(pb_prev_id);
                        pb_prev_id = get_prev_id_pb(pb_next_id);

                    }

                    if (pb_prev_id != -1)
                        get_date_pub(pb_prev_id, 1);

                    if (pb_next_id != -1)
                        get_date_pub(pb_next_id, 2);

                    half_page_f = false;
                    DisConntion_to_DB();
                }
            }
            else
            {
                current_page_pb = 1;
            }

            lb_curr_pg_p.Text = Convert.ToString(current_page_pb);

        }

        private void bt_back_to_begin_pb_Click(object sender, EventArgs e)
        {
            current_page_pb = 1;
            get_publications();
        }

        private void img_ms_Move(object sender, MouseEventArgs e)
        {

            Control control = sender as Control;

            if(control != null)
            {
                string controlName = control.Name;

                if(controlName == "bt_public" || controlName == "pb_public")
                {
                    pb_public.BackColor = Color.FromArgb(255, 192, 128);
                    bt_public.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_cab" || controlName == "pb_cab")
                {
                    pb_cab.BackColor = Color.FromArgb(255, 192, 128);
                    bt_cab.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_proj" || controlName == "pb_proj")
                {
                    pb_proj.BackColor = Color.FromArgb(255, 192, 128);
                    bt_proj.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_research" || controlName == "pb_research")
                {
                    pb_research.BackColor = Color.FromArgb(255, 192, 128);
                    bt_research.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_my_public" || controlName == "pb_my_public")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_my_public.BackColor = Color.FromArgb(255, 192, 128);
                    bt_my_public.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_department" || controlName == "pb_dep")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_dep.BackColor = Color.FromArgb(255, 192, 128);
                    bt_department.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_lab" || controlName == "pb_lab")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_lab.BackColor = Color.FromArgb(255, 192, 128);
                    bt_lab.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_eq" || controlName == "pb_eq")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_eq.BackColor = Color.FromArgb(255, 192, 128);
                    bt_eq.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (controlName == "bt_pers" || controlName == "pb_pers")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_pers.BackColor = Color.FromArgb(255, 192, 128);
                    bt_pers.BackColor = Color.FromArgb(255, 192, 128);
                }
            }
        }

        private void img_ms_Leave(object sender, EventArgs e)
        {

            Control control = sender as Control;

            if (control != null)
            {
                string controlName = control.Name;

                if (controlName == "bt_public" || controlName == "pb_public")
                {
                    //pb_public.BackColor = pb_public.Parent.BackColor;
                    //bt_public.BackColor = bt_public.Parent.BackColor;
                    pb_public.BackColor = Color.Gainsboro;
                    bt_public.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_cab" || controlName == "pb_cab")
                {
                    //  pb_cab.BackColor = pb_cab.Parent.BackColor;
                    //bt_cab.BackColor = bt_cab.Parent.BackColor;
                    pb_cab.BackColor = Color.Gainsboro;
                    bt_cab.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_proj" || controlName == "pb_proj")
                {
                    //pb_proj.BackColor = pb_proj.Parent.BackColor;
                    //bt_proj.BackColor = bt_proj.Parent.BackColor;
                    pb_proj.BackColor = Color.Gainsboro;
                    bt_proj.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_research" || controlName == "pb_research")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_research.BackColor = Color.Gainsboro;
                    bt_research.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_my_public" || controlName == "pb_my_public")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_my_public.BackColor = Color.Gainsboro;
                    bt_my_public.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_department" || controlName == "pb_dep")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_dep.BackColor = Color.Gainsboro;
                    bt_department.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_lab" || controlName == "pb_lab")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_lab.BackColor = Color.Gainsboro;
                    bt_lab.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_eq" || controlName == "pb_eq")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_eq.BackColor = Color.Gainsboro;
                    bt_eq.BackColor = Color.Gainsboro;
                }

                if (controlName == "bt_pers" || controlName == "pb_pers")
                {
                    //pb_research.BackColor = pb_research.Parent.BackColor;
                    //bt_research.BackColor = bt_research.Parent.BackColor;
                    pb_pers.BackColor = Color.Gainsboro;
                    bt_pers.BackColor = Color.Gainsboro;
                }
            }
        }

        private void Main_page_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (conn != null)
            {
                conn.Close();
                conn.Dispose(); // Освобождаем ресурсы
            }
        }

        private void bt_ed_data_usr_Click(object sender, EventArgs e)
        {
            tb_sr_n_cb.ReadOnly = false;
            tb_n_cb.ReadOnly = false;
            tb_md_n_cb.ReadOnly = false;
            tb_em_cb.ReadOnly = false;
            tb_rl_cb.ReadOnly = false;

            bt_ed_data_usr_cb.Enabled = false;
            bt_ed_data_usr_cb.BackColor = Color.DimGray;

            bt_sv_dt_cb.Visible = true;
            bt_cancel_cb.Visible = true;
            bt_delete_ac_cb.Visible = true;

            tb_lg_cb.Visible = true;
            tb_nw_pss_cb.Visible = true;
            tb_rp_nw_pss_cb.Visible = true;

            lb_lg_cb.Visible = true;
            lb_nw_pss_cb.Visible = true;
            lb_rp_nw_pss_cb.Visible = true;

        }

        private void bt_cancel_cb_Click(object sender, EventArgs e)
        {
            tb_sr_n_cb.ReadOnly = true;
            tb_n_cb.ReadOnly = true;
            tb_md_n_cb.ReadOnly = true;
            tb_em_cb.ReadOnly = true;
            tb_rl_cb.ReadOnly = true;

            bt_ed_data_usr_cb.Enabled = true;
            bt_ed_data_usr_cb.BackColor = Color.LightGray;
            bt_sv_dt_cb.Visible = false;
            bt_cancel_cb.Visible = false;
            bt_delete_ac_cb.Visible = false;

            tb_lg_cb.Visible = false;
            tb_nw_pss_cb.Visible = false;
            tb_rp_nw_pss_cb.Visible = false;

            tb_lg_cb.Text = "";
            tb_nw_pss_cb.Text = "";
            tb_rp_nw_pss_cb.Text = "";

            lb_lg_cb.Visible = false;
            lb_nw_pss_cb.Visible = false;
            lb_rp_nw_pss_cb.Visible = false;

            // тут делаем запрос о данных в бд чтобы восстановить их содержимое
            get_full_data_about_user();

        }

        private void bt_registr_send_r_Click(object sender, EventArgs e)
        {
            if(lb_err_empty_dt_r.Visible == true || lb_scc_reg_r.Visible == true || lb_err_pss_r.Visible == true)
            {
                lb_err_empty_dt_r.Visible = false;
                lb_scc_reg_r.Visible = false;
                lb_err_pss_r.Visible = false;
            }
                
            if (
                tb_srnm_r.Text  != "" && tb_nm_r.Text   != "" && tb_md_nm_r.Text    != "" &&
                tb_lg_r.Text    != "" && tb_pss_r.Text  != "" && tb_pss_ch_r.Text   != "" &&
                tb_eml_r.Text    != ""
                )
            {
                
                if(tb_pss_r.Text == tb_pss_ch_r.Text)
                {
                    if (Conntion_to_DB())
                    {
                        //to do необходима проверка на уникальность логина


                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "CALL add_new_user(@p_surname, @p_name, @p_md_name, @p_email, @p_role, @p_login, @p_pass)";

                        // Добавляем параметры
                        cmd.Parameters.AddWithValue("p_surname", tb_srnm_r.Text);
                        cmd.Parameters.AddWithValue("p_name",    tb_nm_r.Text);
                        cmd.Parameters.AddWithValue("p_md_name", tb_md_nm_r.Text);
                        cmd.Parameters.AddWithValue("p_email",   tb_eml_r.Text);
                        cmd.Parameters.AddWithValue("p_role", 2);
                        cmd.Parameters.AddWithValue("p_login",   tb_lg_r.Text);
                        cmd.Parameters.AddWithValue("p_pass",    tb_pss_r.Text);

                        // Выполняем команду
                        cmd.ExecuteNonQuery();

                        DisConntion_to_DB();
                        lb_scc_reg_r.Visible = true;
                    }
                }
                else
                {
                    lb_err_pss_r.Visible = true;
                }                
            }
            else
            {
                lb_err_empty_dt_r.Visible = true;
            }
        }

        private void bt_delete_ac_cb_Click(object sender, EventArgs e)
        {

            DialogResult del_usr = MessageBox.Show("Вы действительно хотите удалить аккаунт?", "Удаление аккаунта", MessageBoxButtons.YesNo);

            if(del_usr == DialogResult.Yes)
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL delete_user(@p_usr_id, NULL)";

                    cmd.Parameters.AddWithValue("p_usr_id", user_id);

                    var p_result = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_result);

                    cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(p_result.Value))
                    {
                        MessageBox.Show("Ваш аккаунт удален.", "Удаление аккаунта");

                        user_id = -1;
                        autorisation_flag = false;

                        tb_sr_n_cb.Text = "";
                        tb_n_cb.Text = "";
                        tb_md_n_cb.Text = "";
                        tb_em_cb.Text = "";
                        tb_rl_cb.Text = "";
                        tb_lg_cb.Text = "";

                        current_page = Pages.Main;

                        if ((int)current_page != page_number)
                        {
                            page_number = (int)current_page;
                            list_panel[(int)current_page].BringToFront();
                        }


                        lb_ac_nm_hd.Text = "Гость";
                    }
                    else
                    {
                        MessageBox.Show("Такого аккаунта не существует!", "Удаление аккаунта");
                    }

                    DisConntion_to_DB();
                }
            }
        }

        private void bt_sv_dt_cb_Click(object sender, EventArgs e)
        {

            lb_err_new_pss_cb.Visible = false;

            if (
                tb_sr_n_cb.Text != "" && tb_n_cb.Text != "" && tb_md_n_cb.Text != "" &&
                tb_em_cb.Text != "" && tb_rl_cb.Text != ""
               )
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL update_user(@p_usr_id, @p_surname, @p_name, @p_md_name, @p_email, @p_role, NULL)";

                    cmd.Parameters.AddWithValue("p_usr_id", user_id);
                    cmd.Parameters.AddWithValue("p_surname", tb_sr_n_cb.Text);
                    cmd.Parameters.AddWithValue("p_name", tb_n_cb.Text);
                    cmd.Parameters.AddWithValue("p_md_name", tb_md_n_cb.Text);
                    cmd.Parameters.AddWithValue("p_email", tb_em_cb.Text);
                    cmd.Parameters.AddWithValue("p_role", Convert.ToInt32(tb_rl_cb.Text));

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();



                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.","Ошибка, обнаружены пустые поля");
            }

            if(
                tb_lg_cb.Text != "" && tb_nw_pss_cb.Text != "" && tb_rp_nw_pss_cb.Text != ""
              )
            {
                if (tb_nw_pss_cb.Text == tb_rp_nw_pss_cb.Text)
                {

                    if (Conntion_to_DB())
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "CALL update_LP_user(@p_usr_id, @p_login, @p_pass, NULL)";

                        cmd.Parameters.AddWithValue("p_usr_id", user_id);
                        cmd.Parameters.AddWithValue("p_login", tb_lg_cb.Text);
                        cmd.Parameters.AddWithValue("p_pass", tb_nw_pss_cb.Text);

                        var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        cmd.Parameters.Add(rtrn);

                        cmd.ExecuteNonQuery();

                        DisConntion_to_DB();
                    }

                }
                else
                {
                    lb_err_new_pss_cb.Visible=true;
                }
            }
        }

        private void tb_nmb_pg_m_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод нецифровых символов
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //if (tb_nmb_pg_m.Text != "")
            //{

            //    int usr_page = Convert.ToInt32(tb_nmb_pg_m.Text);

            //    tb_nmb_pg_m.ForeColor = Color.Black;

            //    if (usr_page > 0 && usr_page <= count_page_pb)
            //    {

            //    }
            //    else
            //    {
            //        tb_nmb_pg_m.ForeColor = Color.Red;
            //    }
            //}
        }

        public static int DivideUp(int dividend)
        {
            return (dividend + 1) / 2;
        }

        private void bt_create_new_pub_Click(object sender, EventArgs e)
        {
            tb_text_pub.Enabled=true;
            tb_title_pub.Enabled = true;
            bt_edit_pub.Enabled = false;

            bt_save_pub.Enabled = true;
            bt_cancel_pub.Enabled = true;

            bt_save_pub.Visible = true;
            bt_cancel_pub.Visible = true;

            bt_edit_pub.BackColor = Color.DimGray;
            bt_create_new_pub.BackColor = Color.GreenYellow;
            bt_create_new_pub.Enabled = false;

        }

        private void bt_save_pub_Click(object sender, EventArgs e)
        {
            if (bt_create_new_pub.BackColor == Color.GreenYellow)
            {
                if (tb_title_pub.Text != "" && tb_text_pub.Text !="")
                {
                    insert_publication(user_id);
                }
                //тут можно месадж бокс вызвать с ошибкой
            }
            else 
            {
                if (tb_title_pub.Text != "" && tb_text_pub.Text != "")
                {
                    update_publication(public_id_tmp);
                }
                //тут можно месадж бокс вызвать с ошибкой
            }

            bt_cancel_pub_Click(sender, e);

            bt_my_public_Click(sender,e);


        }

        private void bt_delete_pub_Click(object sender, EventArgs e)
        {
            delete_publication(public_id_tmp);

            bt_cancel_pub_Click(sender, e);

            bt_my_public_Click(sender, e);
        }

        private void bt_cancel_pub_Click(object sender, EventArgs e)
        {
            tb_text_pub.Enabled = false;
            tb_title_pub.Enabled = false;

            tb_text_pub.Text = "";
            tb_title_pub.Text = "";

            bt_create_new_pub.Enabled = true;
            bt_edit_pub.Enabled = true;

            bt_save_pub.Enabled = false;
            bt_cancel_pub.Enabled = false;
            bt_delete_pub.Enabled = false;

            bt_save_pub.Visible = false;
            bt_cancel_pub.Visible = false;
            bt_delete_pub.Visible = false;
            
            bt_edit_pub.BackColor = Color.WhiteSmoke;
            bt_create_new_pub.BackColor = Color.WhiteSmoke;

        }

        private void bt_edit_pub_Click(object sender, EventArgs e)
        {
            tb_text_pub.Enabled = true;
            tb_title_pub.Enabled = true;
            bt_create_new_pub.Enabled = false;

            bt_save_pub.Enabled = true;
            bt_cancel_pub.Enabled = true;
            bt_delete_pub.Enabled = true;

            bt_save_pub.Visible = true;
            bt_cancel_pub.Visible = true;
            bt_delete_pub.Visible = true;

            bt_create_new_pub.BackColor = Color.DimGray;
            bt_edit_pub.BackColor = Color.GreenYellow;

            bt_edit_pub.Enabled = false;
        }

        public int update_publication(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CALL update_publication(@p_usr_id, @p_text, @p_title, NULL)";

                cmd.Parameters.AddWithValue("p_usr_id", id);
                cmd.Parameters.AddWithValue("p_text", Convert.ToString(tb_text_pub.Text));
                cmd.Parameters.AddWithValue("p_title", Convert.ToString(tb_title_pub.Text));

                var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(rtrn);

                cmd.ExecuteNonQuery();
                
                DisConntion_to_DB();

                return Convert.ToInt32(rtrn.Value);
            }

            return -1;
        }

        public int delete_publication(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CALL delete_publication(@p_pb_id, NULL)";

                cmd.Parameters.AddWithValue("p_pb_id", id);

                var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(rtrn);

                cmd.ExecuteNonQuery();

                DisConntion_to_DB();

                return Convert.ToInt32(rtrn.Value);
            }

            return -1;
        }

        public int insert_publication(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CALL add_new_publication(@p_pb_id, @p_text, @p_title)";

                cmd.Parameters.AddWithValue("p_pb_id", id);
                cmd.Parameters.AddWithValue("p_text", Convert.ToString(tb_text_pub.Text));
                cmd.Parameters.AddWithValue("p_title", Convert.ToString(tb_title_pub.Text));

                cmd.ExecuteNonQuery();

                DisConntion_to_DB();

                return 1;
            }

            return -1;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                public_id_tmp = Convert.ToInt32(selectedRow.Cells["pb_id"].Value);

                tb_text_pub.Text = selectedRow.Cells["text_p"].Value.ToString();
                tb_title_pub.Text = selectedRow.Cells["title_p"].Value.ToString();
            }
        }

        private void bt_ed_prs_Click(object sender, EventArgs e)
        {
            bt_save_ed_prs.Visible = true;
            bt_cancel_ed_prs.Visible=true;
            bt_ed_prs.Enabled = false;
            cb_level_rl_prs.Enabled = true;

        }

        private void bt_save_ed_prs_Click(object sender, EventArgs e)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CALL update_user_role(@p_usr_id, @p_role, NULL)";

                int val = cb_level_rl_prs.SelectedIndex + 2; //так как отсчет идет от 0 и есть еще роль админа

                cmd.Parameters.AddWithValue("p_usr_id", tmp);
                cmd.Parameters.AddWithValue("p_role", val);

                var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(rtrn);

                cmd.ExecuteNonQuery();

                DisConntion_to_DB();
            }

            bt_cancel_ed_prs_Click(sender,e);

            get_all_users();

        }

        private void bt_cancel_ed_prs_Click(object sender, EventArgs e)
        {
            bt_save_ed_prs.Visible = false;
            bt_cancel_ed_prs.Visible = false;
            bt_ed_prs.Enabled = true;
            cb_level_rl_prs.Enabled = false;
        }

        private void dgv_persons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_persons.SelectedRows.Count > 0)
            {
                lb_nm_prs.Text = "ФИО: ";

                DataGridViewRow selectedRow = dgv_persons.SelectedRows[0];

                tmp = Convert.ToInt32(selectedRow.Cells["p_usr_id"].Value);

                string sr_nm = selectedRow.Cells["p_surname"].Value.ToString();
                string nm = selectedRow.Cells["p_name"].Value.ToString();
                string md_nm = selectedRow.Cells["p_middle_name"].Value.ToString();

                lb_nm_prs.Text += sr_nm + " " + nm + " " + md_nm;
            }
        }

        private void dgv_pj_all_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_pj_all.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_pj_all.SelectedRows[0];

                tmp = Convert.ToInt32(selectedRow.Cells["prj_id"].Value);

                prj_name_tb.Text = selectedRow.Cells["prj_name"].Value.ToString();
                prj_desc_tb.Text = selectedRow.Cells["prj_discription"].Value.ToString();

                get_employees_by_project(tmp);
                get_research_by_project(tmp);
            }
        }

        public void get_employees_by_project(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_employees_by_project(@prj_id)";

                cmd.Parameters.AddWithValue("prj_id", id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_pj_empl.DataSource = dt;


                DisConntion_to_DB();
            }
        }

        public void get_research_by_project(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_research_by_project(@prj_id)";

                cmd.Parameters.AddWithValue("prj_id", id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgb_pj_srch.DataSource = dt;


                DisConntion_to_DB();
            }
        }

        private void dgv_pj_empl_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dgv_pj_empl_MultiSelectChanged(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.SelectedRows.Count;

            // Проверяем, есть ли выделенные строки
            if (selectedRowCount > 0)
            {
                // Создаем список для хранения значений из первого столбца
                List<string> firstColumnValues = new List<string>();

                // Перебираем все выделенные строки
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Получаем значение из первого столбца (индекс 0)
                    string value = row.Cells[0].Value?.ToString();
                    if (value != null)
                    {
                        firstColumnValues.Add(value);
                    }
                }

                // Выводим значения из первого столбца
                string valuesMessage = string.Join(", ", firstColumnValues);
                MessageBox.Show($"Значения из первого столбца: {valuesMessage}");
            }
        }

        private void dgv_all_rst_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_all_rst.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_all_rst.SelectedRows[0];

                tmp = Convert.ToInt32(selectedRow.Cells["rst_id"].Value);
                rst_name_tb.Text = selectedRow.Cells["rst_name"].Value.ToString();
                rst_dic_tb.Text = selectedRow.Cells["rst_discription"].Value.ToString();

            }
        }

        private void bt_rst_add_Click(object sender, EventArgs e)
        {
            bt_rst_ed.Enabled = false;
            rst_name_tb.ReadOnly = false;
            rst_dic_tb.ReadOnly = false;

            bt_rst_sv.Enabled = true;
            bt_rst_sv.Visible = true;

            bt_rst_cnl.Enabled = true;
            bt_rst_cnl.Visible = true;

            get_all_name_proj();
        }

        private void bt_rst_ed_Click(object sender, EventArgs e)
        {
            bt_rst_add.Enabled = false;
            rst_name_tb.ReadOnly = false;
            rst_dic_tb.ReadOnly = false;

            bt_rst_sv.Enabled = true;
            bt_rst_sv.Visible = true;

            bt_rst_cnl.Enabled = true;
            bt_rst_cnl.Visible = true;

            bt_rst_del.Enabled = true;
            bt_rst_del.Visible = true;

            get_all_name_proj();

        }

        private void bt_rst_sv_Click(object sender, EventArgs e)
        {

            if(bt_rst_add.Enabled == false)
            {
                update_research();
            }
            else
            {
                add_reasearch();
            }

            bt_rst_cnl_Click(sender,e);
            get_all_research();
        }

        private void bt_rst_cnl_Click(object sender, EventArgs e)
        {
            rst_name_tb.ReadOnly = true;
            rst_dic_tb.ReadOnly = true;

            bt_rst_add.Enabled = true;
            bt_rst_ed.Enabled = true;

            bt_rst_sv.Enabled = false;
            bt_rst_sv.Visible = false;

            bt_rst_cnl.Enabled = false;
            bt_rst_cnl.Visible = false;

            rst_name_tb.Text    = "";
            rst_dic_tb.Text     = "";
            rst_prj_nm_tb.Text  = "";

            bt_rst_del.Enabled = false;
            bt_rst_del.Visible = false;

            get_all_research();
            dgv_labs_rst.DataSource = null;
        }

        public void update_research()
        {
            if (rst_name_tb.Text != "" && rst_dic_tb.Text != "" && rst_prj_nm_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL update_research(@p_rst_id, @p_name, @p_discription, @p_prj_name, NULL)";

                    cmd.Parameters.AddWithValue("p_rst_id", tmp);
                    cmd.Parameters.AddWithValue("p_name", rst_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_discription", rst_dic_tb.Text);
                    cmd.Parameters.AddWithValue("p_prj_name", rst_prj_nm_tb.Text);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }
        }


        public void add_reasearch()
        {
            if (rst_name_tb.Text != "" && rst_dic_tb.Text != "" && rst_prj_nm_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL add_reasearch(@p_name, @p_discription, @p_prj_name)";

                    cmd.Parameters.AddWithValue("p_name", rst_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_discription", rst_dic_tb.Text);
                    cmd.Parameters.AddWithValue("p_prj_name", rst_prj_nm_tb.Text);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }
        }

        public void get_all_name_proj()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_all_name_proj()";

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_labs_rst.DataSource = dt;

                DisConntion_to_DB();
            }
        }

        private void dgv_labs_rst_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_labs_rst.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_labs_rst.SelectedRows[0];

                rst_prj_nm_tb.Text = selectedRow.Cells["prj_name"].Value.ToString();
            }
        }

        private void bt_rst_del_Click(object sender, EventArgs e)
        {

            if (rst_name_tb.Text != "" && rst_dic_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL delete_research(@p_rst_id, NULL)";

                    cmd.Parameters.AddWithValue("p_rst_id", tmp);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    if(Convert.ToInt32(rtrn.Value) == 1)
                    {
                        MessageBox.Show("Запись удалена", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка", "Не удача");
                    }

                    DisConntion_to_DB();


                    bt_rst_cnl_Click(sender,e);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись, которую желаете удалить", "Ошибка, запись не выбрана");
            }
        }

        private void dgv_all_lab_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_all_lab.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_all_lab.SelectedRows[0];

                lab_name_tb.Text = selectedRow.Cells["lb_name"].Value.ToString();
                lab_dpt_tb.Text = selectedRow.Cells["dpt_name"].Value.ToString();

                tmp = Convert.ToInt32(selectedRow.Cells["lb_id"].Value);

                get_equipment(Convert.ToInt32(selectedRow.Cells["lb_id"].Value));

            }
        }

        public void get_equipment(int id)
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_equipment(@lab_id)";

                cmd.Parameters.AddWithValue("lab_id", id);

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);


                dgv_eq_lab.DataSource = dt;

                DisConntion_to_DB();
            }


        }


        private void bt_lab_cr_Click(object sender, EventArgs e)
        {
            bt_lab_ed.Enabled = false;

            bt_lab_sv.Enabled = true;
            bt_lab_cl.Enabled = true;

            bt_lab_sv.Visible = true;
            bt_lab_cl.Visible = true;

            lab_name_tb.Enabled = true;
            lab_dpt_tb.Enabled = true;

            lab_name_tb.Text = "";
            lab_dpt_tb.Text = "";
            
            dgv_all_lab.Enabled = false;

            get_name_dpt();
        }

        private void bt_lab_ed_Click(object sender, EventArgs e)
        {
            bt_lab_cr.Enabled = false;

            bt_lab_sv.Enabled = true;
            bt_lab_cl.Enabled = true;

            bt_lab_sv.Visible = true;
            bt_lab_cl.Visible = true;

            bt_lab_del.Enabled = true;
            bt_lab_del.Visible = true;

            lab_name_tb.Enabled = true;
            lab_dpt_tb.Enabled = true;

            dgv_all_lab.Enabled = false;

            get_name_dpt();
        }

        private void bt_lab_sv_Click(object sender, EventArgs e)
        {
            if (bt_lab_ed.Enabled == true)
            {
                update_lab();
            }
            else
            {
                add_lab();
            }

            bt_lab_cl_Click(sender,e);
        }

        private void bt_lab_cl_Click(object sender, EventArgs e)
        {
            bt_lab_cr.Enabled = true;
            bt_lab_ed.Enabled = true;

            bt_lab_sv.Enabled = false;
            bt_lab_cl.Enabled = false;

            bt_lab_sv.Visible = false;
            bt_lab_cl.Visible = false;

            bt_lab_del.Enabled = false;
            bt_lab_del.Visible = false;

            lab_name_tb.Enabled = false;
            lab_dpt_tb.Enabled = false;

            lab_name_tb.Text = "";
            lab_dpt_tb.Text = "";

            get_all_lab();
            dgv_all_lab.Enabled = true;
            dgv_dpt_lab.DataSource = null;
        }

        private void bt_lab_del_Click(object sender, EventArgs e)
        {

            delete_lab(tmp);


            bt_lab_cl_Click(sender, e);
        }



        public void update_lab()
        {
            if (lab_name_tb.Text != "" && lab_dpt_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL update_lab(@p_lab_id, @p_name, @p_dpt_name, NULL)";

                    cmd.Parameters.AddWithValue("p_lab_id", tmp);
                    cmd.Parameters.AddWithValue("p_name", lab_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_dpt_name", lab_dpt_tb.Text);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }
        }


        public void add_lab()
        {
            if (lab_name_tb.Text != "" && lab_dpt_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL add_lab(@p_name, @p_dpt_name)";

                    cmd.Parameters.AddWithValue("p_name", lab_name_tb.Text);
                    cmd.Parameters.AddWithValue("p_dpt_name", lab_dpt_tb.Text);

                    cmd.ExecuteNonQuery();

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Заполните пожалуйста поля.", "Ошибка, обнаружены пустые поля");
            }

        }

        public void delete_lab(int id)
        {
            if (lab_name_tb.Text != "" && lab_dpt_tb.Text != "")
            {
                if (Conntion_to_DB())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "CALL delete_lab(@p_lb_id, NULL)";

                    cmd.Parameters.AddWithValue("p_lb_id", tmp);

                    var rtrn = new NpgsqlParameter("p_rtrn", NpgsqlTypes.NpgsqlDbType.Text)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rtrn);

                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(rtrn.Value) == 1)
                    {
                        MessageBox.Show("Запись удалена", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка", "Не удача");
                    }

                    DisConntion_to_DB();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись, которую желаете удалить", "Ошибка, запись не выбрана");
            }
        }

        public void get_name_dpt()
        {
            if (Conntion_to_DB())
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM get_name_dpt()";

                DataTable dt = new DataTable();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);

                dgv_dpt_lab.DataSource = dt;

                DisConntion_to_DB();
            }
        }

        private void dgv_dpt_lab_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_dpt_lab.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_dpt_lab.SelectedRows[0];

                lab_dpt_tb.Text = selectedRow.Cells["dpt_name"].Value.ToString();

            }
        }  
    }
}
