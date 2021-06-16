using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UmaUmaTeio2.IO
{
    /// <summary>
    /// <see cref="Json"/> クラスは、JSON 形式のテキストファイルの読込/書込をサポートするためのクラスです。
    /// </summary>
    public class Json
    {
        #region Public Methods

        /// <summary>
        /// 指定したテキストファイルから、ジェネリック型 <typeparamref name="T"/> で指定された型のインスタンスのデータを読み取ります。
        /// </summary>
        /// <typeparam name="T">JSON 値を変換する型。</typeparam>
        /// <param name="filePath">JSON データを表すテキストファイルのパス。</param>
        /// <returns>JSON 値を <typeparamref name="T"/> に変換したインスタンス。</returns>
        public static T LoadFromFile<T>(string filePath) => LoadFromFile<T>(filePath, Encoding.UTF8);

        /// <summary>
        /// 指定したテキストファイルから、ジェネリック型 <typeparamref name="T"/> で指定された型のインスタンスのデータを読み取ります。
        /// </summary>
        /// <typeparam name="T">JSON 値を変換する型。</typeparam>
        /// <param name="filePath">JSON データを表すテキストファイルのパス。</param>
        /// <param name="encoding">テキストファイルのエンコーディング。</param>
        /// <returns>JSON 値を <typeparamref name="T"/> に変換したインスタンス。</returns>
        public static T LoadFromFile<T>(string filePath, Encoding encoding)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new IOException($"指定したファイルパス ({filePath}) は無効です。ファイルが存在しません。");
            }

            using var reader = new StreamReader(filePath, encoding);
            var text = reader.ReadToEnd();

            return Load<T>(text);
        }

        /// <summary>
        /// テキストから、ジェネリック型 <typeparamref name="T"/> で指定された型のインスタンスのデータを読み取ります。
        /// </summary>
        /// <typeparam name="T">JSON 値を変換する型。</typeparam>
        /// <param name="text">JSON データを表すテキスト。</param>
        /// <returns>JSON 値を <typeparamref name="T"/> に変換したインスタンス。</returns>
        public static T Load<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new IOException($"指定したテキストデータは空白です。JSON データが存在しません。");
            }

            return JsonSerializer.Deserialize<T>(text);
        }

        /// <summary>
        /// 指定した型 ジェネリック型 <typeparamref name="T"/> のデータを JSON 文字列データのテキストファイルに保存します。
        /// </summary>
        /// <typeparam name="T">JSON 値に変換する型。</typeparam>
        /// <param name="filePath">JSON データを保存するテキストファイルのパス。</param>
        /// <param name="data">JSON 値に変換するデータ。</param>
        /// <returns><paramref name="data"/> を表す JSON 文字列。</returns>
        public static void SaveToFile<T>(string filePath, T data) => SaveToFile(filePath, Encoding.UTF8, data);

        /// <summary>
        /// 指定した型 ジェネリック型 <typeparamref name="T"/> のデータを JSON 文字列データのテキストファイルに保存します。
        /// </summary>
        /// <typeparam name="T">JSON 値に変換する型。</typeparam>
        /// <param name="filePath">JSON データを保存するテキストファイルのパス。</param>
        /// <param name="encoding">テキストファイルのエンコーディング。</param>
        /// <param name="data">JSON 値に変換するデータ。</param>
        /// <returns><paramref name="data"/> を表す JSON 文字列。</returns>
        public static void SaveToFile<T>(string filePath, Encoding encoding, T data)
        {
            var text = ToText(data);
            using var writer = new StreamWriter(filePath, false, encoding);

            writer.Write(text);
        }

        /// <summary>
        /// 指定した型 ジェネリック型 <typeparamref name="T"/> のデータを JSON 文字列に変換します。
        /// </summary>
        /// <typeparam name="T">JSON 値に変換する型。</typeparam>
        /// <param name="data">JSON 値に変換するデータ。</param>
        /// <returns><paramref name="data"/> を表す JSON 文字列。</returns>
        public static string ToText<T>(T data)
        {
            return JsonSerializer.Serialize(data);
        }

        #endregion

    }
}
