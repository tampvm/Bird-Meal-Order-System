
using BMOS.Models;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace BMOS.Models.Services
{
    public static class FirebaseService
    {
        private static string ApiKey = "AIzaSyBLDYkXtfdYnKseDJbz6J72lousbPIrniE";
        private static string Bucket = "bmosfile.appspot.com";
        private static string AuthEmail = "staff01@gmail.com";
        private static string AuthPassword = "123456";

        public static async Task<string> UploadImage(List<IFormFile> files, string folder)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            // get authentication token
            var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var authResult = await authResultTask;
            var token = authResult.FirebaseToken;


            string imageLink = "";
            FirebaseImageModel firebaseImageModel = new FirebaseImageModel();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    firebaseImageModel.ImageFile = file;
                    var stream = firebaseImageModel.ImageFile.OpenReadStream();
                    //you can use CancellationTokenSource to cancel the upload midway
                    var cancellation = new CancellationTokenSource();

                    var result = await new FirebaseStorage(Bucket,
                         new FirebaseStorageOptions
                         {
                             AuthTokenAsyncFactory = () => Task.FromResult(token)
                         })
                       .Child(folder)
                       .Child(firebaseImageModel.ImageFile.FileName)
                       .PutAsync(stream, cancellation.Token);

                    cancellation.Cancel();
                    try
                    {
                        imageLink += result + "datnt";

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return imageLink;

        }
    }
}