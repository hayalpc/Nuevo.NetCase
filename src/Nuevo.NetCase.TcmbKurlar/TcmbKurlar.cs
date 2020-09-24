using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Nuevo.NetCase.TcmbKurlarImpl
{
    public class TcmbKurlar : ITcmbKurlar
    {
        private string ApiUrl { get; set; } = "https://www.tcmb.gov.tr/kurlar/today.xml";
        private string[] Kurlar = new string[] { "USD", "AUD", "DKK", "EUR", "GBP", "CHF", "SEK", "CAD", "KWD", "NOK", "SAR", "JPY", "BGN", "RON", "RUB", "IRR", "CNY", "PKR" };
        private Tarih_Date Data = null;
        private List<TcmbKurBilgi> Currency = new List<TcmbKurBilgi>();

        public TcmbKurResponse Alis(string kur)
        {
            return Alis(kur, DovizType.ALIS);
        }

        public TcmbKurResponse Alis(string kur, string type)
        {
            if (type == DovizType.ALIS || type == DovizType.EFEKTIFALIS)
                return Getir(new TcmbKurRequest { Kod = kur, Tip = type });
            else
                throw new Exception(ResultDescription.INVALID_TYPE);
        }

        public TcmbKurResponse Satis(string kur)
        {
            return Satis(kur, DovizType.SATIS);
        }

        public TcmbKurResponse Satis(string kur, string type)
        {
            if (type == DovizType.SATIS || type == DovizType.EFEKTIFSATIS)
                return Getir(new TcmbKurRequest { Kod = kur, Tip = type });
            else
                throw new Exception(ResultDescription.INVALID_TYPE);
        }

        public TcmbKurResponse Getir(TcmbKurRequest request)
        {
            var response = new TcmbKurResponse();
            try
            {
                if (Kurlar.Contains(request.Kod))
                {
                    switch (request.Tip)
                    {
                        case DovizType.ALIS:
                        case DovizType.EFEKTIFALIS:
                        case DovizType.EFEKTIFSATIS:
                        case DovizType.SATIS:
                            var currencyData = GetCurrency(request.Kod);
                            if (currencyData != null)
                            {
                                var value = currencyData.GetType().GetFields().Where(x => x.Name == request.Tip).FirstOrDefault().GetValue(currencyData).ToString();

                                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal Kur))
                                {
                                    response.ResultCode = ResultCode.SUCCESS;
                                    response.ResultDescription = ResultDescription.SUCCESS;
                                    response.Kur = Kur;
                                    response.Kod = request.Kod;
                                    response.Tip = request.Tip;
                                }
                                else
                                {
                                    throw new Exception(ResultDescription.INVALID_VALUE);
                                }
                            }
                            else
                            {
                                throw new Exception(ResultDescription.INVALID_DATA);
                            }
                            break;
                        default:
                            throw new Exception(ResultDescription.INVALID_TYPE);
                    }
                }
                else
                {
                    throw new Exception(ResultDescription.INVALID_KUR);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return response;
        }

        public List<TcmbKurResponse> GetirListe(string type)
        {
            return GetirListe(type, TcmbKurSort.ASC);
        }

        public List<TcmbKurResponse> GetirListe(string type, TcmbKurSort sort)
        {
            switch (type)
            {
                case DovizType.ALIS:
                case DovizType.EFEKTIFALIS:
                case DovizType.EFEKTIFSATIS:
                case DovizType.SATIS:
                    LoadData();
                    if (Currency.Count() > 0)
                    {
                        UseSort(sort);

                        var liste = Currency.Select(currencyData => new TcmbKurResponse
                        {
                            ResultCode = ResultCode.SUCCESS,
                            ResultDescription = ResultDescription.SUCCESS,
                            Tip = type,
                            Kod = currencyData.Kod,
                            Kur = decimal.Parse(currencyData.GetType().GetFields().Where(x => x.Name == type).FirstOrDefault().GetValue(currencyData).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture)
                        }).ToList();

                        return liste;
                    }
                    else
                    {
                        throw new Exception(ResultDescription.INVALID_DATA);
                    }
                default:
                    throw new Exception(ResultDescription.INVALID_DATA);
            }
        }

        public string Aktar(TcmbAktarFormat format)
        {
            return Aktar(format, TcmbKurSort.ASC);
        }

        public string Aktar(TcmbAktarFormat format, TcmbKurSort sort)
        {
            LoadData();
            if (Currency.Count() > 0)
            {
                UseSort(sort);

                var str = "";
                switch (format)
                {
                    case TcmbAktarFormat.CSV:
                        str = CsvConverter.ToCsv<TcmbKurBilgi>(",", Currency);
                        break;
                    case TcmbAktarFormat.JSON:
                        str = JsonConvert.SerializeObject(Currency);
                        break;
                    case TcmbAktarFormat.XML:
                        var xmlserializer = new XmlSerializer(typeof(List<TcmbKurBilgi>));
                        var stringWriter = new StringWriter();
                        using (var writer = XmlWriter.Create(stringWriter))
                        {
                            xmlserializer.Serialize(writer, Currency);
                            str = stringWriter.ToString();
                        }
                        break;
                    default:
                        throw new Exception(ResultDescription.INVALID_FORMAT);
                }
                return str;
            }
            else
            {
                throw new Exception(ResultDescription.INVALID_DATA);
            }
        }

        private TcmbKurBilgi GetCurrency(string kod)
        {
            if (Data == null)
            {
                LoadData();
            }
            return Currency.Where(x => x.CurrencyCode == kod).FirstOrDefault();
        }

        private Tarih_Date LoadData()
        {
            if (Data == null)
            {
                var resultingMessage = (Tarih_Date)new XmlSerializer(typeof(Tarih_Date)).Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(new HttpClient().GetStringAsync(ApiUrl).Result)));
                foreach (var currency in resultingMessage.Currency)
                {
                    if (Kurlar.Contains(currency.Kod))
                    {
                        Currency.Add(new TcmbKurBilgi
                        {
                            Unit = currency.Unit,
                            Isim = currency.Isim,
                            CurrencyName = currency.CurrencyName,
                            ForexBuying = currency.ForexBuying,
                            ForexSelling = currency.ForexSelling,
                            BanknoteBuying = currency.BanknoteBuying,
                            BanknoteSelling = currency.BanknoteSelling,
                            CrossRateUSD = currency.CrossRateUSD,
                            CrossRateOther = currency.CrossRateOther,
                            CrossOrder = currency.CrossOrder,
                            Kod = currency.Kod,
                            CurrencyCode = currency.CurrencyCode,
                        });
                    }
                }
                Data = resultingMessage;
            }
            return Data;
        }

        private void UseSort(TcmbKurSort sort)
        {
            if (sort == TcmbKurSort.DESC)
            {
                Currency = Currency.OrderByDescending(x => x.Kod).ToList();
            }
            else if (sort == TcmbKurSort.ASC)
            {
                Currency = Currency.OrderBy(x => x.Kod).ToList();
            }
            else
            {
                throw new Exception(ResultDescription.INVALID_SORT);
            }
        }
    }
}
