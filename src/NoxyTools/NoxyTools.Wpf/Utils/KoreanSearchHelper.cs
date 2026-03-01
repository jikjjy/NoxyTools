using System;

namespace NoxyTools.Wpf.Utils
{
    /// <summary>
    /// 한글 초성 검색 및 부분 음절(종성 생략) 검색을 지원하는 유틸리티.
    /// <para>동작 규칙:</para>
    /// <list type="bullet">
    ///   <item>단독 자음(ㄱ-ㅎ) → 해당 초성을 가진 모든 음절과 매칭 (예: 'ㄱ' → '강','기','고','그' …)</item>
    ///   <item>종성 없는 음절(가,나,다 …) → 같은 초성+중성을 가진 모든 음절과 매칭 (예: '가' → '가','강','각','갈' …)</item>
    ///   <item>종성 있는 완성 음절(강,남 …) → 완전 일치만</item>
    ///   <item>비한글 문자 → 대소문자 무시 일치</item>
    /// </list>
    /// </summary>
    internal static class KoreanSearchHelper
    {
        // ── 한글 유니코드 상수 ──────────────────────────────────────────

        private const char SyllableBase = '\uAC00'; // 가
        private const char SyllableEnd  = '\uD7A3'; // 힣
        private const int  JungCount    = 21;
        private const int  JongCount    = 28;

        // 초성 19개 (유니코드 자모 블록 문자 순서와 동일)
        private static readonly char[] Chosungs =
        {
            'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ',
            'ㅅ','ㅆ','ㅇ','ㅈ','ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };

        // ── 공개 API ───────────────────────────────────────────────────

        /// <summary>
        /// <paramref name="source"/> 안에 <paramref name="query"/>에 해당하는
        /// 부분 문자열이 있으면 true를 반환합니다.
        /// </summary>
        public static bool KoreanContains(string source, string query)
        {
            if (string.IsNullOrEmpty(query))  return true;
            if (string.IsNullOrEmpty(source)) return false;

            // 한글이 없으면 기존 OrdinalIgnoreCase 사용
            if (!ContainsHangul(query))
                return source.Contains(query, StringComparison.OrdinalIgnoreCase);

            // 슬라이딩 윈도우로 각 위치에서 매칭 시도
            int maxStart = source.Length - query.Length;
            for (int si = 0; si <= maxStart; si++)
            {
                if (MatchAt(source, si, query))
                    return true;
            }
            return false;
        }

        // ── 내부 로직 ──────────────────────────────────────────────────

        private static bool MatchAt(string source, int si, string query)
        {
            for (int qi = 0; qi < query.Length; qi++)
            {
                if (!CharMatch(source[si + qi], query[qi]))
                    return false;
            }
            return true;
        }

        private static bool CharMatch(char src, char query)
        {
            // 완전 일치
            if (src == query) return true;

            // 쿼리가 단독 자음(초성 검색) ─────────────────────────────
            int qChoIdx = GetStandaloneChosungIndex(query);
            if (qChoIdx >= 0)
            {
                return IsSyllable(src) && GetSyllableChosungIndex(src) == qChoIdx;
            }

            // 쿼리가 완성 음절 + 소스도 완성 음절 ────────────────────────
            if (IsSyllable(query) && IsSyllable(src))
            {
                DecomposeSyllable(query, out int qCho, out int qJung, out int qJong);
                DecomposeSyllable(src,   out int sCho, out int sJung, out _);

                // 종성 없는 쿼리면 초성+중성만 비교 (받침 있어도 OK)
                if (qJong == 0)
                    return qCho == sCho && qJung == sJung;
            }

            // 비한글 문자: 소문자 비교
            if (!ContainsHangul(query.ToString()) && !ContainsHangul(src.ToString()))
                return char.ToLowerInvariant(src) == char.ToLowerInvariant(query);

            return false;
        }

        // ── 한글 분해 헬퍼 ─────────────────────────────────────────────

        private static bool IsSyllable(char c) => c >= SyllableBase && c <= SyllableEnd;

        private static void DecomposeSyllable(char c, out int cho, out int jung, out int jong)
        {
            int offset = c - SyllableBase;
            jong = offset % JongCount;
            jung = (offset / JongCount) % JungCount;
            cho  = offset / (JongCount * JungCount);
        }

        private static int GetSyllableChosungIndex(char c)
        {
            DecomposeSyllable(c, out int cho, out _, out _);
            return cho;
        }

        /// <summary>독립 자모(ㄱ-ㅎ)를 초성 인덱스로 변환. 해당 없으면 -1.</summary>
        private static int GetStandaloneChosungIndex(char c)
            => Array.IndexOf(Chosungs, c);

        private static bool ContainsHangul(string s)
        {
            foreach (char c in s)
                if ((c >= '\u3131' && c <= '\u318E') || IsSyllable(c))
                    return true;
            return false;
        }
    }
}
