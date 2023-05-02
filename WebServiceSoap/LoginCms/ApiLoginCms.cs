using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.IO;
using System;
using System.Text;
using System.Diagnostics;

namespace WebServiceSoap.LoginCms;

internal class ApiLoginCms
{
    internal void CreateCms()
    {
        StringContent content = new(GetXmlRequest(), Encoding.UTF8,"application/xml");
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(currentDirectory);

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoginCms", "input.txt");

        // Ruta al archivo de entrada a firmar
        string inputFile = filePath;

        // Ruta al archivo de salida para guardar el mensaje CMS firmado
        string outputFile = "output.cms";

        // Ruta a la clave privada y certificado del firmante
        string privateKeyFile = "MiClavePrivada.key";
        string certificateFile = "certificado.pem";

        // Construir el comando de OpenSSL para firmar el archivo de entrada
        string command = $"cms -sign -in {inputFile} -out {outputFile} -signer {certificateFile} -inkey {privateKeyFile}";

        // Crear un proceso para ejecutar el comando de OpenSSL
        ProcessStartInfo startInfo = new("openssl", command);
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        Process process = new();
        process.StartInfo = startInfo;

        // Ejecutar el proceso de OpenSSL y esperar a que termine
        process.Start();
        process.WaitForExit();

        // Leer la salida y el error estándar del proceso de OpenSSL
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        // Imprimir la salida y el error en la consola
        Console.WriteLine($"Salida: {output}");
        Console.WriteLine($"Error: {error}");
    }

    private string GenerateXmlRequest()
    {
        return $"""
            <loginTicketRequest>
                <header>
                    <uniqueId>
            """;
    }
}
