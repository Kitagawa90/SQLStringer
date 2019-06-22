using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Microsoft.Win32;

namespace SQLStringer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private static readonly string strFilter = "テキスト ファイル(.txt)|*.txt|SQL ファイル(*.SQL)|*.SQL|すべてのファイル|*.*";
        private string strFilePath { get; set; } = "";
        //起動パス
        private static readonly string IniPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Ini.xml";

        public clsIni Ini;

        private XmlSerializer XS = new XmlSerializer();

        public MainWindow()
        {
            InitializeComponent();
            //iniファイル読み込み
            LoadIni();
            Ini.setWordCcombo(cmbStart, cmbEnd);
        }

        #region"ボタン"

        /// <summary>
        /// RUNクリック
        /// </summary>
        private void RunClick(object sender, RoutedEventArgs e)
        {
            //今後分岐追加

            if (InputCheck() == false)
            {
                return;
            }
            AddText();
            Ini.setWord(cmbStart.Text, cmbEnd.Text);
            Ini.setWordCcombo(cmbStart, cmbEnd);
        }

        /// <summary>
        /// Removeクリック
        /// </summary>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {

            if (InputCheck() == false)
            {
                return;
            }

            RemoveText();
            Ini.setWord(cmbStart.Text, cmbEnd.Text);
            Ini.setWordCcombo(cmbStart, cmbEnd);

        }


        /// <summary>
        /// 全コピークリック
        /// </summary>
        private void AllCopy_Click(object sender, RoutedEventArgs e)
        {
            AllCopy();
        }



        #endregion

        #region"メニュー"

        /// <summary>
        /// 読み込みクリック
        /// </summary>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        /// <summary>
        ///保存ボタンクリック
        /// </summary>
        private void SaveNew_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// やり直し
        /// </summary>
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            Redo();
        }

        /// <summary>
        /// 終了
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 全選択
        /// </summary>
        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }

        /// <summary>
        /// 上書き保存
        /// </summary>
        private void Upatte_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        #endregion

        #region"イベント"

        /// <summary>
        /// フォームクローズ
        /// </summary>
        private void FormClosed(object sender, EventArgs e)
        {
            SaveIni();
        }

        #endregion

        #region"関数"

        /// <summary>
        /// txtSQL、各行の前後にに文字列追加
        /// </summary>
        private void AddText()
        {
            string strStart = cmbStart.Text;
            string strEnd = cmbEnd.Text;
            StringBuilder strSQL = new StringBuilder();
            List<string> strList = txtSQLText();


            //入力なしの場合は抜け
            if (strList.Count == 0)
            {
                return;
            }


            foreach (string strWk in strList)
            {
                //空白時は処理しない（今後変更するかも）
                if (strWk.Trim() == "")
                {
                    strSQL.AppendLine("");
                    continue;
                }
                //テキストの前後に対象文字列追加
                strSQL.AppendLine(strStart + strWk + strEnd);
            }
            //txtSQLに反映
            SettxtSQL(strSQL.ToString());
        }

        /// <summary>
        /// xtSQL、各行の前後にに文字列削除（未完
        /// </summary>
        private void RemoveText()
        {
            string strStart = cmbStart.Text;
            string strEnd = cmbEnd.Text;
            StringBuilder strSQL = new StringBuilder();
            List<string> strList = txtSQLText();

            //入力なしの場合は抜け
            if (strList.Count == 0)
            {
                return;
            }

            int Index;
            string wkText;

            foreach (string strWk in strList)
            {

                wkText = strWk;
                //開始文字の位置取得
                Index = wkText.IndexOf(strStart);

                if(Index > -1)//開始文字が含まれていれば
                {
                    //開始文字抜き取り
                    wkText = wkText.Remove(Index, strStart.Length);
                }

                //終了文字の位置取得
                Index = wkText.LastIndexOf(strEnd);

                if(Index > -1)//終了文字が含まれていれば
                {
                    //終了文字抜き取り
                    wkText = wkText.Remove(Index, strEnd.Length);
                }

                //文字追加
                strSQL.AppendLine(wkText);
               
            }
            //txtSQLに反映
            SettxtSQL(strSQL.ToString());


        }

        /// <summary>
        /// txtSQLのテキストをリストで取得
        /// </summary>
        /// <returns></returns>
        private List<string> txtSQLText()
        {
            FlowDocument document = this.txtSQL.Document;
            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
            List<string> listRet = new List<string>();
            string[] wkString = range.Text.Replace("\r\n", "\n").Split('\n');

            if (range.Text == "\r\n")
            {
                return listRet;
            }

            foreach (string wk in wkString)
            {
                listRet.Add(wk);
            }

            return listRet;
        }

        /// <summary>
        /// txtSQLにテキストセット
        /// </summary>
        /// <param name="valSetText">セットテキスト</param>
        private void SettxtSQL(string valSetText)
        {
            FlowDocument document = this.txtSQL.Document;
            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);

            range.Text = valSetText;

        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        private void Load()
        {
            string strPath;
            bool? Result;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = strFilter;
            Result = ofd.ShowDialog();

            strPath = ofd.FileName;

            //キャンセル時抜け
            if (Result == false)
            {
                return;
            }

            //ファイル有無確認
            if (File.Exists(strPath) == false)
            {
                return;
            }

            //ロード処理
            TextRange range = new TextRange(txtSQL.Document.ContentStart, txtSQL.Document.ContentEnd);
            using (FileStream fStream = new FileStream(strPath, FileMode.OpenOrCreate))
            {
                range.Load(fStream, DataFormats.Text);
                fStream.Close();
            }
            //ファイルパスメンバに代入
            strFilePath = strPath;

            //TODO:メッセージ表示


        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            string strPath;
            bool? Result;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = strFilter;
            //セーブファイルダイアログ表示
            Result = sfd.ShowDialog();

            strPath = sfd.FileName;

            //キャンセル時抜け
            if (Result == false)
            {
                return;
            }

            //書き込み処理
            FileWrite(strPath);

            //ファイルパスメンバに代入
            strFilePath = strPath;

            //TODO:メッセージ表示

        }


        /// <summary>
        /// 上書き保存
        /// </summary>
        private void Update()
        {
            //ファイルパスが存在しない場合はセーブファイルダイアログ表示
            if (File.Exists(strFilePath) == false)
            {
                Save();
                return;
            }

            //ファイル書き込み
            FileWrite(strFilePath);

            //TODO:メッセージ表示
        }

        /// <summary>
        /// 全コピー
        /// </summary>
        private void AllCopy()
        {
            TextRange range = new TextRange(txtSQL.Document.ContentStart, txtSQL.Document.ContentEnd);
            string strText = range.Text;
            Clipboard.SetText(strText);
            Clipboard.Flush();
        }

        /// <summary>
        /// ファイル書き込み
        /// </summary>
        /// <param name="strPath">ファイルパス</param>
        private void FileWrite(string strPath)
        {
            using (FileStream fStream = new FileStream(strPath, FileMode.Create))
            {
                TextRange range = new TextRange(txtSQL.Document.ContentStart, txtSQL.Document.ContentEnd);
                range.Save(fStream, DataFormats.Text);
                fStream.Close();
            }

        }

        /// <summary>
        /// iniファイル読み込み
        /// </summary>
        private void LoadIni()
        {
            if (File.Exists(IniPath) == true)
            {
                XS.Read<clsIni>(IniPath,ref  Ini);

            }
            else
            {
                Ini = new clsIni();
            }
        }

        /// <summary>
        /// iniファイル保存
        /// </summary>
        private void SaveIni()
        {
            XS.Save<clsIni>(IniPath, Ini);
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private bool InputCheck()
        {
            if (cmbStart.Text != "")
            {
                return true;
            }
            if (cmbEnd.Text != "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        private void Undo()
        {
            txtSQL.Undo();
        }

        /// <summary>
        /// Redo
        /// </summary>
        private void Redo()
        {
            txtSQL.Redo();
        }

        /// <summary>
        /// 全選択
        /// </summary>
        private void SelectAll()
        {
            txtSQL.SelectAll();
        }







        #endregion

  
    }

}
