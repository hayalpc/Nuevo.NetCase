using System;
using System.Collections.Generic;
using System.Text;

namespace Nuevo.NetCase.TcmbKurlarImpl
{
    public interface ITcmbKurlar
    {
        TcmbKurResponse Alis(string kur);
        TcmbKurResponse Alis(string kur, string type);
        TcmbKurResponse Satis(string kur);
        TcmbKurResponse Satis(string kur, string type);
        TcmbKurResponse Getir(TcmbKurRequest request);
        List<TcmbKurResponse> GetirListe(string type);
        List<TcmbKurResponse> GetirListe(string type, TcmbKurSort sort);
        string Aktar(TcmbAktarFormat format);
        string Aktar(TcmbAktarFormat format, TcmbKurSort sort);
    }
}
