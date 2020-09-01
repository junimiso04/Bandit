﻿using System;

namespace Bandit.Utilities
{
    public static class Parser
    {
        private static void Resize(ref string[] array)
        {
            int i = array.Length;
            Array.Resize(ref array, i + 1);
            array[i] = null;
        }

        /// <summary>
        /// 현재 인스턴스를 지정한 두 문자열을 이용하여 파싱하고 그 결과를 반환합니다.
        /// </summary>
        /// <param name="text">파싱할 문자열입니다.</param>
        /// <param name="strStart">시작 문자열입니다.</param>
        /// <param name="strEnd">종료 문자열입니다.</param>
        /// <returns>단일 파싱</returns>
        public static string SingleParse(this string text, string strStart, string strEnd)
        {
            string Source = text;
            string Result = null;
            Source = Source.Substring(Source.IndexOf(strStart) + strStart.Length);
            Result = Source.Substring(0, Source.IndexOf(strEnd));
            return Result;
        }

        /// <summary>
        /// 현재 인스턴스를 지정한 두 문자열을 이용하여 역순으로 파싱하고 그 결과를 반환합니다.
        /// </summary>
        /// <param name="text">파싱할 문자열입니다.</param>
        /// <param name="strStart">시작 문자열입니다.</param>
        /// <param name="strEnd">종료 문자열입니다.</param>
        /// <returns>역순 단일 파싱</returns>
        public static string LastSingleParse(this string text, string strStart, string strEnd)
        {
            string Source = text;
            string Result = null;
            Source = Source.Substring(Source.LastIndexOf(strStart) + strStart.Length);
            Result = Source.Substring(0, Source.LastIndexOf(strEnd));
            return Result;
        }

        /// <summary>
        /// 현재 인스턴스를 지정한 두 문자열을 이용하여 전부 파싱하고 그 결과를 배열로 반환합니다.
        /// </summary>
        /// <param name="text">파싱할 문자열입니다.</param>
        /// <param name="strStart">시작 문자열입니다.</param>
        /// <param name="strEnd">종료 문자열입니다.</param>
        /// <returns>다중 파싱</returns>
        public static string[] MultipleParse(this string text, string strStart, string strEnd)
        {
            string Source = text;
            string[] Result = { null };
            int Count = 0;
            while (Source.IndexOf(strStart) > -1)
            {
                Resize(ref Result);
                Source = Source.Substring(Source.IndexOf(strStart) + strStart.Length);
                if (Source.IndexOf(strEnd) != -1)
                {
                    Result[Count] = Source.Substring(0, Source.IndexOf(strEnd));
                }
                else return Result;
                Count++;
            }
            return Result;
        }
    }
}
