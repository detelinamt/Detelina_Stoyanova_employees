using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamLongestPEriod.Models;
using TeamLongestPEriod.Helpers;

namespace TeamLongestPEriod
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Pair of employees who have worked together";
            }

            lblError.Visible = false;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (fuFilePicker.HasFile)
            {
                if (fuFilePicker.PostedFile.FileName.Length > 0 && fuFilePicker.PostedFile.FileName.EndsWith(".csv"))
                {
                    var result = DataHelper.ReadCsvFile(fuFilePicker.PostedFile.InputStream);

                    if (!string.IsNullOrEmpty(result.Item2))
                    {
                        lblError.Text = result.Item2;
                        lblError.Visible = true;
                        return;
                    }

                    var teams = DataHelper.GetTeams(result.Item1);
                    dgTeamWorkingDays.DataSource = teams;
                    dgTeamWorkingDays.Caption = string.Format("Results from {0} file", fuFilePicker.PostedFile.FileName); 
                    dgTeamWorkingDays.DataBind();
                }
                else
                {
                    lblError.Text = "The file format is incorrect. Please choose .csv file!";
                    lblError.Visible = true;
                }
            }
        }
    }
}