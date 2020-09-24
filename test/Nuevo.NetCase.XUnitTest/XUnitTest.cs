using Nuevo.NetCase.TcmbKurlarImpl;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nuevo.NetCase.XUnitTest
{
    public class XUnitTest
    {
        [Fact]
        public void Alis_ShouldException_WhenInvalidKur()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Alis("GECERSIZ_KUR"));
        }

        [Fact]
        public void Satis_ShouldException_WhenInvalidKur()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Satis("GECERSIZ_KUR"));
        }

        [Fact]
        public void Alis_ShouldException_WhenInvalidType()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Alis("USD", "GECERSIZ_TYPE"));
        }

        [Fact]
        public void Satis_ShouldException_WhenInvalidType()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Satis("USD", "GECERSIZ_TYPE"));
        }

        [Fact]
        public void Getir_ShouldException_WhenInvalidKur()
        {
            var request = new TcmbKurRequest { Kod = "GECERSIZ_KOD", Tip = DovizType.ALIS };
            Assert.Throws<Exception>(() => new TcmbKurlar().Getir(request));
        }

        [Fact]
        public void Getir_ShouldException_WhenInvalidType()
        {
            var request = new TcmbKurRequest { Kod = "USD", Tip = "GECERSIZ_TYPE" };
            Assert.Throws<Exception>(() => new TcmbKurlar().Getir(request));
        }

        [Fact]
        public void GetirListe_ShouldException_WhenInvalidSort()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().GetirListe(DovizType.ALIS, (TcmbKurSort)3));
        }

        [Fact]
        public void GetirListe_ShouldException_WhenInvalidType()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().GetirListe("GECERSIZ_TIP", TcmbKurSort.ASC));
        }

        [Fact]
        public void GetirListe_ShouldException_WhenInvalidTypeWithoutSort()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().GetirListe("GECERSIZ_TIP"));
        }

        [Fact]
        public void Aktar_ShouldException_WhenInvalidSort()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Aktar(TcmbAktarFormat.JSON, (TcmbKurSort)3));
        }

        [Fact]
        public void Aktar_ShouldException_WhenInvalidFormat()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Aktar((TcmbAktarFormat)10, TcmbKurSort.ASC));
        }

        [Fact]
        public void Aktar_ShouldException_WhenInvalidFormatWithoutSort()
        {
            Assert.Throws<Exception>(() => new TcmbKurlar().Aktar((TcmbAktarFormat)10));
        }

        [Fact]
        public void Alis_CorrectResponse_WhenWithKur()
        {
            var result = new TcmbKurlar().Alis("USD");
            Assert.True(result is TcmbKurResponse);
        }

        [Fact]
        public void Satis_CorrectResponse_WhenWithKur()
        {
            var result = new TcmbKurlar().Satis("USD");
            Assert.True(result is TcmbKurResponse);
        }
        
        [Fact]
        public void Alis_CorrectResponse_WhenWithDovizType()
        {
            var result = new TcmbKurlar().Alis("USD",DovizType.ALIS);
            Assert.True(result is TcmbKurResponse);
        }

        [Fact]
        public void Satis_CorrectResponse_WhenWithDovizType()
        {
            var result = new TcmbKurlar().Satis("USD", DovizType.SATIS);
            Assert.True(result is TcmbKurResponse);
        }

        [Fact]
        public void Getir_CorrectResponse_WhenWithCorrectParameters()
        {
            var request = new TcmbKurRequest { Kod = "USD", Tip = DovizType.ALIS };
            var result = new TcmbKurlar().Getir(request);
            Assert.True(result is TcmbKurResponse);
        }

        [Fact]
        public void GetirListe_CorrectResponse_WhenWithDovizType()
        {
            var result = new TcmbKurlar().GetirListe(DovizType.ALIS);
            Assert.True(result is List<TcmbKurResponse>);
        }

        [Fact]
        public void GetirListe_CorrectResponse_WhenWithSort()
        {
            var result = new TcmbKurlar().GetirListe(DovizType.ALIS,TcmbKurSort.ASC);
            Assert.True(result is List<TcmbKurResponse>);
        }

        [Fact]
        public void Aktar_CorrectResponse_WhenWithFormat()
        {
            var result = new TcmbKurlar().Aktar(TcmbAktarFormat.CSV);
            Assert.True(result is string);
        }

        [Fact]
        public void Aktar_CorrectResponse_WhenWithSort()
        {
            var result = new TcmbKurlar().Aktar(TcmbAktarFormat.CSV, TcmbKurSort.ASC);
            Assert.True(result is string);
        }
    }
}
