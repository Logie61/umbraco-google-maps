using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(GoogleMaps.PropertyEditor.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace GoogleMaps.PropertyEditor.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}