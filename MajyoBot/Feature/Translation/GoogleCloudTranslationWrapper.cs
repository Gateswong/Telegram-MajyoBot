using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using System;
using MajyoBot.Utility;

namespace MajyoBot.Feature.Translation
{
    public class GoogleCloudTranslationWrapper
    {
        public GoogleCloudTranslationWrapper()
        {
            client = TranslationClient.Create(AppConfiguration.GoogleAuth.GoogleCredential);
        }

        private TranslationClient client;

        public string Translate(string text, string targetLanguage, string sourceLanguage=null) 
        {
            var result = client.TranslateText(text, targetLanguage, sourceLanguage);
            return result.TranslatedText;
        }

        private static GoogleCloudTranslationWrapper _Default;
        public static GoogleCloudTranslationWrapper Default 
        {
            get 
            {
                if (_Default == null) 
                {
                    _Default = new GoogleCloudTranslationWrapper();                    
                }

                return Default;
            }
        }
    }
}
