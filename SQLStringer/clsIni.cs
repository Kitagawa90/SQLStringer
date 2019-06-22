using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace SQLStringer
{
   public class clsIni
    {
        /// <summary>
        /// 開始文字
        /// </summary>
        public List<string> listStartWord
        {
            get
            {
                if(_listStartWord == null)
                {
                    return new List<string>();
                }else
                {
                    return _listStartWord;
                }
            }
            set
            {
                _listStartWord = value;
            }
        }
        private List<string> _listStartWord = new List<string>();

        /// <summary>
        /// 終了文字
        /// </summary>
        public List<string> listEndWord
        {
            get
            {
                if (_listEndWord == null)
                {
                    return new List<string>();
                }else
                {
                    return _listEndWord;
                }
            }
            set
            {
                _listEndWord = value;
            }
        }
        private List<string> _listEndWord = new List<string>();


        public int maxCountListWord { get; set; } = 5;

        /// <summary>
        /// 入力ワードをリストに登録
        /// </summary>
        /// <param name="StartWord">開始ワード</param>
        /// <param name="EndWord">終了ワード</param>
        public void setWord(string StartWord,string EndWord)
        {
            setStringList(StartWord,  listStartWord);
            setStringList(EndWord, listEndWord);
        }

        /// <summary>
        /// コンボボックスにワードをセット
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        public void setWordCcombo(ComboBox Start,ComboBox End)
        {
            //コンボボックスに入力中の文字退避
            string wkStartText = Start.Text;
            string wkEndText = End.Text;

            //コンボ初期化
            Start.Items.Clear();
            End.Items.Clear();

            //FIFO
            listStartWord.Reverse();
            foreach (string wk in listStartWord)
            {
                Start.Items.Add(wk);
            }

            listEndWord.Reverse();
            foreach (string wk in listEndWord)
            {
                End.Items.Add(wk);
            }

            listStartWord.Reverse();
            listEndWord.Reverse();

            //退避したテキストをコンボに戻す
            Start.Text = wkStartText;
            End.Text = wkEndText;

        }


        /// <summary>
        /// ワードリストに単語を追加（FIFO）
        /// </summary>
        /// <param name="Word">入力単語</param>
        /// <param name="listWord">リスト</param>
        private void setStringList(string Word, List<string> listWord)
        {
            //要素がある場合。要検証
            if (listWord.Any(x => x == Word) == true)
            {
                listWord.Remove(Word);
                //末尾に追加
                listWord.Add(Word);
                return;
            }

            //登録可能最大数未満の場合
            if (listWord.Count < maxCountListWord)
            {
                //末尾に追加
                listWord.Add(Word);
                return;
            }

            //登録可能数の場合
            if (listWord.Count == maxCountListWord)
            {
                //先頭削除
                listWord.RemoveAt(0);
                //末尾に追加
                listWord.Add(Word);
                return;
            }
        }
    }
}
