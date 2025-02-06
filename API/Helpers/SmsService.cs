using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class SmsService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> SendSmsAsync(string receiver, string message)
    {
        // SOAP XML isteğini oluşturuyoruz
        string username = "samsunbsb.bim";
        string password = "W[v5p{o!*";
        string userCode = "3473";
        string accountId = "3320";
        string originator = "SAMSUN BSB";

        string receiverStr = $"<Receiver>{receiver}</Receiver>";

        string xmlPostString = $@"
        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sms=""https://webservice.asistiletisim.com.tr/SmsProxy"">
            <soapenv:Header/>
            <soapenv:Body>
                <sms:sendSms>
                    <sms:requestXml><![CDATA[
                        <SendSms>
                            <Username>{username}</Username>
                            <Password>{password}</Password>
                            <UserCode>{userCode}</UserCode>
                            <AccountId>{accountId}</AccountId>
                            <Originator>{originator}</Originator>
                            <SendDate></SendDate>
                            <ValidityPeriod>60</ValidityPeriod>
                            <MessageText>{message}</MessageText>
                            <IsCheckBlackList>0</IsCheckBlackList>
                            <ReceiverList>
                                {receiverStr}
                            </ReceiverList>
                        </SendSms>
                    ]]></sms:requestXml>
                </sms:sendSms>
            </soapenv:Body>
        </soapenv:Envelope>";

        // URL'yi belirtiyoruz
        string url = "https://webservice.asistiletisim.com.tr/SmsProxy.asmx?op=sendSms";

        // HTTP isteği başlıklarını ayarlıyoruz
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(xmlPostString, Encoding.UTF8, "text/xml")
        };

        // SOAPAction başlığını ekliyoruz
        requestMessage.Headers.Add("SOAPAction", "https://webservice.asistiletisim.com.tr/SmsProxy/sendSms");

        try
        {
            // İstek gönderiyoruz
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                // Yanıtı okuyoruz
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;  // SOAP yanıtını döndürüyoruz
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }
}
