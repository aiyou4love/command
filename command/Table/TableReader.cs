using Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command
{
    public class TableReader : ICommand
    {
        public void runCommand(string[] nValue)
        {
            if (3 != nValue.Length)
            {
                string value_ = string.Format("TableReader nValue.Length != 2:{0}", nValue.Length);
                Message.runError(value_);
                return;
            }
            this.runDirectory(nValue[1], nValue[2]);
        }

        void runDirectory(string nDirectory, string nDestDirectory)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nDirectory);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                runFile(fileInfo_.FullName, nDestDirectory);
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string path_ = Path.Combine(nDirectory, suDirectory_.Name);
                runDirectory(path_, nDestDirectory);
            }
        }

        void runFile(string nFile, string nDestDirectory)
        {
            if (!nFile.EndsWith(".xlsx")) return;
            FileStream excelFile_ = File.Open(nFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader_ = ExcelReaderFactory.CreateOpenXmlReader(excelFile_);
            excelReader_.IsFirstRowAsColumnNames = true;
            DataSet excelData_ = excelReader_.AsDataSet();
            for (int i = 0; i < excelData_.Tables.Count; ++i)
            {
                runDataSet(excelData_.Tables[i]);
            }
            for (int i = 0; i < excelData_.Tables.Count; ++i)
            {
                runDataSet(excelData_.Tables[i], nDestDirectory);
            }
            excelReader_.Close();
            excelFile_.Close();
        }

        Dictionary<string, Dictionary<string, string>> mStrings = new Dictionary<string, Dictionary<string, string>>();
        void runDataSet(DataTable nDataTable)
        {
            if (!nDataTable.TableName.EndsWith(".i")) return;
            if (nDataTable.Columns.Count < 2) return;
            Dictionary<string, string> stringValue_ = new Dictionary<string, string>();
            for (int i = 0; i < nDataTable.Rows.Count; i++)
            {
                DataRow rows_ = nDataTable.Rows[i];
                string key_ = rows_[0].ToString();
                object value_ = rows_[1];
                if (value_.GetType() == typeof(double))
                {
                    double number_ = (double)value_;
                    if ((int)number_ == number_)
                    {
                        value_ = (int)number_;
                    }
                }
                stringValue_[key_] = value_.ToString();
            }
            mStrings[nDataTable.TableName] = stringValue_;
        }

        void Serialize<T>(string nFileName, T nT)
        {
            StreamWriter streamWriter_ = new StreamWriter(nFileName);
            JsonWriter jsonWriter_ = new JsonTextWriter(streamWriter_);
            JsonSerializer jsonSerializer_ = new JsonSerializer();
            jsonSerializer_.Serialize(jsonWriter_, nT);
            jsonWriter_.Close();
            streamWriter_.Close();
        }
        List<Dictionary<string, string>> mValues = new List<Dictionary<string, string>>();
        void runDataSet(DataTable nDataTable, string nDestDirectory)
        {
            if (nDataTable.TableName.EndsWith(".i")) return;

            mValues.Clear();

            if (nDataTable.Columns.Count <= 0) return;
            if (nDataTable.Rows.Count < 3) return;
            DataRow types_ = nDataTable.Rows[0];
            for (int i = 2; i < nDataTable.Rows.Count; i++)
            {
                Dictionary<string, string> row_ = new Dictionary<string, string>();
                DataRow values_ = nDataTable.Rows[i];
                foreach (DataColumn j in nDataTable.Columns)
                {
                    string type_ = (string)types_[j];
                    if ("null" == type_) continue;
                    object ovalue_ = values_[j];
                    if (ovalue_.GetType() == typeof(double))
                    {
                        double number_ = (double)ovalue_;
                        if ((int)number_ == number_)
                        {
                            ovalue_ = (int)number_;
                        }
                    }
                    string name_ = j.ToString();
                    string value_ = ovalue_.ToString();
                    string key_ = name_ + ".i";
                    if (mStrings.ContainsKey(key_))
                    {
                        if (mStrings[key_].ContainsKey(value_))
                        {
                            value_ = mStrings[key_][value_];
                        }
                    }
                    row_[name_] = value_;
                }
                mValues.Add(row_);
            }
            string path_ = Path.Combine(nDestDirectory, nDataTable.TableName);
            path_ += ".json";
            Serialize<List<Dictionary<string, string>>>(path_, mValues);
        }
    }
}
