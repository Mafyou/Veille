using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PoC.MySF.Web
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée du processus d'hébergement du service.
        /// </summary>
        private static void Main()
        {
            try
            {
                // Le fichier ServiceManifest.XML définit un ou plusieurs noms de types de service.
                // L'inscription d'un service entraîne le mappage d'un nom de type de service à un type .NET.
                // Quand Service Fabric crée une instance de ce type de service,
                // une instance de la classe est créée dans le processus hôte.

                ServiceRuntime.RegisterServiceAsync("PoC.MySF.WebType",
                    context => new Web(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Web).Name);

                // Empêche ce processus hôte de se terminer, ce qui permet aux services de continuer à s'exécuter. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
