using System;
using System.Collections.Generic;
using System.Text;

namespace Nuevo.NetCase.TcmbKurlarImpl
{
    public class TcmbKurResponse
    {
        public ResultCode ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public decimal Kur { get; set; }
        public string Kod { get; set; }
        public string Tip { get; set; }
    }

    public enum ResultCode
    {
        SUCCESS = 0,
        FAIL = 1,
        EXCEPTION = 500,
    }

    public static class ResultDescription
    {
        public static string SUCCESS = "Başarılı";
        public static string FAIL = "Başarısız";
        public static string INVALID_KUR = "Geçersiz kur";
        public static string INVALID_TYPE = "Geçersiz tip";
        public static string INVALID_VALUE = "Geçersiz değer";
        public static string INVALID_SORT = "Geçersiz sıralama";
        public static string INVALID_FORMAT = "Geçersiz format";
        public static string INVALID_DATA = "Geçersiz data";
    }
}
