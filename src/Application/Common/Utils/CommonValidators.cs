using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ConvenienceStoreApi.Application.Common.Utils;

public static class CommonValidators
{
    public static bool EsRfcValido(string rfc)
    {
        // Expresión regular para validar RFC
        string patronRfc = @"^([A-ZÑ&]{3,4})?\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])([A-Z\d]{3})$";

        // Validar longitud del RFC y aplicar la expresión regular
        return rfc.Length == 12 || rfc.Length == 13 && Regex.IsMatch(rfc, patronRfc, RegexOptions.IgnoreCase);
    }

    public static bool EsCorreoValido(string correo)
    {
        try
        {
            var mail = new MailAddress(correo);
            return true; // Si no lanza excepción, el correo es válido.
        }
        catch (FormatException)
        {
            return false; // Si lanza excepción, el formato del correo es inválido.
        }
    }
    public static bool EsTelefonoValido(string telefono)
    {
        // Validar si el número tiene un formato válido: código de país + número de teléfono (10 dígitos)
        string patronTelefono = @"^\d{2,3}\d{10}$";
        return Regex.IsMatch(telefono, patronTelefono);
    }
}