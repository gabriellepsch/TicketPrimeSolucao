namespace billet_2.Services;

public class QrCodeService
{
    public string GerarQrCodeBase64(string dados)
    {
        using (var qrGenerator = new QRCoder.QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(dados, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using (var pngQrCode = new QRCoder.PngByteQRCode(qrCodeData))
            {
                var pngBytes = pngQrCode.GetGraphic(20);
                return Convert.ToBase64String(pngBytes);
            }
        }
    }
}
