using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace GiiChecker
{
    public partial class itemInfoLuaFixTool : Form
    {
        public itemInfoLuaFixTool()
        {
            InitializeComponent();
        }


        //参照ボタン
        private void srcButton_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            ofd.FileName = "";
            //はじめに表示されるフォルダを指定する

            ofd.InitialDirectory = @"C:";
            //[ファイルの種類]に表示される選択肢を指定する
            ofd.Filter = "itemInfo.luaファイル(*.lua)|*.lua|すべてのファイル(*.*)|*.*";

            //フィルターにluaが選択されている状態にする
            ofd.FilterIndex = 1;

            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //okが押された際パスをtextへ挿入
                pathTextBox.Text = ofd.FileName;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {


        }




        private async Task fileCheck()
        {
            

            var task = Task.Run(() =>
            {
                // textBox2.Focus()の実行


                //インプット
                StreamReader sr = new StreamReader(
                    pathTextBox.Text,
                    System.Text.Encoding.GetEncoding("shift_jis")
                    );

                //アウトプット(itemInfo.lua_fixと名前なる)

                string fileName = pathTextBox.Text.Replace(".lua", "_fix.lua");
                
                StreamWriter sw =
                    new StreamWriter(
                        fileName,
                        false,
                        System.Text.Encoding.GetEncoding("shift_jis")
                        );


                bool endFlag = false;

                while (sr.Peek() >= 0)
                {

                    

                    
                    //1行取り出す
                    string text = sr.ReadLine();

                    if (text == "--Function #0")
                    {
                        Console.WriteLine("aaaaaaaaaaaaaaaaaaa");
                        endFlag = true;
                    }



                    //修正①(",",　→   ",)に変換
                    text = text.Replace("\",\",", "\",");





                    //修正②(",",　→   ",)に変換


                    char[] cArray = text.ToCharArray();
                    StringBuilder sb = new StringBuilder();

                    int count = 0;
                    bool fastFlag = false;
                    foreach (char c in cArray)
                    {
                        //1回目の"がでてくるまではappend

                        if (fastFlag == true && c == '"' && (count != (cArray.Length - 2)) && (endFlag == false))
                        {

                            sb.Append("\\");
                            sb.Append(c);

                        }
                        else
                        {
                            sb.Append(c);
                        }

                        if (c == '"')
                        {

                            fastFlag = true;

                        }

                        //1回目が出てきたらflag_on
                        //最後のlength-1でflag無視


                        count++;

                    }


                    sw.WriteLine(sb.ToString());


                }
 

                sr.Close();
                sw.Close();

                MessageBox.Show("修正完了");
                
            });
        }

        //修正処理
        private void updateButton_Click(object sender, EventArgs e)
        {
            
            //luaファイル選択時実行
            if (pathTextBox.Text.EndsWith("lua"))
            {
                var Task = fileCheck();
                

            }
            else
            {
                MessageBox.Show("luaファイルを指定してください");

            }


        }





    }
}
