using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaUmaTeio2.Text
{
    /// <summary>
    /// <see cref="JaroWinklerDistance"/> クラスは、２つの文字列の類似度 (0-1) で計算するクラスです。
    /// </summary>
    public class JaroWinklerDistance
    {
        #region Fields

        private const double SimThreshold = 0.7;

        private const int PrefixSize = 4;

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定した２つのテキスト <paramref name="text1"/> と <paramref name="text2"/> の Jaro-Winkler Distance の値を取得します。
        /// <para>
        /// 値の範囲は、0 (完全一致)～ 1 (不一致) を意味します。
        /// </para>
        /// </summary>
        /// <param name="text1">比較するテキスト１。</param>
        /// <param name="text2">比較するテキスト２。</param>
        /// <param name="comparer">利用する等価比較子。</param>
        /// <returns></returns>
        public static double Distance(string text1, string text2, IEqualityComparer<char> comparer = null) => 1.0D - Distance(text1, text2, comparer);

        /// <summary>
        /// 指定した２つのテキスト <paramref name="text1"/> と <paramref name="text2"/> の Jaro-Winkler Similarity の値を取得します。
        /// <para>
        /// 値の範囲は、1 (完全一致)～ 0 (不一致) を意味します。
        /// </para>
        /// </summary>
        /// <param name="text1">比較するテキスト１。</param>
        /// <param name="text2">比較するテキスト２。</param>
        /// <param name="comparer">利用する等価比較子。</param>
        /// <returns></returns>
        public static double Similarity(string text1, string text2, IEqualityComparer<char> comparer = null)
        {
            var len1 = text1.Length;
            var len2 = text2.Length;

            comparer = comparer ?? EqualityComparer<char>.Default;

            // 文字数が０のときの例外処理
            if (len1 == 0)
            {
                return len2 == 0 ? 1.0 : 0.0;
            }

            var sim = JaroSimilarity(text1, text2, comparer);

            // 一致度が閾値よりも低い場合は以下を省略
            if (sim <= SimThreshold) return sim;

            return JaroWinklerSimilarity(sim, text1, text2, comparer);
        }

        #endregion

        #region Private Methods

        private static double JaroSimilarity(string text1, string text2, IEqualityComparer<char> comparer = null)
        {
            var len1 = text1.Length;
            var len2 = text2.Length;

            var searchRange = Math.Max(0, Math.Max(len1, len2) / 2 - 1);
            var matched1 = new bool[len1];
            var matched2 = new bool[len2];
            var m = 0; // 近くで一致する文字数

            for (int i = 0; i < len1; i++)
            {
                var start = Math.Max(0, i - searchRange);
                var end = Math.Min(i + searchRange + 1, len2);

                for (int j = start; j < end; j++)
                {
                    if (matched2[j]) continue;
                    if (!comparer.Equals(text1[i], text2[j])) continue;

                    matched1[i] = true;
                    matched2[j] = true;

                    m += 1;
                    break;
                }
            }

            // 一致していない
            if (m == 0) return 0;

            var transposition2 = 0; // 転置 t
            var k = 0;

            for (int i = 0; i < len1; i++)
            {
                if (!matched1[i]) continue;

                while (!matched2[k])
                {
                    k += 1;
                }

                if (!comparer.Equals(text1[i], text2[k]))
                {
                    transposition2 += 1;
                }

                k += 1;
            }

            return ((double)m / len1 + (double)m / len2 + (m - (transposition2 / 2)) / (double)m) / 3.0D;
        }

        private static double JaroWinklerSimilarity(double sim, string text1, string text2, IEqualityComparer<char> comparer = null)
        {
            var len1 = text1.Length;
            var len2 = text2.Length;

            var position = 0;
            var max = Math.Min(PrefixSize, Math.Min(len1, len2));

            // 共通の接頭辞を持つか
            while (position < max && comparer.Equals(text1[position], text2[position]))
            {
                position += 1;
            }

            if (position == 0) return sim; // 例外

            return sim + 0.1 * position * (1 - sim);
        }

        #endregion
    }
}
