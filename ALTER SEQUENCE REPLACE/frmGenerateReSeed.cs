using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALTER_SEQUENCE_REPLACE
{
    public partial class frmGenerateReSeed : Form
    {
        public frmGenerateReSeed()
        {
            InitializeComponent();

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtResult.Text = string.Empty;

            StringBuilder sb = new StringBuilder();
                       
            sb.AppendLine("raise notice 'UPDATE ThisTable';");
            sb.AppendLine("select max(\"ThisColumn\") from public.\"ThisTable\" into maxid;");
            sb.AppendLine("if (maxid is not null) THEN");
            sb.AppendLine("execute 'alter SEQUENCE public.\"ThisTable_ThisColumn_seq\" RESTART with '|| maxid+1;");
            sb.AppendLine("raise notice 'SUCCESSFULLY UPDATED Sequence with ThisColumn: %', maxid;");
            sb.AppendLine("end if;");
            sb.AppendLine("if (maxid is null) THEN");
            sb.AppendLine("execute 'alter SEQUENCE public.\"ThisTable_ThisColumn_seq\" RESTART with '|| 1;");
            sb.AppendLine("raise notice 'SUCCESSFULLY UPDATED Sequence with ThisColumn: 1';");
            sb.AppendLine("end if;");

            var textToUpdate = sb.ToString();

            string scriptResult = string.Empty;

            StringBuilder sb1 = new StringBuilder();

            sb1.AppendLine("DO $$DECLARE maxid int;");
            sb1.AppendLine("BEGIN\n");

            scriptResult = sb1.ToString();

            for (int i = 0; i < txtTable.Lines.Count(); i++)
            {
                scriptResult = scriptResult + "\n" + textToUpdate.Replace("ThisTable", txtTable.Lines[i]).Replace("ThisColumn", txtPK.Lines[i]);
            }
            
            StringBuilder sb2 = new StringBuilder();

            sb2.Append(scriptResult + "\n");
            sb2.AppendLine("END");
            sb2.AppendLine("$$;");
            
            txtResult.Text = sb2.ToString();
        }
    }
}
