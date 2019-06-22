using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SQLStringer
{
    class XmlSerializer
    {
        public Exception ErrorException { get; private set; }

        public enum Result
        {
            Success,
            /// <summary>
            /// ファイルなし
            /// </summary>
            FileNotExist,
            /// <summary>
            /// ファイルパス不正
            /// </summary>
            FilePathError,
            /// <summary>
            /// ファイルアクセス不可
            /// </summary>
            FileUnAccessed
        }

        /// <summary>
        /// オブジェクト読み込み
        /// </summary>
        /// <typeparam name="Type">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <param name="ReadObject">読み込みオブジェクト</param>
        /// <returns></returns>
        public Result Read<Type>(string Path,ref Type ReadObject)
        {
            if (System.IO.File.Exists(Path) == false)
            {
                return Result.FileNotExist;
            }
            try
            {
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Type));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamReader sr = new System.IO.StreamReader(Path,  new System.Text.UTF8Encoding(false));
                //逆シリアル化し、XMLファイルに保存する
                ReadObject= (Type)serializer.Deserialize(sr);

                //ファイルを閉じる
                sr.Close();
                return Result.Success;
            }catch(System.IO.FileNotFoundException ex1){
                ErrorException = ex1;
                return Result.FileNotExist;
            }
            catch (System.IO.IOException ex2)
            {
                ErrorException = ex2;
                return Result.FileUnAccessed;
            }
            catch (System.UnauthorizedAccessException ex3)
            {
                ErrorException = ex3;
                return Result.FileUnAccessed;
            }
        }

        /// <summary>
        /// オブジェクト保存
        /// </summary>
        /// <typeparam name="type">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <param name="SaveObjrct">保存オブジェクト</param>
        /// <returns></returns>
        public Result Save<type>(string Path,type SaveObjrct)
        {
            try
            {
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(type));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Path, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, SaveObjrct);
                //ファイルを閉じる
                sw.Close();
                return Result.Success;
            }catch(UnauthorizedAccessException ex1)
            {
                ErrorException = ex1;
                return Result.FileUnAccessed;
            }catch(DirectoryNotFoundException ex2)
            {
                ErrorException = ex2;
                return Result.FilePathError;
            }catch(IOException ex3)
            {
                ErrorException = ex3;
                return Result.FileUnAccessed;
            }
            catch (System.Security.SecurityException ex4)
            {
                ErrorException = ex4;
                return Result.FileUnAccessed;
            }
        }


    }
}
