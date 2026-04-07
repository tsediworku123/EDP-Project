using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    public static class CSVExporter
    {
        public static void ExportToCSV<T>(IEnumerable<T> items, string defaultFilename)
        {
            using (var sfd = new SaveFileDialog { Filter = "CSV Files|*.csv", FileName = defaultFilename })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        var props = typeof(T).GetProperties();
                        
                        // Header
                        for (int i = 0; i < props.Length; i++)
                        {
                            sb.Append(props[i].Name + (i == props.Length - 1 ? "" : ","));
                        }
                        sb.AppendLine();

                        // Rows
                        foreach (var item in items)
                        {
                            for (int i = 0; i < props.Length; i++)
                            {
                                string val = props[i].GetValue(item)?.ToString() ?? "";
                                if (val.Contains(",") || val.Contains("\"")) val = "\"" + val.Replace("\"", "\"\"") + "\"";
                                sb.Append(val + (i == props.Length - 1 ? "" : ","));
                            }
                            sb.AppendLine();
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString());
                        MessageBox.Show("Exported successfully to " + sfd.FileName, "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Export failed: " + ex.Message, "Error");
                    }
                }
            }
        }
    }
}
